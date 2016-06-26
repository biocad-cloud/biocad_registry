Imports Microsoft.VisualBasic.Net.Mailto

''' <summary>
''' services@gcmodeller.org
''' </summary>
Module MailServices

    Const AppKey As String = "nhchgmjmsqbrdcbc"

    Public Function GetClient() As EMailClient
        Return Net.Mailto.EMailClient.QQMail("analysis-services@gcmodeller.org", AppKey)
    End Function
End Module
