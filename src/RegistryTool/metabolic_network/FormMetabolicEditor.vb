Imports Galaxy.Workbench.CommonDialogs
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data
Imports RegistryTool.My

Public Class FormMetabolicEditor

    Public ReadOnly Property Page As Integer
        Get
            Return CInt(Val(ToolStripTextBox1.Text))
        End Get
    End Property

    Private Async Sub FormMetabolicEditor_Load(sender As Object, e As EventArgs) Handles Me.Load
        Call ApplyVsTheme(ToolStrip1)
        Await GotoPageData()
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

        Await ShowReaction(DirectCast(row.Tag, biocad_registryModel.reaction))
    End Sub

    Private Async Function ShowReaction(rxn As biocad_registryModel.reaction) As Task
        Dim graph = Await Task.Run(
            Function()
                Return MyApplication.biocad_registry.metabolic_network _
                    .left_join("metabolites") _
                    .on(field("species_id") = field("metabolites.id")) _
                    .left_join("vocabulary") _
                    .on(field("vocabulary.id") = field("role")) _
                    .where(field("reaction_id") = rxn.id) _
                    .select(Of reaction_graphdata)("metabolites.*", "symbol_id as db_xref", "term AS role")
            End Function)

        DataGridView2.Rows.Clear()

        For Each compound In graph
            Call DataGridView2.Rows.Add(
                compound.id,
                compound.db_xref,
                compound.name,
                compound.formula,
                compound.exact_mass,
                compound.role
            )
        Next
    End Function

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

    Private Async Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click
        Dim id As String = Strings.Trim(ToolStripTextBox2.Text)
        Dim rxn = Await Task.Run(Function() MyApplication.biocad_registry.reaction.where(field("id") = id).find(Of biocad_registryModel.reaction))

        Await ShowReaction(rxn)
    End Sub

    Private Sub ToolStripButton5_Click(sender As Object, e As EventArgs) Handles ToolStripButton5.Click
        Call InputDialog.Input(Of FormBuildReaction)()
    End Sub
End Class