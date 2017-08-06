Imports Oracle.LinuxCompatibility.MySQL
Imports SMRUCC.WebCloud.DataCenter
Imports SMRUCC.WebCloud.DataCenter.mysql

Module WebApp

    Sub Main()
        Console.WriteLine("Call Main")

        ' 进行一些数据库的初始化工作

        With New MySQL

            Call .init

            ' 检查WebApp的注册情况
            Dim apps = AppHelper.LoadData(Of BiostackApps)

            For Each app As app In apps
                Call .ExecReplace(app)
            Next
        End With
    End Sub
End Module
