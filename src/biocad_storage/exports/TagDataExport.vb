Imports System.IO
Imports System.Runtime.CompilerServices
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1.Annotations
Imports BioNovoGene.BioDeep.Chemoinformatics
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes

Public Module TagDataExport

    ''' <summary>
    ''' export a set of the molecule id which is associated with a specific topic
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="tagName">the topic name</param>
    ''' <returns></returns>
    <Extension>
    Public Function ExportTagList(registry As biocad_registry, tagName As String) As IEnumerable(Of String)
        Dim tag_id = registry.vocabulary.where(field("term") = tagName).find(Of biocad_registryModel.vocabulary)

        If tag_id Is Nothing Then
            Throw New InvalidDataException($"missing  topic vocabulary term '{tagName}' inside registry.")
        Else
            Return From id As UInteger
                   In registry.molecule_tags _
                       .where(field("tag_id") = tag_id.id) _
                       .distinct() _
                       .project(Of UInteger)("molecule_id")
                   Select "BioCAD" & id.ToString.PadLeft(11, "0")
        End If
    End Function

    <Extension>
    Public Function ExportTagData(registry As biocad_registry, tagName As String) As MetaboliteAnnotation()
        Dim tag_id = registry.vocabulary.where(field("term") = tagName).find(Of biocad_registryModel.vocabulary)

        If tag_id Is Nothing Then
            Throw New InvalidDataException($"missing  topic vocabulary term '{tagName}' inside registry.")
        End If

        Return registry.molecule_tags _
            .left_join("molecule") _
            .on(field("`molecule`.id") = field("`molecule_tags`.molecule_id")) _
            .where(field("tag_id") = tag_id.id) _
            .distinct() _
            .select(Of MetaboliteAnnotation)("CONCAT('BioCAD', LPAD(`molecule`.id, 11, '0')) AS id",
                                             "name",
                                             "formula",
                                             "mass AS exact_mass")
    End Function

    Public Function ExportKEGGMetabolites(registry As biocad_registry) As IEnumerable(Of MetaboliteStructData)
        Return registry.ExportDatabaseMetabolites(registry.vocabulary_terms.kegg_term)
    End Function

    <Extension>
    Public Function ExportDatabaseMetabolites(registry As biocad_registry, db_key As UInteger) As IEnumerable(Of MetaboliteStructData)
        Return registry.db_xrefs _
            .left_join("molecule").on(field("`molecule`.id") = field("`db_xrefs`.obj_id")) _
            .left_join("sequence_graph").on(field("`sequence_graph`.molecule_id") = field("`molecule`.id")) _
            .where(field("db_key") = db_key,
                   field("`molecule`.type") = registry.vocabulary_terms.metabolite_term,
                   field("sequence").char_length > 0) _
            .select(Of MetaboliteStructData)(
                "CAST(molecule.id AS CHARACTER) AS id",
                "molecule.name",
                "formula",
                "mass AS exact_mass",
                "db_xrefs.xref AS xref_id",
                "sequence AS smiles")
    End Function

    Public Function ExportLipidmapsMetabolites(registry As biocad_registry) As IEnumerable(Of MetaboliteStructData)
        Return registry.ExportDatabaseMetabolites(registry.vocabulary_terms.lipidmaps)
    End Function

    Public Function ExportHMDBMetabolites(registry As biocad_registry) As IEnumerable(Of MetaboliteStructData)
        Return registry.ExportDatabaseMetabolites(registry.vocabulary_terms.hmdb_term)
    End Function

    <Extension>
    Public Iterator Function ExportTopicMetabolites(registry As biocad_registry, tagName As String) As IEnumerable(Of MetaboliteStructData())
        Dim page_size As Integer = 1000
        Dim tag_id As UInteger = registry.getVocabulary(tagName, "Topic")

        For page As Integer = 0 To Integer.MaxValue
            Dim page_data As MetaboliteStructData() = registry.molecule_tags _
                .left_join("molecule").on(field("`molecule`.id") = field("molecule_id")) _
                .left_join("sequence_graph").on(field("`sequence_graph`.molecule_id") = field("`molecule`.id")) _
                .where(field("tag_id") = tag_id,
                       field("formula").char_length > 0,
                       field("sequence").char_length > 0) _
                .limit(page * page_size, page_size) _
                .select(Of MetaboliteStructData)(
                    "CAST(molecule.id AS CHARACTER) AS id",
                    "xref_id",
                    "name",
                    "formula",
                    "sequence AS smiles")

            If page_data.IsNullOrEmpty Then
                Exit For
            End If

            For i As Integer = 0 To page_data.Length - 1
                page_data(i).exact_mass = FormulaScanner.EvaluateExactMass(page_data(i).exact_mass)
            Next

            Yield page_data
        Next
    End Function

    <Extension>
    Public Iterator Function ExportSmiles(registry As biocad_registry, id As IEnumerable(Of String)) As IEnumerable(Of MetaboliteStructData())
        Dim page_size As Integer = 100

        For Each page As UInteger() In TqdmWrapper.Wrap(id.Select(Function(s) UInteger.Parse(s.Match("\d+"))).SplitIterator(page_size).ToArray)
            Dim page_data As MetaboliteStructData() = registry.molecule _
                .left_join("sequence_graph").on(field("`sequence_graph`.molecule_id") = field("`molecule`.id")) _
                .where(field("`molecule`.id").in(page),
                       field("formula").char_length > 0,
                       field("sequence").char_length > 0) _
                .select(Of MetaboliteStructData)(
                    "CAST(molecule.id AS CHARACTER) AS id",
                    "xref_id",
                    "name",
                    "formula",
                    "sequence AS smiles")

            If page_data.IsNullOrEmpty Then
                Exit For
            End If

            For i As Integer = 0 To page_data.Length - 1
                page_data(i).exact_mass = FormulaScanner.EvaluateExactMass(page_data(i).exact_mass)
            Next

            Yield page_data
        Next
    End Function
End Module

Public Class MetaboliteStructData : Inherits MetaboliteAnnotation

    <DatabaseField> Public Property xref_id As String
    <DatabaseField> Public Property smiles As String

End Class

Public Class MetaboliteAnnotation
    Implements GenericCompound
    Implements IReadOnlyId, ICompoundNameProvider, IExactMassProvider, IFormulaProvider

    <DatabaseField> Public Property id As String Implements IReadOnlyId.Identity
    <DatabaseField> Public Property name As String Implements ICompoundNameProvider.CommonName
    <DatabaseField> Public Property formula As String Implements IFormulaProvider.Formula
    <DatabaseField> Public Property exact_mass As Double Implements IExactMassProvider.ExactMass

End Class