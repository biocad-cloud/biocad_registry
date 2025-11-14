Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Text.Xml.Models
Imports SMRUCC.genomics.Assembly.NCBI.GenBank
Imports SMRUCC.genomics.Data.Regprecise
Imports SMRUCC.genomics.SequenceModel.FASTA

Public Module regpreciseMotifs

    Sub extractTF()
        Dim db = "F:\ecoli\regprecise.xml".LoadXml(Of TranscriptionFactors)
        Dim TF As New List(Of FastaSeq)

        For Each genome As BacteriaRegulome In db.AsEnumerable
            If genome.genome.taxonomyId = 0 Then
                Continue For
            End If

            Dim id As String = genome.genome.genomeId.Split().First
            Dim genbank = $"D:\datapool\regprecise_genbank\genomes\{id}_genomic.gbff"
            Dim asm = GBFF.File.LoadDatabase(genbank).ToArray
            Dim prots = asm.Select(Function(gb) gb.ExportProteins_Short).IteratesALL.ToArray

            If prots.IsNullOrEmpty Then
                Continue For
            End If

            Dim protsIndex = prots.ToDictionary(Function(a) a.Headers(0).Split.First, Function(a) a.SequenceData)
            Dim regulators = genome.regulome.AsEnumerable _
                .Where(Function(r) r.type = Types.TF) _
                .Select(Iterator Function(a) As IEnumerable(Of FastaSeq)
                            If a.locus_tags Is Nothing Then
                                Return
                            End If
                            For Each prot As NamedValue In a.locus_tags
                                If prot Is Nothing Then
                                    Continue For
                                End If
                                If Not protsIndex.ContainsKey(prot.name) Then
                                    Continue For
                                End If

                                Dim seq = protsIndex(prot.name)
                                Dim family = a.family
                                Dim fa As New FastaSeq With {
                                    .Headers = {prot.name, family},
                                    .SequenceData = seq
                                }

                                Yield fa
                            Next
                        End Function) _
                .IteratesALL _
                .ToArray

            Call TF.AddRange(regulators)
        Next

        Call New FastaFile(TF).Save("./TF.fasta")

        Pause()
    End Sub

    Sub extractMotifs()
        Dim db = "F:\ecoli\regprecise.xml".LoadXml(Of TranscriptionFactors)
        Dim tfs = db.AsEnumerable.Select(Function(g) g.regulome.AsEnumerable).IteratesALL.GroupBy(Function(r) r.family).Select(Function(a)
                                                                                                                                   Dim familySites = a.Select(Function(tf) tf.ExportMotifs).IteratesALL.ToArray
                                                                                                                                   Return (a.Key, New FastaFile(familySites))
                                                                                                                               End Function).ToArray

        For Each group In tfs
            Call group.Item2.Save($"./{group.Key}.fasta")
        Next
    End Sub
End Module
