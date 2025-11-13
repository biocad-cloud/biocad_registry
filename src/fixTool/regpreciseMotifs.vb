Imports Microsoft.VisualBasic.Linq
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
            Dim protsIndex = prots.ToDictionary(Function(a) a.Headers(0), Function(a) a.SequenceData)
            Dim regulators = genome.regulome.AsEnumerable _
                .Where(Function(r) r.type = "TF") _
                .Select(Function(a)
                            Dim seq = protsIndex(a.regulator.name)
                            Dim family = a.family
                            Dim fa As New FastaSeq With {.Headers = {a.regulator.name, family}, .SequenceData = seq}

                            Return fa
                        End Function) _
                .ToArray

            Call TF.AddRange(regulators)
        Next

        Call New FastaFile(TF).Save("./TF.fasta")

        Pause()
    End Sub
End Module
