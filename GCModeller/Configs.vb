Public Class Configs

    ''' <summary>
    ''' GCModeller/bin
    ''' </summary>
    ''' <returns></returns>
    Public Property bin As String

    Public Shared ReadOnly Property DefaultFile As String =
        App.HOME & "/GCModeller.webApp.json"

End Class
