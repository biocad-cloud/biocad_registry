Imports System.Runtime.CompilerServices
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports SMRUCC.genomics.SequenceModel.FASTA

Public Module ProtFasta

    ''' <summary>
    ''' export protein sequence of the enzyme
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <returns></returns>
    <Extension>
    Public Iterator Function ExportEnzyme(registry As biocad_registry) As IEnumerable(Of FastaSeq)
        Dim page_size As Integer = 1000
        Dim ec_id As UInteger = registry.vocabulary_terms.ecnumber_term

        For page As Integer = 1 To Integer.MaxValue
            Dim seqs = registry.db_xrefs _
                .left_join("sequence_graph") _
                .on(field("sequence_graph.molecule_id") = field("obj_id")) _
                .where(field("db_key") = ec_id) _
                .select(Of EnzymeSequence)("obj_id as cad_id", "xref as ec_number", "sequence")

            If seqs.IsNullOrEmpty Then
                Exit For
            End If

            For Each seq As EnzymeSequence In seqs
                Yield New FastaSeq With {
                    .Headers = {
                        seq.ec_number,
                        "BioCAD" & seq.cad_id.ToString.PadLeft(11, "0"c)
                    },
                    .SequenceData = seq.sequence
                }
            Next
        Next
    End Function
End Module

Public Class EnzymeSequence

    <DatabaseField> Public Property cad_id As UInteger
    <DatabaseField> Public Property ec_number As String
    <DatabaseField> Public Property sequence As String

End Class