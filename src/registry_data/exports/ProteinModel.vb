Imports registry_data
Imports registry_data.biocad_registryModel

Public Class ProteinModel

    ReadOnly registry As biocad_registry

    Sub New(registry As biocad_registry)
        Me.registry = registry
    End Sub

    Public Function GetProteinModelLabel(protein As protein_data) As String
        If protein.protein_id > 0 Then

        End If
    End Function
End Class
