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
        Dim page_size As Integer = 100
        Dim ec_id As UInteger = registry.vocabulary_terms.ecnumber_term
        Dim last_id As UInteger = 0
        Dim prot_key As UInteger = registry.vocabulary_terms.protein_term

        For page As Integer = 1 To Integer.MaxValue
            Dim seqs_page = registry.db_xrefs _
                .left_join("sequence_graph") _
                .on(field("sequence_graph.molecule_id") = field("obj_id")) _
                .where(field("db_key") = ec_id, field("type") = prot_key, field("obj_id") > last_id) _
                .limit(page_size) _
                .select(Of EnzymeSequence)("obj_id as cad_id", "xref as ec_number", "sequence")

            If seqs_page.IsNullOrEmpty Then
                Exit For
            Else
                last_id = seqs_page.Select(Function(s) s.cad_id).Max
            End If

            For Each seq As EnzymeSequence In seqs_page
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