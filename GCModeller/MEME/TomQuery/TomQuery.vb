Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports GCModeller.WebAPP.Framework
Imports LANS.SystemsBiology.GCModeller.Workbench
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports SMRUCC.HTTPInternal.AppEngine.APIMethods
Imports SMRUCC.HTTPInternal.AppEngine.POSTParser
Imports SMRUCC.HTTPInternal.Platform

Namespace TomQuery

    <[Namespace]("Tom.Query")>
    Public Class API : Inherits WebAPP

        Sub New(main As PlatformEngine)
            Call MyBase.New(main)
            Call main.AddMappings(Settings.DataCache, "/tom.query/cache/")
        End Sub

        <POST(GetType(String)), ExportAPI("/tom.query/swtom.query.run_task.vb")>
        Public Function TomQueryTask(args As String, inputs As PostReader) As String
            Dim email As String = inputs.Form("email")
            Dim jobTitle As String = inputs.Form("job_title")
            Dim File As HttpPostedFile = inputs.Files("memeText")
            Dim fileData As String '= (From x As Content In data
            '                          Where String.Equals(x.Name, "memeText", StringComparison.OrdinalIgnoreCase) AndAlso
            '                          String.IsNullOrEmpty(x.FileName)
            '                          Select x.content).FirstOrDefault
            If String.IsNullOrEmpty(fileData) OrElse String.Equals(fileData, vbLf) Then
                ' fileData = File.InputStream.re
            End If

            Dim name As String = getMEMEQueryName(fileData)
            Dim uid As String = Now.ToBinary & "." & name  ' 多个用户同一个时间点提交会出问题，在这里再加一个名称来避免
            Dim memeText As String = $"{App.GetAppSysTempFile()}/{name}.txt"
            Dim out As String = Settings.DataCache & "/" & uid
            Dim resultPage As String = __showResultPage & $"?uid={uid}"

            args = $"/SWTOM.Query /query {memeText.CliPath} /out {out.CliPath}"

            Dim task As New SWTomTask(App.HOME & "/GCModeller/meme.exe", args) With {
                .EMail = email,
                .JobTitle = jobTitle,
                .out = out,
                .uid = uid,
                .ResultPage = resultPage
            }

            Call fileData.SaveTo(memeText)

            Return __joinTask(task)
        End Function

        ReadOnly __showResultPage As String =
            GetType(API).GetMethod(NameOf(SWTomShowResult)).GetAttribute(Of ExportAPIAttribute).Name

        ''' <summary>
        ''' 将index.html返回
        ''' </summary>
        ''' <param name="args"></param>
        ''' <returns></returns>
        <[GET](GetType(String)),
         ExportAPI("/tom.query/swtom.query.show_result.vb", Usage:="/tom.query/swtom.query.show_result.vb?uid=<uid>")>
        Public Function SWTomShowResult(args As String) As String
            Dim arg As Dictionary(Of String, String) = args.postRequestParser
            Dim uid As String = arg("uid")
            Dim page As String = Settings.DataCache & "/" & uid & "/index.html"
            Dim index As New StringBuilder(FileIO.FileSystem.ReadAllText(page))
            Dim m As String() = Regex.Matches(index.ToString, "<td><a href="".+?/TomQuery.html"">.+?</a></td>").ToArray
            For Each s As String In m
                Dim s2 = s.Replace("href=""", $"href=""/tom.query/cache/{uid}/")
                Call index.Replace(s, s2)
            Next

            Return index.ToString
        End Function
    End Class

End Namespace