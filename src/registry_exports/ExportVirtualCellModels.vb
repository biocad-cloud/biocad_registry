Imports registry_data
Imports SMRUCC.genomics.SequenceModel.FASTA

Public Class ExportVirtualCellModels

    ReadOnly registry As biocad_registry
    ReadOnly repo As String
    ReadOnly vocabulary As biocad_vocabulary

    Sub New(registry As biocad_registry, repo As String)
        Me.vocabulary = registry.biocad_vocabulary
        Me.repo = repo
        Me.registry = registry
    End Sub

    Public Sub ExportEnzymeDb()
        Using text As New StreamWriter($"{repo}/ec_numbers.fasta".Open(IO.FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False))
            Call text.Add(registry.ExportEnzyme, filterEmpty:=True)
        End Using
    End Sub

End Class
