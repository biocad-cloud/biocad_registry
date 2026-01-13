Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Linq
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data.biocad_registryModel
Imports SMRUCC.genomics.Assembly.NCBI.GenBank
Imports SMRUCC.genomics.Assembly.NCBI.GenBank.GBFF.Keywords.FEATURES
Imports SMRUCC.genomics.Assembly.NCBI.Taxonomy
Imports SMRUCC.genomics.ComponentModel.Loci

Public Module ImportsGenBank

    ''' <summary>
    ''' just create the data entry
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="asm"></param>
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

    <Extension>
    Public Sub SaveDbXrefs(registry As biocad_registry, asm As GBFF.File)
        Dim vocabulary As New biocad_vocabulary(registry)
        Dim prot_type As UInteger = vocabulary.protein_data
        Dim nucl_type As UInteger = vocabulary.nucleotide_data
        Dim ncbi_genbank As UInteger = vocabulary.db_genbank
        Dim ec_number As UInteger = vocabulary.db_ECNumber
        Dim xrefs As CommitTransaction = registry.db_xrefs.ignore.open_transaction

        For Each gene As Feature In TqdmWrapper.Wrap(asm.EnumerateGeneFeatures(ORF:=True).ToArray)
            Dim locus_tag As String = gene.Query(FeatureQualifiers.locus_tag)

            If locus_tag.StringEmpty() Then
                Continue For
            End If

            Dim prot = registry.protein_data.where(field("source_id") = locus_tag, field("source_db") = ncbi_genbank).find(Of protein_data)
            Dim nucl = registry.nucleotide_data.where(field("source_id") = locus_tag, field("source_db") = ncbi_genbank).find(Of nucleotide_data)

            If prot Is Nothing OrElse nucl Is Nothing Then
                Continue For
            End If

            registry.protein_data.where(field("id") = prot.id).save(field("gene_id") = nucl.id)

            For Each id As String In gene.QueryDuplicated("EC_number").SafeQuery
                Call xrefs.ignore.add(field("obj_id") = prot.id, field("type") = prot_type, field("db_name") = ec_number, field("db_xref") = id, field("db_source") = ncbi_genbank)
            Next
        Next

        Call xrefs.commit()
    End Sub

    <Extension>
    Public Sub ImportsNCBITaxonomyTree(registry As biocad_registry, ncbi_tax As NcbiTaxonomyTree)
        Dim taxdata As CommitTransaction = registry.ncbi_taxonomy.ignore.open_transaction
        Dim rank_key As New Dictionary(Of String, vocabulary)

        For Each node As TaxonomyNode In TqdmWrapper.Wrap(ncbi_tax.Taxonomy.Values)
            Dim rank As vocabulary = rank_key.ComputeIfAbsent(
                key:=node.rank,
                lazyValue:=Function(level)
                               Return registry.biocad_vocabulary.GetVocabulary(biocad_vocabulary.NCBITaxonomyRank, If(level, "unknown"))
                           End Function)

            Call taxdata.ignore.add(
                field("id") = node.taxid,
                field("name") = node.name,
                field("rank") = rank.id,
                field("ancestor") = node.parent,
                field("num_childs") = node.children.TryCount,
                field("note") = node.ToString
            )
        Next

        Call taxdata.commit()
    End Sub

End Module
