Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Oracle.LinuxCompatibility.MySQL
Imports SMRUCC.WebCloud.DataCenter
Imports SMRUCC.WebCloud.HTTPInternal.AppEngine
Imports SMRUCC.WebCloud.HTTPInternal.AppEngine.APIMethods.Arguments
Imports SMRUCC.WebCloud.HTTPInternal.Platform

<[Namespace]("KEGG")> Public Class KEGG : Inherits SMRUCC.WebCloud.HTTPInternal.AppEngine.WebApp

    ReadOnly mysql As New MySQL

    Public Sub New(main As PlatformEngine)
        Call MyBase.New(main)
        Call mysql.init
    End Sub

    <ExportAPI("/KEGG/D3.js/pathway-network.json")>
    Public Function InitPagePathwayNetwork(request As HttpRequest, response As HttpResponse) As Boolean

        Return True
    End Function
End Class
