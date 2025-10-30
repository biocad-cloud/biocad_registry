Imports System.Runtime.CompilerServices
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Module ExportGeneFasta

    <Extension>
    Public Iterator Function ExportOperonGeneFasta(registry As biocad_registry) As IEnumerable(Of views.OperonGene)
        Dim page_size As Integer = 1000
        Dim offset As UInteger = 0
        Dim pagedata As views.OperonGene()

        Const sql As String = "SELECT 
    t1.*, sequence
FROM
    (SELECT 
        cluster_id, gene_id, name
    FROM
        cad_registry.cluster_link
    LEFT JOIN conserved_cluster ON cluster_link.cluster_id = conserved_cluster.id) t1
        LEFT JOIN
    sequence_graph ON sequence_graph.molecule_id = t1.gene_id
LIMIT {0} , {1}"

        For i As Integer = 1 To 100000
            offset = (i - 1) * page_size
            pagedata = registry.cluster_link.getDriver.Query(Of views.OperonGene)(String.Format(sql, offset, page_size))

            If pagedata.IsNullOrEmpty Then
                Exit For
            End If

            For Each seq As views.OperonGene In pagedata
                Yield seq
            Next
        Next
    End Function

End Module
