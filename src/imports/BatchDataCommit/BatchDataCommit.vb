Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
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
    Public Sub importsDNASequence(registry As biocad_registry, genomes As IEnumerable(Of GenBankImports))
        Dim trans As CommitTransaction = registry.sequence_graph.open_transaction.ignore
        Dim vocabulary As BioCadVocabulary = registry.vocabulary_terms

        For Each data As GenBankImports In genomes
            Dim genes = data.getGenes.ToArray

            Call VBDebugger.EchoLine("processing gene sequence batch imports of genome " & data.desc)
            Call Parallel.For(0, genes.Length, Sub(i) data.addSingleDNASeuqnece(genes(i), trans, vocabulary))
        Next

        Call trans.commit()
    End Sub

    <Extension>
    Public Sub importsGenes(registry As biocad_registry, genomes As IEnumerable(Of GBFF.File))
        Dim trans As CommitTransaction = registry.molecule.open_transaction.ignore
        Dim vocabulary As BioCadVocabulary = registry.vocabulary_terms
        Dim pull As New List(Of GenBankImports)

        For Each gb As GBFF.File In genomes
            Dim data As New GenBankImports(registry, gb)
            Dim genes = gb.Features.ListFeatures("gene").ToArray

            Call pull.Add(data)
            Call VBDebugger.EchoLine("processing gene batch imports of genome " & gb.Definition.Value)
            Call Parallel.For(0, genes.Length, Sub(i) data.addSingleGene(genes(i), trans, vocabulary))
        Next

        Call trans.commit()
        Call registry.importsDNASequence(pull)
    End Sub

    <Extension>
    Public Sub importsProteins(registry As biocad_registry, genomes As IEnumerable(Of GBFF.File))
        Dim trans As CommitTransaction = registry.molecule.open_transaction.ignore
        Dim vocabulary As BioCadVocabulary = registry.vocabulary_terms
        Dim pull As New List(Of GenBankImports)

        For Each gb As GBFF.File In genomes
            Dim data As New GenBankImports(registry, gb)
            Dim genes = gb.Features.ListFeatures("gene").ToArray

            Call pull.Add(data)
            Call VBDebugger.EchoLine("processing protein data batch imports of genome " & gb.Definition.Value)
            Call Parallel.For(0, genes.Length, Sub(i) data.addSingleProtein(genes(i), trans, vocabulary))
        Next

        Call trans.commit()

        trans = registry.sequence_graph.open_transaction.ignore

        For Each gb As GenBankImports In pull
            Dim data As GenBankImports = gb
            Dim genes = gb.getGenes.ToArray

            Call VBDebugger.EchoLine("processing protein sequence data batch imports of genome " & gb.desc)
            Call Parallel.For(0, genes.Length, Sub(i) data.addSingleProteinSequence(genes(i), trans, vocabulary))
        Next

        Call trans.commit()
    End Sub

    <Extension>
    Public Sub importsFeatureXrefs(registry As biocad_registry, genomes As IEnumerable(Of GBFF.File))
        Dim trans As CommitTransaction = registry.db_xrefs.open_transaction.ignore
        Dim vocabulary As BioCadVocabulary = registry.vocabulary_terms

        For Each gb As GBFF.File In genomes
            Dim data As New GenBankImports(registry, gb)
            Dim genes = data.getGenes.ToArray

            Call VBDebugger.EchoLine("processing gene feature corss reference id data batch imports of genome " & data.desc)
            Call Parallel.For(0, genes.Length, Sub(i) data.addGeneDbXrefs(genes(i), trans, vocabulary))
        Next

        Call trans.commit()
    End Sub

    ''' <summary>
    ''' add db_xrefs for gene/proteins
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="gene"></param>
    ''' <param name="trans"></param>
    ''' <param name="vocabulary"></param>
    <Extension>
    Private Sub addGeneDbXrefs(data As GenBankImports, gene As Feature, ByRef trans As CommitTransaction, vocabulary As BioCadVocabulary)
        Dim locus_tag As String = gene.Query(FeatureQualifiers.locus_tag)

        If locus_tag Is Nothing Then
            Return
        End If

        Dim gene_id As biocad_registryModel.molecule = data.registry.molecule _
            .where(field("xref_id") = $"{data.ncbi_taxid}:{locus_tag}",
                   field("type") = vocabulary.gene_term) _
            .find(Of biocad_registryModel.molecule)

        If gene_id Is Nothing Then
            Return
        Else
            Call trans.add(
                field("obj_id") = gene_id.id,
                field("db_key") = vocabulary.genbank_term,
                field("xref") = locus_tag,
                field("type") = vocabulary.gene_term
            )

            Dim xrefs = gene.QueryDuplicated("db_xref")

            For Each db_xref As NamedValue(Of String) In xrefs.Select(Function(tag) tag.GetTagValue(":"))
                Call trans.add(
                    field("obj_id") = gene_id.id,
                    field("db_key") = vocabulary.GetDatabaseKey(db_xref.Name),
                    field("xref") = db_xref.Value,
                    field("type") = vocabulary.gene_term
                )
            Next
        End If

        If data.CheckProtein(locus_tag) Then
            Dim cds As Feature = data.GetCDS(locus_tag)
            Dim cds_id = cds.Query(FeatureQualifiers.protein_id)
            Dim protein_mol As biocad_registryModel.molecule = data.registry.molecule _
                .where(field("xref_id") = $"{data.ncbi_taxid}:{cds_id}",
                       field("type") = vocabulary.protein_term) _
                .find(Of biocad_registryModel.molecule)

            If protein_mol Is Nothing Then
                Return
            Else
                ' add db_xrefs
                Call trans.add(
                    field("obj_id") = protein_mol.id,
                    field("db_key") = vocabulary.genbank_term,
                    field("xref") = cds_id,
                    field("type") = vocabulary.protein_term
                )
                Call trans.add(
                    field("obj_id") = protein_mol.id,
                    field("db_key") = vocabulary.genbank_term,
                    field("xref") = locus_tag,
                    field("type") = vocabulary.protein_term
                )

                Dim xrefs = cds.QueryDuplicated("db_xref")

                For Each db_xref As NamedValue(Of String) In xrefs.Select(Function(tag) tag.GetTagValue(":"))
                    Call trans.add(
                        field("obj_id") = protein_mol.id,
                        field("db_key") = vocabulary.GetDatabaseKey(db_xref.Name),
                        field("xref") = db_xref.Value,
                        field("type") = vocabulary.protein_term
                    )
                Next
            End If
        End If
    End Sub

    <Extension>
    Private Sub addSingleDNASeuqnece(data As GenBankImports, gene As Feature, ByRef trans As CommitTransaction, vocabulary As BioCadVocabulary)
        Dim locus_tag As String = gene.Query(FeatureQualifiers.locus_tag)

        If locus_tag Is Nothing Then
            Return
        End If

        Dim rnaSeq As String = Strings.Trim(data.GetRNA(gene)).ToUpper
        Dim uniref As String = $"{data.ncbi_taxid}:{locus_tag}"
        Dim gene_id As biocad_registryModel.molecule = data.registry.molecule _
            .where(field("xref_id") = uniref,
                   field("type") = vocabulary.gene_term) _
            .find(Of biocad_registryModel.molecule)

        If gene_id Is Nothing OrElse rnaSeq = "" Then
            Return
        End If

        Call trans.add(
            field("molecule_id") = gene_id.id,
            field("sequence") = rnaSeq,
            field("hashcode") = LCase(rnaSeq).MD5
        )
    End Sub

    <Extension>
    Private Sub addSingleProteinSequence(data As GenBankImports, gene As Feature, ByRef trans As CommitTransaction, vocabulary As BioCadVocabulary)
        Dim locus_tag As String = gene.Query(FeatureQualifiers.locus_tag)

        If locus_tag Is Nothing Then
            Return
        ElseIf Not data.CheckProtein(locus_tag) Then
            Return
        End If

        Dim cds As Feature = data.GetCDS(locus_tag)
        Dim cds_id = cds.Query(FeatureQualifiers.protein_id)
        Dim protein_mol As biocad_registryModel.molecule = data.registry.molecule _
            .where(field("xref_id") = $"{data.ncbi_taxid}:{cds_id}",
                   field("type") = vocabulary.protein_term) _
            .find(Of biocad_registryModel.molecule)

        If protein_mol Is Nothing Then
            Return
        End If

        Dim polyAASeq As String = Strings.Trim(cds.Query(FeatureQualifiers.translation)).ToUpper

        Call trans.add(
            field("molecule_id") = protein_mol.id,
            field("sequence") = polyAASeq,
            field("hashcode") = LCase(polyAASeq).MD5
        )
    End Sub

    <Extension>
    Private Sub addSingleProtein(data As GenBankImports, gene As Feature, ByRef trans As CommitTransaction, vocabulary As BioCadVocabulary)
        Dim locus_tag As String = gene.Query(FeatureQualifiers.locus_tag)
        Dim gene_name = gene.Query(FeatureQualifiers.gene)

        If locus_tag Is Nothing Then
            Return
        ElseIf Not data.CheckProtein(locus_tag) Then
            Return
        End If

        Dim unifyKey As String = $"{data.ncbi_taxid}:{locus_tag}"
        Dim gene_mol As biocad_registryModel.molecule = data.registry.molecule _
            .where(field("xref_id") = unifyKey, field("tax_id") = data.ncbi_taxid) _
            .find(Of biocad_registryModel.molecule)
        Dim gene_id As UInteger = If(gene_mol Is Nothing, 0, gene_mol.id)

        ' get mRNA sequence
        Dim cds As Feature = data.GetCDS(locus_tag)
        Dim polyAASeq As String = Strings.Trim(cds.Query(FeatureQualifiers.translation))
        Dim massVal As Double = If(polyAASeq = "", 0, MolecularWeightCalculator.CalcMW_Polypeptide(polyAASeq))
        Dim formula As String = If(polyAASeq = "", "", MolecularWeightCalculator.PolypeptideFormula(polyAASeq).ToString)
        Dim func As String = data.GetFunction(locus_tag)
        Dim cds_id = cds.Query(FeatureQualifiers.protein_id)

        SyncLock trans
            Call trans.add(
                field("xref_id") = data.ncbi_taxid & ":" & cds_id,
                field("name") = If(gene_name, cds_id),
                field("mass") = massVal,
                field("type") = vocabulary.protein_term,
                field("formula") = formula,
                field("parent") = gene_id,
                field("tax_id") = data.ncbi_taxid,
                field("note") = func
           )
        End SyncLock
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
