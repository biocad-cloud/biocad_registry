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

    Public Sub importsGenes(registry As biocad_registry, genomes As IEnumerable(Of GBFF.File))
        Dim trans As CommitTransaction = registry.molecule.open_transaction.ignore
        Dim vocabulary = registry.vocabulary_terms

        For Each gb As GBFF.File In genomes
            Dim ncbi_taxid = CUInt(Val(gb.Taxon))
            Dim data As New GenBankImports(registry, gb)

            For Each gene As Feature In gb.Features.ListFeatures("gene")
                Dim locus_tag As String = gene.Query(FeatureQualifiers.locus_tag)
                Dim gene_name = gene.Query(FeatureQualifiers.gene)

                If locus_tag Is Nothing Then
                    Continue For
                End If

                Dim rnaSeq As String = data.GetRNA(gene)

                Call trans.add(
                   field("xref_id") = ncbi_taxid & ":" & locus_tag,
                   field("name") = If(gene_name, locus_tag),
                   field("mass") = MolecularWeightCalculator.CalcMW_Nucleotides(rnaSeq, is_rna:=False),
                   field("type") = vocabulary.gene_term,
                   field("formula") = MolecularWeightCalculator.DeoxyribonucleotideFormula(rnaSeq).ToString,
                   field("parent") = 0,
                   field("tax_id") = ncbi_taxid,
                   field("note") = data.GetFunction(locus_tag)
               )
            Next
        Next
    End Sub

End Module
