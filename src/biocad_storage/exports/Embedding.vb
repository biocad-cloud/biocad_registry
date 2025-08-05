Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.DataMining.KMeans
Imports Microsoft.VisualBasic.Net.Http
Imports Microsoft.VisualBasic.Serialization.BinaryDumping
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports SMRUCC.genomics.Model.MotifGraph.ProteinStructure
Imports SMRUCC.genomics.Model.MotifGraph.ProteinStructure.Kmer

Public Module Embedding

    Public Sub RunEnzymeFingerprintBuilder(registry As biocad_registry)
        Dim morgan As New MorganFingerprint(8 ^ 5)
        Dim page_size = 1000
        Dim page_data As biocad_storage.biocad_registryModel.sequence_graph()
        Dim terms = registry.vocabulary_terms
        Dim ec_id As UInteger = terms.ecnumber_term
        Dim trans As CommitTransaction
        Dim bar As Tqdm.ProgressBar = Nothing

        For i As Integer = 0 To Integer.MaxValue
            trans = registry.sequence_graph.open_transaction
            page_data = registry.db_xrefs _
                .left_join("sequence_graph") _
                .on(field("sequence_graph.molecule_id") = field("db_xrefs.obj_id")) _
                .where(field("db_key") = ec_id) _
                .limit(i * page_size, page_size) _
                .select(Of biocad_storage.biocad_registryModel.sequence_graph)("sequence_graph.*")

            If page_data.IsNullOrEmpty Then
                Exit For
            End If

            For Each seq In TqdmWrapper.Wrap(page_data, bar:=bar)
                Dim graph = KMerGraph.FromSequence(seq.sequence, k:=3)
                Dim fingerprint As Byte() = morgan.CalculateFingerprintCheckSum(graph, radius:=3)

                Call bar.SetLabel(seq.hashcode)
                Call trans.add(registry.sequence_graph _
                    .where(field("id") = seq.id) _
                    .save_sql(field("morgan") = fingerprint.GZipAsBase64))
            Next

            Call trans.commit()
        Next
    End Sub

    Public Iterator Function ExportEnzymeFingerprint(registry As biocad_registry) As IEnumerable
        Dim page_size = 1000
        Dim page_data As EnzymeFingerprint()
        Dim terms = registry.vocabulary_terms
        Dim ec_id As UInteger = terms.ecnumber_term
        Dim decoder As New NetworkByteOrderBuffer

        For i As Integer = 0 To Integer.MaxValue
            page_data = registry.db_xrefs _
                .left_join("sequence_graph") _
                .on(field("sequence_graph.molecule_id") = field("db_xrefs.obj_id")) _
                .where(field("db_key") = ec_id) _
                .limit(i * page_size, page_size) _
                .select(Of EnzymeFingerprint)("xref as ec_number", "molecule_id", "morgan")

            If page_data.IsNullOrEmpty Then
                Exit For
            End If

            For Each seq As EnzymeFingerprint In page_data
                If Len(seq.morgan) > 0 Then
                    Dim checksum = seq.morgan.UnGzipBase64.ToArray
                    Dim v = decoder.decode(checksum).Select(Function(b, o) (o, b)).ToArray

                    Yield New EntityClusterModel With {
                        .ID = seq.molecule_id & " [" & seq.ec_number & "]",
                        .Cluster = seq.ec_number,
                        .Properties = v _
                            .ToDictionary(Function(o) "v" & (o.o + 1),
                                          Function(o)
                                              Return o.Item2
                                          End Function)
                    }
                End If
            Next
        Next
    End Function

    Public Class EnzymeFingerprint

        <DatabaseField> Public Property ec_number As String
        <DatabaseField> Public Property molecule_id As UInteger
        <DatabaseField> Public Property morgan As String

    End Class

    ''' <summary>
    ''' export the genomics sequence fingerprint matrix
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <returns>
    ''' only exports of the complete genomics sequence data
    ''' </returns>
    <Extension>
    Public Iterator Function ExportGenomicsFingerprint(registry As biocad_registry) As IEnumerable(Of NamedCollection(Of Double))
        Dim page_size As Integer = 100
        Dim offset As UInteger
        Dim page_data As biocad_registryModel.genomics()

        For page As Integer = 1 To Integer.MaxValue
            offset = (page - 1) * page_size
            page_data = registry.genomics _
                .where(match("def").against("+complete -plasmid", booleanMode:=True), Not field("fingerprint").is_nothing) _
                .limit(offset, page_size) _
                .select(Of biocad_registryModel.genomics)

            If page_data.IsNullOrEmpty Then
                Exit For
            End If

            For Each genome As biocad_registryModel.genomics In From nt As biocad_registryModel.genomics
                                                                In page_data
                                                                Where nt.fingerprint.Length > 0

                Dim fingerprint As Byte() = genome.fingerprint.UnGzipBase64.ToArray
                Dim v As Double() = fingerprint.Select(Function(b) CDbl(b)).ToArray

                Yield New NamedCollection(Of Double)(genome.id.ToString, v, genome.biom_string)
            Next
        Next
    End Function

    Public Sub UpdateGenomicsFingerprint(registry As biocad_registry)
        Dim page_size = 10
        Dim morgan As New MorganFingerprint(8 ^ 5)

        For i As Integer = 0 To Integer.MaxValue
            Dim page = registry.genomics _
                .where(match("def").against("+complete -plasmid", booleanMode:=True),
                       field("fingerprint").is_nothing) _
                .limit(i * page_size, page_size) _
                .select(Of biocad_registryModel.genomics)

            If page.IsNullOrEmpty Then
                Exit For
            End If

            For Each seq As biocad_registryModel.genomics In page
                Dim graph = KMerGraph.FromSequence(seq.nt, k:=3)
                Dim fingerprint = morgan.CalculateFingerprintCheckSum(graph, radius:=3)

                Call VBDebugger.EchoLine(seq.db_xref & vbTab & seq.def)
                Call registry.genomics.where(field("id") = seq.id).save(field("fingerprint") = fingerprint.GZipAsBase64)
            Next
        Next
    End Sub

    Sub BuildProteinNucleotideAll(registry As biocad_registry)
        Dim morgan As New MorganFingerprint(8 ^ 5)
        Dim page_size = 10000
        Dim page_data As biocad_storage.biocad_registryModel.sequence_graph()
        Dim trans As CommitTransaction
        Dim bar As Tqdm.ProgressBar = Nothing
        Dim terms As BioCadVocabulary = registry.vocabulary_terms

        For i As Integer = 0 To Integer.MaxValue
            trans = registry.sequence_graph.open_transaction
            page_data = registry.sequence_graph _
                .left_join("molecule").on(field("molecule.id") = field("sequence_graph.molecule_id")) _
                .where(field("type") <> terms.metabolite_term) _
                .limit(i * page_size, page_size) _
                .select(Of biocad_storage.biocad_registryModel.sequence_graph)("sequence_graph.*")

            If page_data.IsNullOrEmpty Then
                Exit For
            End If

            For Each seq In TqdmWrapper.Wrap(page_data, bar:=bar)
                Dim graph = KMerGraph.FromSequence(seq.sequence, k:=3)
                Dim fingerprint = morgan.CalculateFingerprintCheckSum(graph, radius:=3)

                Call bar.SetLabel(seq.hashcode)
                Call trans.add(registry.sequence_graph _
                    .where(field("id") = seq.id) _
                    .save_sql(field("morgan") = fingerprint.GZipAsBase64))
            Next

            Call trans.commit()
        Next
    End Sub
End Module
