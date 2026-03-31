Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data.biocad_registryModel

Public Class ProteinModel

    ReadOnly registry As biocad_registry

    ReadOnly cluster_labels As New Dictionary(Of UInteger, String)
    ReadOnly model_labels As New Dictionary(Of UInteger, String)

    Sub New(registry As biocad_registry)
        Me.registry = registry
    End Sub

    Private Function ProteinModelLabel(id As UInteger) As String
        Return model_labels.ComputeIfAbsent(id,
             Function(model_id)
                 Dim model = registry.protein.where(field("id") = id).find(Of protein)
                 Return $"{id}-{RegisterSymbol.makeSymbol(model.name)}"
             End Function)
    End Function

    Private Function ProteinClusterLabel(id As UInteger) As String
        Return cluster_labels.ComputeIfAbsent(id,
             Function(cluster_id)
                 Dim cluster = registry.protein_data.where(field("id") = id).find(Of protein_data)
                 Return $"Protein:{cluster_id}-{RegisterSymbol.makeSymbol(cluster.name)}"
             End Function)
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function GetProteinModelLabel(protein_id As UInteger) As String
        Return GetProteinModelLabel(registry.protein_data.where(field("id") = protein_id).find(Of protein_data)("id", "cluster_id", "protein_id"))
    End Function

    Public Function GetProteinModelLabel(protein As protein_data) As String
        If protein.protein_id > 0 Then
            Return ProteinModelLabel(protein.protein_id)
        ElseIf protein.cluster_id > 0 Then
            Return ProteinClusterLabel(protein.cluster_id)
        Else
            Return ProteinClusterLabel(protein.id)
        End If
    End Function
End Class
