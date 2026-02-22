Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes

Public Class FormMetabolicTopicView

    Private Async Sub FormMetabolicTopicView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await LoadTable()
    End Sub

    Private Async Function LoadTable() As Task
        Dim metabolite_type As UInteger = Terms.metabolite_type
        Dim q = Await Workbench.cad_registry.topic.async _
            .left_join("vocabulary") _
            .on(field("`vocabulary`.id") = field("topic_id")) _
            .where(field("type").in({0, metabolite_type})) _
            .group_by("topic_id") _
            .select(Of TopicSet)("topic_id", "term", "COUNT(*) AS size")

        Call DataGridView1.Rows.Clear()

        For Each topic As TopicSet In q
            Call DataGridView1.Rows.Add(topic.topic_id, topic.term, topic.size)
        Next
    End Function

    Private Class TopicSet

        <DatabaseField> Public Property topic_id As UInteger
        <DatabaseField> Public Property term As String
        <DatabaseField> Public Property size As Long

    End Class
End Class