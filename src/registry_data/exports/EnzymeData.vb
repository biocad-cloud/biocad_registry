Imports System.Runtime.CompilerServices
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports SMRUCC.genomics.SequenceModel.FASTA
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Module EnzymeData

    <Extension>
    Public Iterator Function ExportEnzyme(registry As biocad_registry, Optional page_size As Integer = 5000) As IEnumerable(Of FastaSeq)
        Dim terms = registry.biocad_vocabulary
        Dim ec_number As UInteger = terms.db_ECNumber
        Dim prot_fasta As UInteger = terms.protein_data

        For page As Integer = 1 To Integer.MaxValue
            Dim offset As UInteger = (page - 1) * page_size
            Dim page_data As EnzymeSequenceView() = registry.protein_data _
                .left_join("db_xrefs") _
                .on((field("db_xrefs.obj_id") = field("protein_data.id")) And (field("type") = prot_fasta)) _
                .where(field("db_name") = ec_number,
                       field("sequence").char_length > 1) _
                .limit(offset, page_size) _
                .select(Of EnzymeSequenceView)("db_xref AS ec_number", "sequence", "protein_data.id")

            If page_data.IsNullOrEmpty Then
                Exit For
            Else
                Call $"Export enzyme sequence data page {page}...".info

                For Each seq As EnzymeSequenceView In page_data
                    Yield New FastaSeq(seq.sequence, title:=seq.ec_number & " " & seq.id)
                Next
            End If
        Next
    End Function

    Private Class EnzymeSequenceView

        <DatabaseField> Public Property id As UInteger
        <DatabaseField> Public Property ec_number As String
        <DatabaseField> Public Property sequence As String

    End Class
End Module
