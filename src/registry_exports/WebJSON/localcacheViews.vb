Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes

Namespace localcacheViews

    Public Class xref_query
        <DatabaseField> Public Property dbname As String
        <DatabaseField> Public Property xref_id As String
    End Class

    Public Class kinetics_args

        <DatabaseField> Public Property params As String
        <DatabaseField> Public Property lambda As String
        <DatabaseField> Public Property metabolite_id As UInteger
        <DatabaseField> Public Property json_str As String

    End Class

    Public Class reaction_group

        <DatabaseField> Public Property hashcode As String
        <DatabaseField> Public Property reactions As String

    End Class

    Public Class operonData

        <DatabaseField> Public Property cluster_id As UInteger
        <DatabaseField> Public Property name As String
        <DatabaseField> Public Property members As String

    End Class

End Namespace