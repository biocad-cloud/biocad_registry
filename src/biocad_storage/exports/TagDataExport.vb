Imports System.IO
Imports System.Runtime.CompilerServices
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

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
End Module
