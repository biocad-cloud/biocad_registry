Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes

Namespace views

    Public Class EnzymeReaction

        <DatabaseField> Public Property id As UInteger
        <DatabaseField> Public Property term As String
        <DatabaseField> Public Property db_xref As String
        <DatabaseField> Public Property name As String

    End Class
End Namespace