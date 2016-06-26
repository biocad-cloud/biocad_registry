Imports LANS.SystemsBiology.GCModeller.Workbench
Imports Microsoft.VisualBasic.CommandLine
Imports SMRUCC.HTTPInternal.AppEngine.APIMethods
Imports SMRUCC.HTTPInternal.Platform

Public MustInherit Class WebAPP : Inherits SMRUCC.HTTPInternal.AppEngine.WebApp

    Sub New(main As PlatformEngine)
        Call MyBase.New(main)
        Call Settings.Initialize()
    End Sub

    Protected Function __joinTask(task As TaskCallback) As String
        Dim queuePos As Integer = PlatformEngine.TaskPool.Queue(task.Task)
        Dim title As String = task.JobTitle & " submit success"
        Dim innerHTML As String

        If queuePos > 0 Then
            innerHTML = $"
<p>Task '{task.JobTitle}' submit successful, but the server is busy now and your task in queue position ""{queuePos}"", result will be send to {task.EMail} once the job is complete.</p>
<p>And you also can bookmark and check the result on this page: <br />
    <a href=""{task.ResultPage}"">http://services.gcmodeller.org{task.ResultPage}</a>
</p>" & ReportBuilder.BackPreviousPage
        Else
            innerHTML = $"
<p>Task '{task.JobTitle}' submit successful, result will be send to {task.EMail} once the job is complete.</p>
<p>And you also can bookmark and check the result on this page: <br />
    <a href=""{task.ResultPage}"">http://services.gcmodeller.org{task.ResultPage}</a>
</p>" & ReportBuilder.BackPreviousPage
        End If

        Return ReportBuilder.GetHTML(innerHTML, title)
    End Function

    Public Overrides Function Page404() As String
        Return ReportBuilder.Error404
    End Function
End Class

Public MustInherit Class TaskCallback

    Public Property ResultPage As String
    Public Property EMail As String
    Public Property JobTitle As String

    ReadOnly _IO As IIORedirectAbstract
    Public ReadOnly Property Task As Task
    Public Property uid As String
        Get
            Return Task.uid
        End Get
        Set(value As String)
            Task.uid = value
        End Set
    End Property

    ''' <summary>
    '''
    ''' </summary>
    ''' <param name="app">命令行程序</param>
    ''' <param name="args">命令行参数</param>
    Sub New(app As String, args As String)
        Call Me.New(New IORedirectFile(app, args))
    End Sub

    Sub New(cmd As IIORedirectAbstract)
        _IO = cmd
        _Task = New Task(AddressOf _IO.Run, AddressOf Callback)
    End Sub

    Public MustOverride Sub Callback()

    Public Overrides Function ToString() As String
        Return _IO.ToString
    End Function

End Class