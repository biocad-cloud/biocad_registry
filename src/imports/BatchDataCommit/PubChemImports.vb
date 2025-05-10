Imports System.Runtime.CompilerServices
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.CrossReference
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
Imports BioNovoGene.BioDeep.Chemistry.NCBI.PubChem
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Linq
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Module PubChemImports

    <Extension>
    Public Sub RunDataCommit(registry As biocad_registry, pagedata As PugViewRecord())
        Dim trans As CommitTransaction = registry.molecule.open_transaction.ignore
        Dim terms = registry.vocabulary_terms
        Dim metadata As MetaLib() = pagedata _
            .Select(Function(m)
                        Try
                            Return m.GetMetaInfo
                        Catch ex As Exception
                            Call App.LogException(ex)
                        End Try

                        Return Nothing
                    End Function) _
            .Where(Function(m) Not m Is Nothing) _
            .ToArray

        For Each meta As MetaLib In metadata
            Dim mol As biocad_registryModel.molecule = registry.findMolecule(meta)

            If mol Is Nothing Then
                Dim mass As Double = FormulaScanner.EvaluateExactMass(meta.formula)

                ' add new 
                Call trans.add(
                    field("xref_id") = $"PubChem:{meta.ID}",
                    field("name") = meta.name,
                    field("mass") = If(mass < 0, 0, mass),
                    field("type") = terms.metabolite_term,
                    field("formula") = meta.formula,
                    field("parent") = 0,
                    field("tax_id") = 0,
                    field("note") = meta.description
                )
            End If
        Next

        Call trans.commit()
    End Sub

    <Extension>
    Private Function findMolecule(registry As biocad_registry, meta As MetaLib) As biocad_registryModel.molecule
        Dim cid As String = $"PubChem:{meta.ID}"

        ' find molecule table via xref_id directly at first
        Dim q = registry.molecule _
            .where(field("xref_id") = cid) _
            .find(Of biocad_registryModel.molecule)

        If Not q Is Nothing Then
            Return q
        End If

        ' find molecule via db_xrefs
        Dim filter As FieldAssert = FieldAssert.empty
        Dim xrefs As NamedValue(Of String)() = meta.xref.PopulateXrefs(True, True).ToArray
        Dim mass As Double = FormulaScanner.EvaluateExactMass(meta.formula)

        For Each xref In xrefs
            filter = filter Or (field("db_key") = registry.vocabulary_terms.GetDatabaseKey(xref.Name) And field("xref") = xref.Value)
        Next

        q = registry.molecule _
            .left_join("db_xrefs") _
            .on(field("`db_xrefs`.obj_id") = field("`molecule`.id")) _
            .where(field("mass").between(mass - 1, mass + 1), filter) _
            .find(Of biocad_registryModel.molecule)("`molecule`.*")

        If Not q Is Nothing Then
            Return q
        End If

        ' find molecule table via name
        q = registry.molecule _
            .where(field("mass").between(mass - 1, mass + 1),
                   field("name") = meta.name) _
            .find(Of biocad_registryModel.molecule)

        If Not q Is Nothing Then
            Return q
        End If

        Dim hashset As String() = {meta.name, meta.IUPACName} _
            .JoinIterates(meta.synonym) _
            .Where(Function(s) Not s.StringEmpty(, True)) _
            .Select(Function(s) s.ToLower.MD5) _
            .ToArray

        ' find molecule via the synonym
        q = registry.molecule _
            .left_join("synonym") _
            .on(field("`synonym`.obj_id") = field("`molecule`.id")) _
            .where(field("mass").between(mass - 1, mass + 1),
                   field("hashcode").in(hashset)) _
            .find(Of biocad_registryModel.molecule)("`molecule`.*")

        If Not q Is Nothing Then
            Return q
        End If

        Return Nothing
    End Function

End Module
