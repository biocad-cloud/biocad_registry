Imports System.IO
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports SMRUCC.HTTPInternal.AppEngine
Imports SMRUCC.HTTPInternal.AppEngine.APIMethods
Imports SMRUCC.HTTPInternal.Platform

<[Namespace]("seqtools")>
Public Class SequenceTools : Inherits SMRUCC.HTTPInternal.AppEngine.WebApp

    Sub New(main As PlatformEngine)
        Call MyBase.New(main)
    End Sub

    Public Overrides Function Page404() As String
        Return ""
    End Function

    <ExportAPI("/seqtools/DNA_sites.html")>
    <[POST](GetType(String))>
    Public Function SiteParser(args As String, params As StreamReader) As String

    End Function
End Class
