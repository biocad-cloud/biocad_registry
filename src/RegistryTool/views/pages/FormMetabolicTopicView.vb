Imports Galaxy.Workbench
Imports Microsoft.VisualBasic.Data.Framework
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports registry_data.Exports

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

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        If DataGridView1.SelectedRows.Count = 0 Then Exit Sub

        Dim row = DataGridView1.SelectedRows(0)
        Dim topic As String = row.Cells(1).Value.ToString
        Dim num As Long = row.Cells(2).Value

        topic_id = topic

        Call CommonRuntime.StatusMessage($"Make export of the {num} id set for topic '{topic}'?")
    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged
        Call DataGridView1_CellContentClick(Nothing, Nothing)
    End Sub

    Dim topic_id As String

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        If topic_id.StringEmpty(, True) Then
            CommonRuntime.Warning("please select a topic row at first!")
            Return
        Else
            Call ExportTagToolStripMenuItem_Click(tag:=topic_id)
        End If
    End Sub

    Private Sub ExportTagToolStripMenuItem_Click(tag As String)
        Using file As New SaveFileDialog With {
            .Filter = "id file(*.txt)|*.txt|Molecule table(*.csv)|*.csv",
            .FileName = tag & ".txt"
        }
            If file.ShowDialog = DialogResult.OK Then
                Call TaskProgress.RunAction(
                    Sub(p As ITaskProgress)
                        If file.FileName.ExtensionSuffix("txt") Then
                            Call Workbench.cad_registry.ExportTagList(tag).SaveTo(file.FileName)
                        Else
                            Call Workbench.cad_registry.ExportTagData(tag).SaveTo(file.FileName)
                        End If
                    End Sub, info:=$"Export topic data of '{tag}' from database...")

                Call MessageBox.Show($"Export topic data of '{tag}' success!",
                                     "Export data success",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Information)
            End If
        End Using
    End Sub

    Private Sub ExportWorkflowIDSetToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportWorkflowIDSetToolStripMenuItem.Click
        Call ToolStripButton1_Click(Nothing, Nothing)
    End Sub
End Class