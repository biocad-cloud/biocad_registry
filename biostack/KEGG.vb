Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Oracle.LinuxCompatibility.MySQL
Imports SMRUCC.WebCloud.HTTPInternal.AppEngine
Imports SMRUCC.WebCloud.HTTPInternal.AppEngine.APIMethods.Arguments
Imports SMRUCC.WebCloud.HTTPInternal.Platform

<[Namespace]("KEGG")> Public Class KEGG : Inherits WebApp

    ReadOnly mysql As New MySQL

    Public Sub New(main As PlatformEngine)
        MyBase.New(main)

        If mysql <= New ConnectionUri With {
            .Database     = App.GetVariable("database"),
            .IPAddress    = App.GetVariable("host"),
            .Password     = App.GetVariable("password"),
            .Port         = App.GetVariable("port"),
            .User         = App.GetVariable("user")
        } = -1.0R Then

            ' Throw New Exception("No MySQL database connection!")
        End If
    End Sub

    <ExportAPI("/KEGG/D3.js/pathway-network.json")>
    Public Function InitPagePathwayNetwork(request As HttpRequest, response As HttpResponse) As Boolean

        Return True
    End Function
End Class
