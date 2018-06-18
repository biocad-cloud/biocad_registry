Imports biostack
Imports Oracle.LinuxCompatibility.MySQL
Imports Oracle.LinuxCompatibility.MySQL.Uri

Module Module1

    Sub Main()
        Dim mysql As New MySqli
        mysql.Connect(New ConnectionUri With {.Database = "biocad", .IPAddress = "127.0.0.1", .Password = "root", .User = "root"})

        Call App.JoinVariable("OSS_ROOT", "D:\GCModeller-Cloud\php\data")
        Call App.JoinVariable("GCModeller", "E:\GCModeller\GCModeller\bin")
        Call App.JoinVariable("repository", "P:\Resources")

        Dim d As New Daemon(mysql)

        Call d.TaskWorker()
    End Sub
End Module
