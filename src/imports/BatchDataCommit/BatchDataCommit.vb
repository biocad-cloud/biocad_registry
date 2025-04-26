Imports System.Runtime.CompilerServices
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports SMRUCC.genomics.Assembly.NCBI.GenBank

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

End Module
