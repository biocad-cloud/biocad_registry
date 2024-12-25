Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports RegistryTool.My
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Class FormOdors

    Dim page As Integer = 1
    Dim page_size As Integer = 3000

    Private Sub FormOdors_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub

    Private Sub loadPage()
        Dim q = OdorData.LoadPage(page, page_size)

        DataGridView1.Rows.Clear()

        For Each item In q
            Dim i = DataGridView1.Rows.Add(item.name, item.formula, item.category, item.odor, item.text)
            Dim row = DataGridView1.Rows(i)

            row.Tag = item
        Next

        DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub

    Private Function getSelected() As OdorData
        Dim selRows = DataGridView1.SelectedRows

        If selRows.Count = 0 Then
            Return Nothing
        Else
            Return selRows(0).Tag
        End If
    End Function

    Private Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click
        Dim odor As OdorData = getSelected()

        If odor Is Nothing Then
            Return
        End If

        If MessageBox.Show($"Going to delete current odor data: {odor.odor} ({odor.name})?", "Delete Confirmed", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) = DialogResult.OK Then

            Call MyApplication.biocad_registry.odor.where(field("id") = odor.id).limit(1).delete()
        End If
    End Sub

    Private Sub DeleteSimilarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteSimilarToolStripMenuItem.Click
        Dim odor As OdorData = getSelected()

        If odor Is Nothing Then
            Return
        End If

        If MessageBox.Show($"Going to delete all odor data: {odor.odor}?", "Delete Confirmed", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) = DialogResult.OK Then

            Call MyApplication.biocad_registry.odor.where(field("odor") = odor.odor).delete()
        End If
    End Sub
End Class

Public Class OdorData

    <DatabaseField> Public Property id As UInteger
    <DatabaseField> Public Property molecule_id As UInteger
    <DatabaseField> Public Property name As String
    <DatabaseField> Public Property formula As String
    <DatabaseField> Public Property category As String
    <DatabaseField> Public Property odor As String
    <DatabaseField> Public Property text As String

    Public Shared Function LoadPage(page As Integer, page_size As Integer) As OdorData()
        Dim offset As Long = (page - 1) * page_size
        Dim q = MyApplication.biocad_registry.odor _
            .left_join("molecule") _
            .on(field("`molecule`.id") = field("molecule_id")) _
            .left_join("vocabulary") _
            .on(field("`vocabulary`.id") = field("`odor`.category")) _
            .limit(offset, page_size) _
            .select(Of OdorData)("`odor`.id",
    "molecule_id",
    "name",
    "formula",
    "`vocabulary`.term AS category",
    "odor",
    "text")

        Return q
    End Function

End Class