Imports System.Runtime.CompilerServices
Imports Oracle.LinuxCompatibility.MySQL

Module DataExtensions

    ''' <summary>
    ''' 从环境变量之中初始化mysql的数据库连接的通用拓展
    ''' </summary>
    ''' <param name="mysql"></param>
    <Extension> Public Sub __init(ByRef mysql As MySQL)
        If mysql <= New ConnectionUri With {
            .Database = App.GetVariable("database"),
            .IPAddress = App.GetVariable("host"),
            .Password = App.GetVariable("password"),
            .Port = App.GetVariable("port"),
            .User = App.GetVariable("user")
        } = -1.0R Then

#If Not DEBUG Then
            Throw New Exception("No MySQL database connection!")
#End If
        End If
    End Sub
End Module
