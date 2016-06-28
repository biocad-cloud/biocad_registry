#Region "Microsoft.VisualBasic::1c1f0526687d475a101c2459659f208a, ..\GCModeller\GCModeller.WebAPP.Framework\WebAPP.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2016 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.

#End Region

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
