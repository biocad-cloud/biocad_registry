
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes

Public Class XrefID

    <DatabaseField> Public Property xref_id As UInteger
    <DatabaseField> Public Property dbname As String
    <DatabaseField> Public Property xref As String
    <DatabaseField> Public Property db_key As UInteger

    Public Overrides Function ToString() As String
        Return $"{dbname} - {xref}"
    End Function

End Class