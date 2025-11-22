Imports biocad_storage
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports RegistryTool.My

Public Class FormMetabolicEditor

    Public ReadOnly Property Page As Integer
        Get
            Return CInt(Val(ToolStripTextBox1.Text))
        End Get
    End Property

    Private Sub FormMetabolicEditor_Load(sender As Object, e As EventArgs) Handles Me.Load
        Call ApplyVsTheme(ToolStrip1)
    End Sub

    Private Async Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        If Page = 1 Then
            Return
        End If

        ToolStripTextBox1.Text = Page - 1

        Await GotoPageData()
    End Sub

    Private Async Sub GotoPage() Handles ToolStripButton2.Click
        Await GotoPageData()
    End Sub

    Private Async Function GotoPageData() As Task
        Dim page As Integer = Me.Page
        Dim page_size As Integer = 1000
        Dim offset = (page - 1) * page_size
        Dim reactions = Await Task.Run(Function() MyApplication.biocad_registry.reaction.limit(offset, page_size).select(Of biocad_registryModel.reaction))

        Call DataGridView1.Rows.Clear()

        For Each rxn In reactions
            offset = DataGridView1.Rows.Add(rxn.name, rxn.equation, rxn.note)
            DataGridView1.Rows(offset).Tag = rxn
            DataGridView1.Rows(offset).HeaderCell.Value = rxn.hashcode
        Next

        Call DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Function

    Private Async Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        ToolStripTextBox1.Text = Page + 1
        Await GotoPageData()
    End Sub

    Private Async Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        If DataGridView1.SelectedRows.Count = 0 Then
            Return
        End If

        Dim row = DataGridView1.SelectedRows(0)

        If row.Tag Is Nothing Then
            Return
        End If

        Dim rxn As biocad_registryModel.reaction = DirectCast(row.Tag, biocad_registryModel.reaction)
        Dim graph = Await Task.Run(
            Function()
                Return MyApplication.biocad_registry.reaction_graph _
                    .left_join("molecule") _
                    .on(field("molecule_id") = field("molecule.id")) _
                    .left_join("vocabulary") _
                    .on(field("vocabulary.id") = field("role")) _
                    .where(field("reaction") = rxn.id) _
                    .select(Of reaction_graphdata)("molecule.*", "db_xref", "term AS role")
            End Function)

        DataGridView2.Rows.Clear()

        For Each compound In graph
            Call DataGridView2.Rows.Add(
                compound.id,
                compound.db_xref,
                compound.name,
                compound.formula,
                compound.mass,
                compound.role
            )
        Next
    End Sub

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        If DataGridView2.SelectedRows.Count = 0 Then
            Return
        End If

        Dim meta = DataGridView2.SelectedRows(0)
        Dim registry_id As String = CStr(meta.Cells(0).Value)
        Dim name As String = CStr(meta.Cells(1).Value)

        If registry_id <> "" Then
            Call Workbench.OpenMoleculeEditor(registry_id, name)
        End If
    End Sub
End Class