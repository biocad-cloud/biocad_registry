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

    Public Sub New(main As PlatformEngine)
        Call MyBase.New(main)
        Call MySqliHelper.init(mysqli)
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
        Dim workspace$ = ""
    End Sub
End Class
