Imports System.Runtime.CompilerServices
Imports biocad_registry.biocad_registryModel
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports SMRUCC.genomics.Assembly.NCBI.GenBank
Imports SMRUCC.genomics.Assembly.NCBI.GenBank.GBFF.Keywords.FEATURES
Imports SMRUCC.genomics.SequenceModel
Imports SMRUCC.genomics.SequenceModel.FASTA

Public Module GenBankImports

    <Extension>
    Public Sub ImportsData(registry As biocad_registry, gb As GBFF.File)
        Dim vocabulary As New BioCadVocabulary(registry)
        Dim ncbi_taxid = CInt(Val(gb.Taxon))
        Dim cds = gb.Features.AsEnumerable _
            .AsParallel _
            .Where(Function(f) f.KeyName = "CDS") _
            .ToDictionary(Function(f) f.Query(FeatureQualifiers.locus_tag),
                          Function(f)
                              Return f
                          End Function)

        For Each gene As Feature In gb.Features.AsEnumerable.Where(Function(f) f.KeyName = "gene")
            Dim locus_tag As String = gene.Query(FeatureQualifiers.locus_tag)
            Dim cds_feature = cds.TryGetValue(locus_tag)
            Dim polypeptide As String = cds_feature.Query(FeatureQualifiers.translation)
            Dim cds_id = cds_feature.Query(FeatureQualifiers.protein_id)
            Dim func = cds_feature.Query(FeatureQualifiers.product)
            Dim mrna = gb.GetmRNASequence(mRNA:=cds_feature)
            Dim gene_dbxref = $"{ncbi_taxid}:{locus_tag}"

            ' add gene molecule
            Dim gene_mol As molecule

            If gene_mol Is Nothing Then
                ' create new in the database
                Call registry.molecule.add(
                    field("xref_id") = gene_dbxref,
                    field("name") = locus_tag,
                    field("mass") = MolecularWeightCalculator.CalcMW_Nucleotides(mrna, is_rna:=False),
                    field("type") = vocabulary.gene_term,
                    field("formula") = MolecularWeightCalculator.DeoxyribonucleotideFormula(mrna).ToString,
                    field("parent") = 0,
                    field("tax_id") = ncbi_taxid,
                    field("note") = func
                )
            End If

            If Not polypeptide Is Nothing Then
                ' add mRNA molecule
                Dim mRNA_mol As molecule

                ' add polypeptide molecule

            End If
        Next

        Call registry.AddGenomics(gb, gb.Origin.ToFasta)
    End Sub

    <Extension>
    Private Sub AddGenomics(registry As biocad_registry, gb As GBFF.File, genomics As FastaSeq)
        Call registry.genomics.delayed.add(
            field("ncbi_taxid") = CInt(Val(gb.Taxon)),
            field("db_xref") = gb.Accession.AccessionId,
            field("def") = gb.Definition.Value,
            field("nt") = genomics.SequenceData,
            field("comment") = gb.Comment.Comment,
            field("biom_string") = gb.Source.BiomString
        )
    End Sub

End Module
