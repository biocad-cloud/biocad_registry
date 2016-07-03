Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports SMRUCC.HTTPInternal
Imports SMRUCC.HTTPInternal.AppEngine
Imports SMRUCC.HTTPInternal.AppEngine.APIMethods
Imports SMRUCC.HTTPInternal.AppEngine.POSTParser
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
    Public Function SiteParser(args As String, params As PostReader) As String
        Dim hash = args.postRequestParser
        Dim fna = params.Files("fna")
        Dim gff = params.Files("gff")
        Dim ftmp As String = App.GetAppSysTempFile(".fna")
        Dim gtmp As String = App.GetAppSysTempFile(".gff")
        Dim file As String = $"/{App.GetAppSysTempFile().BaseName}/{fna.FileName.Split("."c).First}-{gff.FileName.Split("."c).First}.fasta"
        Dim out As String = App.LocalDataTemp & file
        Dim CLI As String = $"/Gff.Sites /fna {ftmp.CliPath} /gff {gtmp.CliPath} /out {out.CliPath}"

        Call fna.SaveAs(ftmp)
        Call gff.SaveAs(gtmp)
        Call WebApp.Invoke("seqtools", CLI)

        Dim html As HtmlPage = HtmlPage.LoadPage(wwwroot & "/seqtools/DNA_sites.done.html", wwwroot)
        Dim sb As New StringBuilder(html.html)
        Dim name As String = out.BaseName & ".fasta"

        Call sb.Replace("@", file)
        Call sb.Replace("{Name}", name)

        html.html = sb.ToString
        html.html = html.BuildPage(WebApp.Template)

        Return html.html
    End Function
End Class
