Imports System.Text
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports SMRUCC.WebCloud.HTTPInternal.AppEngine
Imports SMRUCC.WebCloud.HTTPInternal.AppEngine.APIMethods
Imports SMRUCC.WebCloud.HTTPInternal.AppEngine.APIMethods.Arguments
Imports SMRUCC.WebCloud.HTTPInternal.Platform

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
    <ExportAPI("/Application/tools/COG_myva.vbs")>
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

        Dim task As New COGMyva(fastafile)
        ' 将任务添加到服务器内部的任务队列之中
        Call PlatformEngine .TaskPool .Queue ( task )

        Return 0
    End Function

    <ExportAPI("/Application/getTask_status.vbs")>
    <[GET](GetType(String))>
    Public Function GetTaskStatus(request As HttpRequest, response As HttpResponse) As Boolean
        Call response.WriteJSON(New COGMyva("").GetProgress)
        Return True
    End Function
End Class
