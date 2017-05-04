Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports SMRUCC.WebCloud.HTTPInternal.AppEngine
Imports SMRUCC.WebCloud.HTTPInternal.AppEngine.APIMethods
Imports SMRUCC.WebCloud.HTTPInternal.AppEngine.APIMethods.Arguments
Imports SMRUCC.WebCloud.HTTPInternal.Platform

<[Namespace]("biostack")> Public Class Platform : Inherits WebApp

    Public Sub New(main As PlatformEngine)
        MyBase.New(main)
    End Sub

    <ExportAPI("/biostack/search.vbs",
               Usage:="/biostack/search.vbs?q=term")>
    <[GET](GetType(String))>
    Public Function Search(request As HttpRequest, response As HttpResponse) As Boolean
        Throw New NotImplementedException
    End Function
End Class
