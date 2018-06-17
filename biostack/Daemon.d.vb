Imports System.Threading
Imports biostack.MySql
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Oracle.LinuxCompatibility.MySQL
Imports Oracle.LinuxCompatibility.MySQL.Expressions
Imports SMRUCC.WebCloud.HTTPInternal
Imports SMRUCC.WebCloud.HTTPInternal.AppEngine
Imports SMRUCC.WebCloud.HTTPInternal.AppEngine.APIMethods.Arguments
Imports SMRUCC.WebCloud.HTTPInternal.Platform

''' <summary>
''' 数据分析任务的后台守护进程
''' </summary>
''' 
<[Namespace]("biostack.d")>
Public Class Daemon : Inherits WebApp

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

    <ExportAPI("/biostack.d/task_push.vbs")>
    Public Function TaskPush(request As HttpRequest, response As HttpResponse) As Boolean
        Call PlatformEngine.RunTask(AddressOf TaskWorker)
        Call response.SuccessMsg("Task trigger pushed!")

        Return True
    End Function

    Public Sub TaskWorker()
        Dim task As bioCAD.task

        Do While True
            task = New Table(Of bioCAD.task)(mysqli).Where($"`status` = '0'").Find

            If task Is Nothing Then
                Exit Do
            Else
                Call taskWorker(task)
            End If

            Call Thread.Sleep(10 * 1000)
        Loop

        Call "No pending task, thread exit....".__INFO_ECHO
    End Sub

    Private Sub taskWorker(task As bioCAD.task)
        Dim app As IBiostackApp = AppContainer.GetApp(task.app_id)
        ' https://github.com/GCModeller-Cloud/bioCAD/blame/ca6c0ecddfdebbe08a7d7b42e11e456f5cdc3f35/biostack/api.php#L54
        Dim workspace$ = $"/data/upload/{task.user_id}/{task.app_id}/{task.id}/"
        Dim exception = app.RunApp(task.parameters, OSS_ROOT & workspace)

        If Not exception Is Nothing Then

        End If
    End Sub
End Class
