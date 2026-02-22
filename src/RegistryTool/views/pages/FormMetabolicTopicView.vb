Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes

Public Class FormMetabolicTopicView

    Private Async Sub FormMetabolicTopicView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim metabolite_type As UInteger = Terms.metabolite_type
        Dim q = Await Workbench.cad_registry.topic _
            .left_join("vocabulary") _
            .on(field("`vocabulary`.id") = field("topic_id")) _
            .where(field("type").in({0, metabolite_type})) _
            .group_by("topic_id") _
            .select(Of TopicSet)("topic_id", "term", "COUNT(*) AS size")


    End Sub

    Private Class TopicSet

        <DatabaseField> Public Property topic_id As UInteger
        <DatabaseField> Public Property term As String
        <DatabaseField> Public Property size As Integer

    End Class
End Class