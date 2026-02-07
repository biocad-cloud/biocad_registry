Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes

Public Class OrganismSource

    <DatabaseField> Public Property ncbi_taxid As UInteger
    <DatabaseField> Public Property taxname As String

    Public Overrides Function ToString() As String
        Return taxname
    End Function

End Class
