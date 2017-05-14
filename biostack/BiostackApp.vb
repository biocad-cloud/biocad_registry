Imports System.Text
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Mathematical.HashMaps
Imports SMRUCC.WebCloud.DataCenter.Platform
Imports SMRUCC.WebCloud.HTTPInternal.AppEngine
Imports SMRUCC.WebCloud.HTTPInternal.AppEngine.APIMethods
Imports SMRUCC.WebCloud.HTTPInternal.AppEngine.APIMethods.Arguments
Imports SMRUCC.WebCloud.HTTPInternal.Platform
Imports SMRUCC.WebCloud.HTTPInternal.Scripting

<[Namespace]("Application")> Public Class BiostackApp : Inherits WebApp

    ''' <summary>
    ''' 用户任务池，排队队列
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property TaskPool As New TaskPool

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
        Dim fastafile$ = App.AppSystemTemp & $"/COG_myva/{App.NextTempName}/query.fasta"

        If Not request.POSTData.Form.ContainsKey("fasta.text") Then
            ' 用户是通过文件上传的
            Call request.POSTData.Files("fasta.file").SaveAs(fastafile)
        Else
            ' 用户是通过文本框粘贴序列数据上传的
            Dim fastaText$ = request.POSTData.Form("fasta.text")
            Call fastaText.SaveTo(fastafile, Encoding.ASCII)
        End If

        Dim title$, describ$, email$

        With request.POSTData.Form.ToDictionary
            title = .TryGetValue("task.title")
            describ = .TryGetValue("task.describ")
            email = .TryGetValue("task.email")
        End With

        Dim task As New COGMyva(
            fastafile, Sub()
                           ' 发送电子邮件给用户告知结果
                           If Not email.StringEmpty Then
                               ' send notification email
                           End If
                       End Sub)

        ' 将任务添加到服务器内部的任务队列之中
        Call TaskPool.Queue(task)
        ' 返回结果页面

        Dim html$ = wwwroot & "/Application/COG_myva/result.vbhtml"
        html = vbhtml.ReadHTML(wwwroot, html)

        Call response.WriteHTML(html)

        Return 0
    End Function

#Region "Task Manager"

    <ExportAPI("/Application/TaskManager.vbhtml")>
    Public Function TaskManager(request As HttpRequest, response As HttpResponse) As Boolean
        Dim uid$ = request.URLParameters("uid")

        If uid.StringEmpty Then
            Throw New EntryPointNotFoundException("No task uid was provided!")
        End If

        With TaskPool
            ' 因为任务池之中只有正在执行或者处于排队的任务对象
            ' 所以查询执行完成的任务只能够通过数据库查询来获取
            Dim position% = .GetTaskQueuePosition(uid)

            If position > -1 Then
                ' 任务还在排队，则重定向到排队查看页面
                Dim url$ = "/Application/TaskPending.vbhtml?uid=" & uid
                Return response <= url
            End If

            ' 任务不在队列之中，说明任务已经执行完成了或者正在执行之中
            Dim task As Task = .GetRunningTask(uid)

            If task Is Nothing Then
                ' 任务已经执行完毕，则需要查询数据库来获取结果页面
                Dim hash& = JenkinsHash.hash(uid) ' 为了更加快速的查询数据库，任务表是以哈数值作为主键的，这里需要计算出哈希值
                Dim SQL$
                Dim url$
                Return response <= url
            Else
                ' 任务还在执行状态，则返回状态json
                Dim status As TaskProgress = task.GetProgress
                Call response.WriteJSON(status)
            End If
        End With

        Return True
    End Function
#End Region
End Class
