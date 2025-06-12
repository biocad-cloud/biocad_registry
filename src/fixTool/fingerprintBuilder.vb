Imports SMRUCC.genomics.Model.MotifGraph.ProteinStructure
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports SMRUCC.genomics.Model.MotifGraph.ProteinStructure.Kmer
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar
Imports Microsoft.VisualBasic.Net.Http
Imports Microsoft.VisualBasic.Data.Framework.IO
Imports Microsoft.VisualBasic.DataMining.KMeans
Imports Microsoft.VisualBasic.Data.Framework
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes

Module fingerprintBuilder

    Sub RunBuilder()
        Dim morgan As New MorganFingerprint(8 ^ 5)
        Dim page_size = 1000
        Dim page_data As biocad_storage.biocad_registryModel.sequence_graph()
        Dim terms = registry.vocabulary_terms
        Dim ec_id As UInteger = terms.ecnumber_term
        Dim trans As CommitTransaction
        Dim bar As Tqdm.ProgressBar = Nothing

        For i As Integer = 0 To Integer.MaxValue
            trans = registry.sequence_graph.open_transaction
            page_data = Program.registry.db_xrefs _
                .left_join("sequence_graph") _
                .on(field("sequence_graph.molecule_id") = field("db_xrefs.obj_id")) _
                .where(field("db_key") = ec_id) _
                .limit(i * page_size, page_size) _
                .select(Of biocad_storage.biocad_registryModel.sequence_graph)("sequence_graph.*")

            For Each seq In TqdmWrapper.Wrap(page_data, bar:=bar)
                Dim graph = KMerGraph.FromSequence(seq.sequence, k:=3)
                Dim fingerprint = morgan.CalculateFingerprintCheckSum(graph, radius:=9)

                Call bar.SetLabel(seq.hashcode)
                Call trans.add(registry.sequence_graph _
                    .where(field("id") = seq.id) _
                    .save_sql(field("morgan") = fingerprint.GZipAsBase64))
            Next

            Call trans.commit()
        Next
    End Sub

    Sub exportFingerprint()
        Dim page_size = 1000
        Dim page_data As EnzymeFingerprint()
        Dim terms = registry.vocabulary_terms
        Dim ec_id As UInteger = terms.ecnumber_term
        Dim fingerprint As New List(Of EntityClusterModel)

        For i As Integer = 0 To 50
            page_data = Program.registry.db_xrefs _
                .left_join("sequence_graph") _
                .on(field("sequence_graph.molecule_id") = field("db_xrefs.obj_id")) _
                .where(field("db_key") = ec_id) _
                .limit(i * page_size, page_size) _
                .select(Of EnzymeFingerprint)("xref as ec_number", "molecule_id", "morgan")

            For Each seq As EnzymeFingerprint In page_data
                If Len(seq.morgan) > 0 Then
                    fingerprint.Add(New EntityClusterModel With {
                        .ID = seq.molecule_id & " [" & seq.ec_number & "]",
                        .Cluster = seq.ec_number,
                        .Properties = seq.morgan _
                            .UnGzipBase64 _
                            .ToArray _
                            .Select(Function(b, o) (o, CDbl(b))) _
                            .ToDictionary(Function(o) "v" & (o.o + 1),
                                          Function(o)
                                              Return o.Item2
                                          End Function)
                    })
                End If
            Next
        Next

        Call fingerprint.SaveTo("./fingerprint.csv")
    End Sub

    Public Class EnzymeFingerprint

        <DatabaseField> Public Property ec_number As String
        <DatabaseField> Public Property molecule_id As UInteger
        <DatabaseField> Public Property morgan As String

    End Class
End Module
