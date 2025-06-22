Imports System.IO
Imports System.Runtime.CompilerServices
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1.Annotations
Imports BioNovoGene.BioDeep.Chemoinformatics
Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes

Public Module TagDataExport

    <Extension>
    Public Function ExportTagList(registry As biocad_registry, tagName As String) As String()
        Dim tag_id = registry.vocabulary.where(field("term") = tagName).find(Of biocad_registryModel.vocabulary)
        If tag_id Is Nothing Then
            Throw New InvalidDataException
        End If
        Return registry.molecule_tags _
            .where(field("tag_id") = tag_id.id) _
            .project(Of UInteger)("molecule_id") _
            .Select(Function(id) "BioCAD" & id.ToString.PadLeft(11, "0")) _
            .ToArray
    End Function

    <Extension>
    Public Function ExportTagData(registry As biocad_registry, tagName As String) As MetaboliteAnnotation()
        Dim tag_id = registry.vocabulary.where(field("term") = tagName).find(Of biocad_registryModel.vocabulary)

        If tag_id Is Nothing Then
            Throw New InvalidDataException
        End If

        Return registry.molecule_tags _
            .left_join("molecule") _
            .on(field("`molecule`.id") = field("`molecule_tags`.molecule_id")) _
            .where(field("tag_id") = tag_id.id) _
            .select(Of MetaboliteAnnotation)("CONCAT('BioCAD', LPAD(`molecule`.id, 11, '0')) AS id",
                                             "name",
                                             "formula",
                                             "mass AS exact_mass")
    End Function
End Module

Public Class MetaboliteAnnotation
    Implements GenericCompound
    Implements IReadOnlyId, ICompoundNameProvider, IExactMassProvider, IFormulaProvider

    <DatabaseField> Public Property id As String Implements IReadOnlyId.Identity
    <DatabaseField> Public Property name As String Implements ICompoundNameProvider.CommonName
    <DatabaseField> Public Property formula As String Implements IFormulaProvider.Formula
    <DatabaseField> Public Property exact_mass As Double Implements IExactMassProvider.ExactMass

End Class