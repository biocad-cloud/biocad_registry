Imports System.Runtime.CompilerServices
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

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
    End Module
End Namespace