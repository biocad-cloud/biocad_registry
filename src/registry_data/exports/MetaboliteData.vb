Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Linq
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes

Namespace Exports

    Public Module MetaboliteData

        <Extension>
        Public Iterator Function exportSMILES(registry As biocad_registry, dbname As String) As IEnumerable(Of SMILESData)
            Dim page_size As Integer = 2000
            Dim offset As UInteger
            Dim page_data As SMILESData()

            For i As Integer = 1 To Integer.MaxValue
                offset = (i - 1) * page_size
                page_data = registry.metabolites _
                    .left_join("struct_data") _
                    .on(field("`struct_data`.metabolite_id") = field("`metabolites`.id")) _
                    .where(Not field(dbname).is_nothing, field(dbname) <> "") _
                    .limit(offset, page_size) _
                    .select(Of SMILESData)($"{dbname} AS xref_id",
                                           "smiles",
                                           "CONCAT('BioCAD', LPAD(metabolite_id, 11, '0')) AS id",
                                           "name",
                                           "formula",
                                           "exact_mass")
                If page_data.IsNullOrEmpty Then
                    Exit For
                Else
                    For Each data As SMILESData In page_data
                        Yield data
                    Next
                End If
            Next
        End Function

        <Extension>
        Public Function ExportIDMapping(registry As biocad_registry, db_field As String) As Dictionary(Of String, String())
            Dim id As String = "CONCAT('BioCAD', LPAD(id, 11, '0')) AS id"
            Dim db_xref As String = $"`{db_field}` AS db_xref"
            Dim mapping As ExportIDMapping() = registry.metabolites.where(field(db_field).char_length > 0).select(Of ExportIDMapping)(id, db_field)

            Return mapping.SafeQuery _
                .GroupBy(Function(a) a.db_xref) _
                .ToDictionary(Function(a) a.Key,
                              Function(a)
                                  Return a _
                                      .Select(Function(map) map.id) _
                                      .Distinct _
                                      .ToArray
                              End Function)
        End Function
    End Module

    Friend Class ExportIDMapping

        <DatabaseField> Public Property id As String
        <DatabaseField> Public Property db_xref As String

    End Class
End Namespace