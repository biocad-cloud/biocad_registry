Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.Data.Framework
Imports Microsoft.VisualBasic.DataMining.KMeans
Imports Microsoft.VisualBasic.MachineLearning.Bootstrapping
Imports Microsoft.VisualBasic.Net.Http
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports SMRUCC.genomics.Model.MotifGraph.ProteinStructure
Imports SMRUCC.genomics.Model.MotifGraph.ProteinStructure.Kmer

Module fingerprintBuilder

    Sub RunBuilder()
        ' Dim morgan As New MorganFingerprint(8 ^ 5)
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
                Dim graph As node2vec.Graph = KMerGraph.FromSequence(seq.sequence, k:=3).GetVectorGraph
                Dim walks = graph.simulateWalks(5, 5).ToArray
                Dim fingerprint As Byte() ' = morgan.CalculateFingerprintCheckSum(graph, radius:=9)

                Call bar.SetLabel(seq.hashcode)
                Call trans.add(registry.sequence_graph _
                    .where(field("id") = seq.id) _
                    .save_sql(field("morgan") = fingerprint.GZipAsBase64))
            Next

            Call trans.commit()
        Next
    End Sub
End Module
