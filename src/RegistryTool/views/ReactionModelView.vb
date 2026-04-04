Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports RegistryTool.My

Public Class ReactionModelView

    <DatabaseField> Public Property id As UInteger
    <DatabaseField> Public Property db_xref As String
    <DatabaseField> Public Property db_name As String
    <DatabaseField> Public Property name As String
    <DatabaseField> Public Property ec_number As String
    <DatabaseField> Public Property equation As String
    <DatabaseField> Public Property note As String
    <DatabaseField> Public Property hashcode As String

    Public Shared Async Function QueryPage(page As Integer, page_size As Integer) As Task(Of ReactionModelView())
        Dim offset = (page - 1) * page_size
        Dim reactions = Await MyApplication.biocad_registry.reaction _
            .async _
            .left_join("vocabulary").on(field("`vocabulary`.id") = field("db_source")) _
            .limit(offset, page_size) _
            .select(Of ReactionModelView)("reaction.*", "term as db_name")

        Return reactions
    End Function

    Public Shared Async Function QueryByID(id As String) As Task(Of ReactionModelView())
        Return Await MyApplication.biocad_registry.reaction _
            .async _
            .left_join("vocabulary").on(field("`vocabulary`.id") = field("db_source")) _
            .where((field("id") = id) Or (field("db_xref") = id) Or (field("ec_number") = id)) _
            .select(Of ReactionModelView)("reaction.*", "term as db_name")
    End Function

End Class
