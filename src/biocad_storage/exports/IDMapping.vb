Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Linq
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Module IDMapping

    <Extension>
    Public Function ExportIdMapping(registry As biocad_registry, db_name As String) As Dictionary(Of String, String)
        Dim mapping As New Dictionary(Of String, String)
        Dim dbkey As UInteger = registry.vocabulary_terms.GetDatabaseKey(db_name)
        Dim page_size As UInteger = 1000

        For page As Integer = 1 To Integer.MaxValue
            Dim xrefs = registry.db_xrefs _
                .where(field("db_key") = dbkey, field("type") = registry.vocabulary_terms.metabolite_term) _
                .limit((page - 1) * page_size, page_size) _
                .select(Of biocad_registryModel.db_xrefs)

            If xrefs.IsNullOrEmpty Then
                Exit For
            End If

            For Each xref As biocad_registryModel.db_xrefs In xrefs
                If Not mapping.ContainsKey(xref.xref) Then
                    Call mapping.Add(xref.xref, "BioCAD" & xref.obj_id.ToString.PadLeft(11, "0"))
                End If
            Next
        Next

        Return mapping
    End Function
End Module
