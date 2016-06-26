Imports GCModeller.WebAPP.Framework

Namespace Drawing

    Public Class SeqLogoTask : Inherits TaskCallback

        Public Property outDIR As String

        Sub New(app As String, args As String)
            Call MyBase.New(app, args)
        End Sub

        Public Overrides Sub Callback()

        End Sub
    End Class
End Namespace