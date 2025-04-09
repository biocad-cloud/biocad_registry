Imports System.Runtime.CompilerServices
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports SMRUCC.genomics.Assembly.NCBI.GenBank
Imports SMRUCC.genomics.SequenceModel.FASTA

Public Module GenBankImports

    <Extension>
    Public Sub ImportsData(registry As biocad_registry, gb As GBFF.File)
        Dim vocabulary As New BioCadVocabulary(registry)

        Call registry.AddGenomics(gb, gb.Origin.ToFasta)
    End Sub

    <Extension>
    Private Sub AddGenomics(registry As biocad_registry, gb As GBFF.File, genomics As FastaSeq)
        Call registry.genomics.delayed.add(
            field("ncbi_taxid") = CInt(Val(gb.Taxon)),
            field("db_xref") = gb.Accession.AccessionId,
            field("def") = gb.Definition.Value,
            field("nt") = genomics.SequenceData,
            field("comment") = gb.Comment.Comment
        )
    End Sub

End Module
