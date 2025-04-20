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
        Dim vocabulary As BioCadVocabulary = registry.vocabulary_terms
        Dim ncbi_taxid = CUInt(Val(gb.Taxon))
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
            Dim gene_mol As molecule = find_gene(registry, ncbi_taxid, locus_tag)
            Dim gene_name = gene.Query(FeatureQualifiers.gene)

            If gene_mol Is Nothing Then
                ' create new in the database
                Call registry.molecule.add(
                    field("xref_id") = gene_dbxref,
                    field("name") = If(gene_name, locus_tag),
                    field("mass") = MolecularWeightCalculator.CalcMW_Nucleotides(mrna, is_rna:=False),
                    field("type") = vocabulary.gene_term,
                    field("formula") = MolecularWeightCalculator.DeoxyribonucleotideFormula(mrna).ToString,
                    field("parent") = 0,
                    field("tax_id") = ncbi_taxid,
                    field("note") = func
                )

                gene_mol = registry.molecule.find_object(field("xref_id") = gene_dbxref)

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

            If Not polypeptide Is Nothing Then
                ' add mRNA molecule
                Dim mRNA_mol As molecule

                ' add polypeptide molecule

            End If
        Next

        Call registry.AddGenomics(gb, gb.Origin.ToFasta)
    End Sub

    Public Function find_gene(registry As biocad_registry, ncbi_taxid As UInteger, locus_tag As String) As molecule
        Dim uniref As String = $"{ncbi_taxid}:{locus_tag}"
        Dim gene As molecule = registry.molecule.find_object(field("xref_id") = uniref)
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

    <Extension>
    Private Sub AddGenomics(registry As biocad_registry, gb As GBFF.File, genomics As FastaSeq)
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
