Imports System.Threading
Imports biostack.MySql
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Oracle.LinuxCompatibility.MySQL
Imports Oracle.LinuxCompatibility.MySQL.Expressions
Imports SMRUCC.WebCloud.HTTPInternal
Imports SMRUCC.WebCloud.HTTPInternal.AppEngine
Imports SMRUCC.WebCloud.HTTPInternal.AppEngine.APIMethods
Imports SMRUCC.WebCloud.HTTPInternal.AppEngine.APIMethods.Arguments
Imports SMRUCC.WebCloud.HTTPInternal.Platform

''' <summary>
''' 数据分析任务的后台守护进程
''' </summary>
''' 
<[Namespace]("biostack.d")> Public Class Daemon : Inherits WebApp

    ReadOnly mysqli As New MySqli
    ReadOnly OSS_ROOT$

    Public Sub New(main As PlatformEngine)
        Call MyBase.New(main)
        Call MySqliHelper.init(mysqli)

        OSS_ROOT = App.GetVariable("OSS_ROOT")

        If OSS_ROOT.DirectoryExists Then
            Call $"OSS_ROOT mounted at: {OSS_ROOT}".__INFO_ECHO
        Else
            Call $"Invalid OSS_ROOT location: {OSS_ROOT}".PrintException
            Throw New InvalidOperationException(OSS_ROOT)
        End If
    End Sub

    ''' <summary>
    ''' Debugger
    ''' </summary>
    ''' <param name="mysql"></param>
    Sub New(mysql As MySqli)
        Call MyBase.New(Nothing)

        mysqli = mysql
        OSS_ROOT = App.GetVariable("OSS_ROOT")
    End Sub

    <ExportAPI("/biostack.d/task_push.vbs")>
    <[GET]>
    Public Function TaskPush(request As HttpRequest, response As HttpResponse) As Boolean
        Call PlatformEngine _
            .RunTask(Sub()
                         Call TaskWorker(cleanup:=False)
                     End Sub)
        Call response.SuccessMsg("Task trigger pushed!")

        Return True
    End Function

    Public Sub TaskWorker(cleanup As Boolean)
        Dim task As bioCAD.task
        Dim condition$

        If cleanup Then
            condition = "`status` = '0' OR `status` = '1'"
        Else
            condition = "`status` = '0'"
        End If

        Do While True
            task = New Table(Of bioCAD.task)(mysqli) _
                .Where(condition) _
                .Find

            If task Is Nothing Then
                Exit Do
            Else
                task.status = 1

                Call mysqli.ExecUpdate(task)
                Call taskWorker(task)
            End If

            Call Thread.Sleep(10 * 1000)
        Loop

        Call "No pending task, thread exit....".__INFO_ECHO
    End Sub

    Private Sub taskWorker(task As bioCAD.task)
        Dim app As IBiostackApp = AppContainer.GetApp(task.app_id)
        ' https://github.com/GCModeller-Cloud/bioCAD/blame/ca6c0ecddfdebbe08a7d7b42e11e456f5cdc3f35/biostack/api.php#L54
        Dim workspace$ = $"/upload/{task.user_id}/{task.app_id}/{task.id}/"

        If task.user_id > 0 Then
            ' 是一个注册用户，会根据用户的账户配置发送电子邮件
        End If

        Dim exception = app.RunApp(task.parameters, OSS_ROOT & workspace)

        If Not exception Is Nothing Then
            task.status = 500
        Else
            task.status = 200
        End If

        Call mysqli.ExecUpdate(task)
    End Sub
End Class
