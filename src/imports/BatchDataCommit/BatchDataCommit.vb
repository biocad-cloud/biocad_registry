Imports System.Runtime.CompilerServices
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports SMRUCC.genomics.Assembly.NCBI.GenBank
Imports SMRUCC.genomics.Assembly.NCBI.GenBank.GBFF.Keywords.FEATURES
Imports SMRUCC.genomics.SequenceModel

''' <summary>
''' Create database, make dataset batch imports in transaction mode
''' </summary>
Public Module BatchDataCommit

    <Extension>
    Public Sub importsGenomics(registry As biocad_registry, genomes As IEnumerable(Of GBFF.File))
        Dim trans As CommitTransaction = registry.genomics.open_transaction(5).ignore

        For Each gb As GBFF.File In genomes
            Dim id As String = gb.Accession.AccessionId
            Dim seq As String = Strings.Trim(gb.Origin.ToFasta.SequenceData.TrimNewLine)

            Call trans.add(
                field("ncbi_taxid") = CInt(Val(gb.Taxon)),
                field("db_xref") = id,
                field("def") = gb.Definition.Value,
                field("nt") = seq,
                field("comment") = gb.Comment.Comment,
                field("biom_string") = gb.Source.BiomString,
                field("length") = seq.Length,
                field("checksum") = seq.ToUpper.MD5
            )
        Next

        Call trans.commit()
    End Sub

    <Extension>
    Public Sub importsDNASequence(registry As biocad_registry, genomes As IEnumerable(Of GBFF.File))
        Dim trans As CommitTransaction = registry.sequence_graph.open_transaction.ignore
        Dim vocabulary As BioCadVocabulary = registry.vocabulary_terms

        For Each gb As GBFF.File In genomes
            Dim data As New GenBankImports(registry, gb)
            Dim genes = gb.Features.ListFeatures("gene").ToArray

            Call VBDebugger.EchoLine("processing gene sequence batch imports of genome " & gb.Definition.Value)
            Call Parallel.For(0, genes.Length, Sub(i) data.addSingleSeuqnece(genes(i), trans, vocabulary))
        Next

        Call trans.commit()
    End Sub

    <Extension>
    Public Sub importsGenes(registry As biocad_registry, genomes As IEnumerable(Of GBFF.File))
        Dim trans As CommitTransaction = registry.molecule.open_transaction.ignore
        Dim vocabulary As BioCadVocabulary = registry.vocabulary_terms

        For Each gb As GBFF.File In genomes
            Dim data As New GenBankImports(registry, gb)
            Dim genes = gb.Features.ListFeatures("gene").ToArray

            Call VBDebugger.EchoLine("processing gene batch imports of genome " & gb.Definition.Value)
            Call Parallel.For(0, genes.Length, Sub(i) data.addSingleGene(genes(i), trans, vocabulary))
        Next

        Call trans.commit()
    End Sub

    <Extension>
    Private Sub addSingleSeuqnece(data As GenBankImports, gene As Feature, ByRef trans As CommitTransaction, vocabulary As BioCadVocabulary)
        Dim locus_tag As String = gene.Query(FeatureQualifiers.locus_tag)

        If locus_tag Is Nothing Then
            Return
        End If

        Dim rnaSeq As String = data.GetRNA(gene)
        Dim uniref As String = $"{data.ncbi_taxid}:{locus_tag}"
        Dim gene_id As biocad_registryModel.molecule = data.registry.molecule _
            .where(field("xref_id") = uniref,
                   field("type") = vocabulary.gene_term) _
            .find(Of biocad_registryModel.molecule)

        If gene_id Is Nothing Then
            Return
        End If

        Call trans.add(
            field("molecule_id") = gene_id.id,
            field("sequence") = rnaSeq,
            field("hashcode") = LCase(rnaSeq).MD5
        )
    End Sub

    <Extension>
    Private Sub addSingleGene(data As GenBankImports, gene As Feature, ByRef trans As CommitTransaction, vocabulary As BioCadVocabulary)
        Dim locus_tag As String = gene.Query(FeatureQualifiers.locus_tag)
        Dim gene_name = gene.Query(FeatureQualifiers.gene)

        If locus_tag Is Nothing Then
            Return
        End If

        Dim rnaSeq As String = data.GetRNA(gene)
        Dim massVal As Double = MolecularWeightCalculator.CalcMW_Nucleotides(rnaSeq, is_rna:=False)
        Dim formula As String = MolecularWeightCalculator.DeoxyribonucleotideFormula(rnaSeq).ToString
        Dim func As String = data.GetFunction(locus_tag)

        SyncLock trans
            Call trans.add(
               field("xref_id") = data.ncbi_taxid & ":" & locus_tag,
               field("name") = If(gene_name, locus_tag),
               field("mass") = massVal,
               field("type") = vocabulary.gene_term,
               field("formula") = formula,
               field("parent") = 0,
               field("tax_id") = data.ncbi_taxid,
               field("note") = func
           )
        End SyncLock
    End Sub

End Module
