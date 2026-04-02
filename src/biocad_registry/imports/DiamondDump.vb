Imports System.Runtime.CompilerServices
Imports registry_data.biocad_registryModel
Imports SMRUCC.genomics.Interops.NCBI.Extensions

Module DiamondDump

    <Extension>
    Public Iterator Function CreateDiamondDumpData(block As DiamondAnnotation()) As IEnumerable(Of protein_cluster)
        For Each d As DiamondAnnotation In block
            Dim qid As UInteger = d.QseqId.Split("|"c).Last
            Dim sid As UInteger = d.SseqId.Split("|"c).Last

            Yield New protein_cluster With {
                .bit_score = d.BitScore,
                .e_value = d.EValue,
                .gap_open = d.GapOpen,
                .identities = d.Pident,
                .mis_match = d.Mismatch,
                .q_end = d.QEnd,
                .q_start = d.QStart,
                .s_end = d.SEnd,
                .s_start = d.SStart,
                .hit_id = sid,
                .query_id = qid,
                .add_time = Now
            }
        Next
    End Function
End Module
