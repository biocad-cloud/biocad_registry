Imports RegistryTool.My
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports biocad_storage

Public Class FormBatchEditor

    Dim list As MoleculeBatchView()

    Public Sub LoadFromIDSet(ids As IEnumerable(Of String))
        list = MoleculeBatchView.FromIdSet(ids.Select(Function(id) UInteger.Parse(id.Match("\d+"))))

        Call DataGridView1.Rows.Clear()

        For Each mol As MoleculeBatchView In list
            Dim offset = DataGridView1.Rows.Add(mol.id, mol.name, mol.tags, mol.note)
            DataGridView1.Rows(offset).Tag = mol
        Next

        DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
        updateTitle()
    End Sub

    Private Sub refreshUI()
        Call LoadFromIDSet(From mol In list Select mol.id)
    End Sub

    Private Sub OpenInEditorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenInEditorToolStripMenuItem.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            Return
        End If

        Dim sel As MoleculeBatchView = DataGridView1.SelectedRows(0).Tag

        If sel Is Nothing Then
            Return
        End If

        Call Workbench.OpenMoleculeEditor(sel.id, sel.name)
    End Sub

    Private Sub RemoveFromListToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoveFromListToolStripMenuItem.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            Return
        End If

        Dim sel As MoleculeBatchView = DataGridView1.SelectedRows(0).Tag

        If sel Is Nothing Then
            Return
        End If

        list = list.Where(Function(a) a.id <> sel.id).ToArray
        DataGridView1.Rows.Remove(DataGridView1.SelectedRows(0))
        DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
        updateTitle()
    End Sub

    Private Sub updateTitle()
        Text = $"Batch Editor [{list.Length} molecules]"
    End Sub

    Private Sub FormBatchEditor_Load(sender As Object, e As EventArgs) Handles Me.Load
        For Each term As TopicTerm In TopicTerm.GetTopics
            Call ComboBox1.Items.Add(term)
        Next
    End Sub

    ''' <summary>
    ''' add
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If ComboBox1.SelectedIndex < 0 Then
            Return
        End If

        Dim term As TopicTerm = ComboBox1.SelectedItem

        For Each mol As MoleculeBatchView In list
            If MyApplication.biocad_registry.molecule_tags.where(field("tag_id") = term.id, field("molecule_id") = mol.id).find(Of biocad_registryModel.molecule_tags) Is Nothing Then
                Call MyApplication.biocad_registry.molecule_tags.add(field("tag_id") = term.id, field("molecule_id") = mol.id, field("description") = "Batch Editor")
            End If
        Next

        Call refreshUI()
    End Sub

    ''' <summary>
    ''' remove
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If ComboBox1.SelectedIndex < 0 Then
            Return
        End If

        Dim term As TopicTerm = ComboBox1.SelectedItem

        For Each mol As MoleculeBatchView In list
            MyApplication.biocad_registry.molecule_tags.where(field("tag_id") = term.id, field("molecule_id") = mol.id).delete()
        Next

        Call refreshUI()
    End Sub
End Class