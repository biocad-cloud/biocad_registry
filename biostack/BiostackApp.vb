Imports System.Text
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports SMRUCC.WebCloud.HTTPInternal.AppEngine
Imports SMRUCC.WebCloud.HTTPInternal.AppEngine.APIMethods
Imports SMRUCC.WebCloud.HTTPInternal.AppEngine.APIMethods.Arguments
Imports SMRUCC.WebCloud.HTTPInternal.Platform
Imports SMRUCC.WebCloud.HTTPInternal.Scripting

<[Namespace]("Application")> Public Class BiostackApp : Inherits WebApp

    Public Sub New(main As PlatformEngine)
        MyBase.New(main)
    End Sub

    ''' <summary>
    ''' 用户通过这个函数提交数据进行COG注释
    ''' </summary>
    ''' <param name="request"></param>
    ''' <param name="response"></param>
    ''' <returns></returns>
    <ExportAPI("/Application/COG_myva/COG_myva.vbs")>
    <POST(GetType(String))>
    Public Function COGMyva(request As HttpPOSTRequest, response As HttpResponse) As Boolean
        Dim fastaText$ = request.POSTData.Form("fasta.text")
        Dim fastafile$ = App.AppSystemTemp & $"/COG_myva/{App.NextTempName}/query.fasta"

        If fastaText.StringEmpty Then
            ' 用户是通过文件上传的
            Call request.POSTData.Files("fasta.file").SaveAs(fastafile)
        Else
            ' 用户是通过文本框粘贴序列数据上传的
            Call fastaText.SaveTo(fastafile, Encoding.ASCII)
        End If

        Dim task As New COGMyva(
            fastafile, Sub()
                           ' 发送电子邮件给用户告知结果
                       End Sub)

        ' 将任务添加到服务器内部的任务队列之中
        Call PlatformEngine.TaskPool.Queue(task)
        ' 返回结果页面

        Dim html$ = wwwroot & "/Application/COG_myva/result.vbhtml"
        html = vbhtml.ReadHTML(wwwroot, html)

        Call response.WriteHTML(html)

        Return 0
    End Function

    <ExportAPI("/Application/getTask_status.vbs")>
    <[GET](GetType(String))>
    Public Function GetTaskStatus(request As HttpRequest, response As HttpResponse) As Boolean

        Return True
    End Function
End Class
