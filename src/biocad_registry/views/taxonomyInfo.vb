
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes

Public Class taxonomyInfo

    <DatabaseField> Public Property ncbi_taxid As UInteger
    <DatabaseField> Public Property taxname As String
    <DatabaseField> Public Property rank As String
    <DatabaseField> Public Property parent_id As UInteger
    <DatabaseField> Public Property description As String

End Class