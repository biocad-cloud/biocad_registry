Imports Oracle.LinuxCompatibility.MySQL.Uri

Public Class biocad_registry : Inherits biocad_registryModel.db_mysql

    Sub New(mysql As ConnectionUri)
        Call MyBase.New(mysql)
    End Sub
End Class
