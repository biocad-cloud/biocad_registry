Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports SMRUCC.genomics.Assembly.NCBI.GenBank
Imports SMRUCC.genomics.Assembly.NCBI.GenBank.GBFF.Keywords.FEATURES
Imports SMRUCC.genomics.ComponentModel.Loci

Public Module ImportsGenBank

    <Extension>
    Public Sub SaveGenBank(registry As biocad_registry, asm As GBFF.File)
        Dim taxid As UInteger = asm.Taxon
        Dim genes As CommitTransaction = registry.nucleotide_data.ignore.open_transaction
        Dim proteins As CommitTransaction = registry.protein_data.ignore.open_transaction
        Dim vocabulary As New biocad_vocabulary(registry)
        Dim ncbi_genbank As UInteger = vocabulary.db_genbank

        For Each gene As Feature In TqdmWrapper.Wrap(asm.EnumerateGeneFeatures(ORF:=True).ToArray)
            Dim loc As NucleotideLocation = gene.Location
            Dim locus_tag As String = gene.Query(FeatureQualifiers.locus_tag)
            Dim name As String = If(gene.Query(FeatureQualifiers.gene), locus_tag)
            Dim product As String = If(gene.Query(FeatureQualifiers.product), "")
            Dim nt As String = Strings.UCase(gene.SequenceData)
            Dim prot As String = Strings.UCase(gene.Query(FeatureQualifiers.translation))

            Call genes.ignore.add(
                field("source_id") = locus_tag,
                field("source_db") = ncbi_genbank,
                field("name") = name,
                field("function") = product,
                field("is_motif") = 0,
                field("left") = loc.left,
                field("strand") = If(loc.Strand = Strands.Forward, "+", "-"),
                field("operon_id") = 0,
                field("model_id") = 0,
                field("organism_source") = taxid,
                field("sequence") = nt,
                field("checksum") = nt.MD5
            )
            Call proteins.ignore.add(
                field("source_id") = locus_tag,
                field("source_db") = ncbi_genbank,
                field("name") = name,
                field("function") = product,
                field("gene_id") = 0,
                field("protein_id") = 0,
                field("sequence") = prot,
                field("checksum") = prot.MD5
            )
        Next

        Call genes.commit()
        Call proteins.commit()
    End Sub

End Module
