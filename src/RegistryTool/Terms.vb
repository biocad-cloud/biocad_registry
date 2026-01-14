Imports RegistryTool.My

Public NotInheritable Class Terms

    Public Shared ReadOnly Property metabolite_type As UInteger
        Get
            Return MyApplication.biocad_registry.biocad_vocabulary.metabolite_type
        End Get
    End Property

    Private Sub New()
    End Sub


End Class
