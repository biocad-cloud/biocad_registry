Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes

Public Class MoleculeSearch

    <DatabaseField> Public Property id As UInteger
    <DatabaseField> Public Property name As String
    <DatabaseField> Public Property formula As String
    <DatabaseField> Public Property mass As Double
    <DatabaseField> Public Property type As String
    <DatabaseField> Public Property note As String

    Public Overrides Function ToString() As String
        Return name
    End Function

End Class
