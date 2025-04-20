Imports System.Runtime.CompilerServices
Imports biocad_registry.biocad_registryModel
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports SMRUCC.genomics.Assembly.NCBI.GenBank
Imports SMRUCC.genomics.Assembly.NCBI.GenBank.GBFF.Keywords.FEATURES
Imports SMRUCC.genomics.SequenceModel
Imports SMRUCC.genomics.SequenceModel.FASTA

Public Class GenBankImports

    ReadOnly registry As biocad_registry
    ReadOnly gb As GBFF.File
    ReadOnly vocabulary As BioCadVocabulary
    ReadOnly ncbi_taxid As UInteger
    ReadOnly cds As Dictionary(Of String, Feature)
    ReadOnly rRNA As Dictionary(Of String, Feature)
    ReadOnly tRNA As Dictionary(Of String, Feature)

    Sub New(registry As biocad_registry, gb As GBFF.File)
        Me.registry = registry
        Me.gb = gb

        vocabulary = registry.vocabulary_terms
        ncbi_taxid = CUInt(Val(gb.Taxon))
        cds = gb.LoadFeatureIndex("CDS")
        tRNA = gb.LoadFeatureIndex("tRNA")
        rRNA = gb.LoadFeatureIndex("rRNA")
    End Sub

    Public Sub ImportsData()
        Call registry.AddGenomics(gb, gb.Origin.ToFasta)

        For Each gene As Feature In TqdmWrapper.Wrap(gb.Features.ListFeatures("gene").ToArray)
            Call ImportsGeneFeature(gene)
        Next
    End Sub

    Private Sub ImportsGeneFeature(gene As Feature)
        Dim locus_tag As String = gene.Query(FeatureQualifiers.locus_tag)
        Dim cds_feature = cds.TryGetValue(locus_tag)
        Dim rnaSeq = gb.GetmRNASequence(mRNA:=cds_feature)
        Dim gene_dbxref = $"{ncbi_taxid}:{locus_tag}"
        Dim func As String = Nothing

        If cds_feature Is Nothing Then
            ' is tRNA/rRNA
            If tRNA.ContainsKey(locus_tag) Then
                func = tRNA(locus_tag).Query(FeatureQualifiers.product)
            ElseIf rRNA.ContainsKey(locus_tag) Then
                func = rRNA(locus_tag).Query(FeatureQualifiers.product)
            Else
                func = locus_tag
            End If
        Else
            func = cds_feature.Query(FeatureQualifiers.product)
        End If

        ' add gene molecule
        Dim gene_mol As molecule = find_gene(locus_tag)
        Dim gene_name = gene.Query(FeatureQualifiers.gene)

        If gene_mol Is Nothing Then
            ' create new in the database
            Call registry.molecule.add(
                field("xref_id") = ncbi_taxid & ":" & locus_tag,
                field("name") = If(gene_name, locus_tag),
                field("mass") = MolecularWeightCalculator.CalcMW_Nucleotides(rnaSeq, is_rna:=False),
                field("type") = vocabulary.gene_term,
                field("formula") = MolecularWeightCalculator.DeoxyribonucleotideFormula(rnaSeq).ToString,
                field("parent") = 0,
                field("tax_id") = ncbi_taxid,
                field("note") = func
            )

            gene_mol = registry.molecule.find_object(field("xref_id") = ncbi_taxid & ":" & locus_tag)

            ' add db_xrefs
            registry.db_xrefs.delayed.add(
                field("obj_id") = gene_mol.id,
                field("db_key") = vocabulary.genbank_term,
                field("xref") = locus_tag,
                field("type") = vocabulary.gene_term
            )

            If Not gene_name.StringEmpty(, True) Then
                ' add synonym name
                registry.synonym.delayed.add(
                    field("obj_id") = gene_mol.id,
                    field("type_id") = vocabulary.gene_term,
                    field("hashcode") = LCase(gene_name).MD5,
                    field("synonym") = gene_name,
                    field("lang") = "en"
                )
            End If
        End If

        ' add gene sequence
        If registry.sequence_graph.find_object(field("molecule_id") = gene_mol.id) Is Nothing Then
            registry.sequence_graph.delayed.add(
                field("molecule_id") = gene_mol.id,
                field("sequence") = rnaSeq,
                field("hashcode") = LCase(rnaSeq).MD5
            )
        End If

        If Not cds_feature Is Nothing Then
            ' add polypeptide molecule
            Call AddProtein(cds_feature)
        Else
            ' is rRNA or tRNA
            If tRNA.ContainsKey(locus_tag) Then

            ElseIf rRNA.ContainsKey(locus_tag) Then

            Else
                ' ? unknown
                ' skip do nothing
            End If
        End If
    End Sub

    Private Sub AddProtein(cds As Feature)
        Dim polypeptide As String = cds.Query(FeatureQualifiers.translation)
        Dim cds_id = cds.Query(FeatureQualifiers.protein_id)
        Dim func = cds.Query(FeatureQualifiers.product)
    End Sub

    Public Function find_gene(locus_tag As String) As molecule
        Dim uniref As String = $"{ncbi_taxid}:{locus_tag}"
        Dim gene As molecule = registry.molecule.find_object(
            field("xref_id") = uniref,
            field("tax_id") = ncbi_taxid
        )
        Dim vocabulary As BioCadVocabulary = registry.vocabulary_terms

        If gene Is Nothing Then
            ' find by db_xref
            gene = registry.db_xrefs _
                .left_join("molecule") _
                .on(field("`molecule`.id") = "obj_id") _
                .where(field("`molecule`.type") = vocabulary.gene_term,
                       field("`db_xrefs`.type") = vocabulary.gene_term,
                       field("db_key") = vocabulary.genbank_term,
                       field("xref") = locus_tag) _
                .find(Of biocad_registryModel.molecule)("`molecule`.*")
        End If

        Return gene
    End Function
End Class

Public Module GenBankTools

    <Extension>
    Public Function LoadFeatureIndex(gb As GBFF.File, key As String) As Dictionary(Of String, Feature)
        Return gb.Features _
            .ListFeatures(key) _
            .ToDictionary(Function(f) f.Query(FeatureQualifiers.locus_tag),
                          Function(f)
                              Return f
                          End Function)
    End Function

    <Extension>
    Public Sub AddGenomics(registry As biocad_registry, gb As GBFF.File, genomics As FastaSeq)
        Dim id As String = gb.Accession.AccessionId

        If registry.genomics.find_object(field("db_xref") = id) Is Nothing Then
            Call registry.genomics.delayed.add(
                field("ncbi_taxid") = CInt(Val(gb.Taxon)),
                field("db_xref") = id,
                field("def") = gb.Definition.Value,
                field("nt") = genomics.SequenceData,
                field("comment") = gb.Comment.Comment,
                field("biom_string") = gb.Source.BiomString
            )
        End If
    End Sub
End Module