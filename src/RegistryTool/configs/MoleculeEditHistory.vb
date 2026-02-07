Namespace Configs

    Public Class MoleculeEditHistory

        Public Property id As String
        Public Property name As String

        Public Overrides Function ToString() As String
            Return $"{id} - {name}"
        End Function

    End Class
End Namespace