Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports registry_data

Public Class ExportVirtualCellModels

    ReadOnly registry As biocad_registry
    ReadOnly repo As String
    ReadOnly vocabulary As biocad_vocabulary

    Sub New(registry As biocad_registry, repo As String)
        Me.vocabulary = registry.biocad_vocabulary
        Me.repo = repo
        Me.registry = registry
    End Sub



End Class
