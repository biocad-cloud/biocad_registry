Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes

Namespace views

    Public Class OperonGene

        <DatabaseField> Public Property cluster_id As UInteger
        <DatabaseField> Public Property gene_id As UInteger
        <DatabaseField> Public Property name As String
        <DatabaseField> Public Property sequence As String

    End Class
End Namespace