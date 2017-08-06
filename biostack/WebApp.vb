Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Oracle.LinuxCompatibility.MySQL
Imports SMRUCC.WebCloud.DataCenter
Imports Microsoft.VisualBasic.Terminal
Imports SMRUCC.WebCloud.DataCenter.mysql
Imports sys = Microsoft.VisualBasic.Terminal.STDIO

<RunDllEntryPoint(NameOf(WebApp))> Module WebApp

    Sub New()
        Call Settings.Session.Initialize()
    End Sub

    Sub Main()
        With New MySQL

            ' 进行一些数据库的初始化工作
            Call .init

            ' 检查WebApp的注册情况
            Dim apps = AppHelper.LoadData(Of BiostackApps)

            For Each app As app In apps
                Call .ExecReplace(app)
            Next
        End With
    End Sub

    Public Sub RunConfig()
        Dim readString = Function(s$, ByRef result$)
                             result = s
                             Return Not s.StringEmpty
                         End Function

        Dim repo$ = sys.Read("Input the directory location that where the repository data is?", readString, HOME & "/repo/")
        Dim blast = sys.Read("Input the directory location of the NCBI blast+ suite", readString, HOME & "/blast+/")

        With Settings.SettingsFile

            .RepositoryRoot = repo
            .BlastBin = blast

            Call .Save()

        End With
    End Sub
End Module
