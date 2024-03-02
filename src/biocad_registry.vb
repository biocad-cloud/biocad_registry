Imports Oracle.LinuxCompatibility.MySQL
Imports Oracle.LinuxCompatibility.MySQL.Uri

Public Class biocad_registry : Inherits IDatabase

    Sub New(mysql As ConnectionUri)
        Call MyBase.New(mysql)
    End Sub
End Class
