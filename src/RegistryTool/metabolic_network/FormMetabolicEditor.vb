Imports Galaxy.Workbench
Imports Galaxy.Workbench.CommonDialogs
Imports Ollama
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
        Dim reactions = Await ReactionModelView.QueryPage(Me.Page, page_size:=500)
        Dim offset As Integer

        Call DataGridView1.Rows.Clear()

        For Each rxn As ReactionModelView In reactions
            offset = DataGridView1.Rows.Add(rxn.db_name, rxn.name, rxn.ec_number, rxn.equation, rxn.note)
            DataGridView1.Rows(offset).Tag = rxn
            DataGridView1.Rows(offset).HeaderCell.Value = rxn.db_xref
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
        Else
            With DirectCast(row.Tag, ReactionModelView)
                Await ShowReaction(DirectCast(row.Tag, ReactionModelView).id)
                ShowReactionEdit(.name, .ec_number, .note)
            End With
        End If
    End Sub

    Dim reaction_id As UInteger

    Private Async Function ShowReaction(rxn_id As UInteger) As Task
        Dim graph = Await MyApplication.biocad_registry.metabolic_network _
            .async _
            .left_join("metabolites") _
            .on(field("species_id") = field("metabolites.id")) _
            .left_join("vocabulary") _
            .on(field("vocabulary.id") = field("role")) _
            .where(field("reaction_id") = rxn_id) _
            .select(Of reaction_graphdata)("metabolites.*", "symbol_id as db_xref", "term AS role")

        Call DataGridView2.Rows.Clear()

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

        reaction_id = rxn_id
    End Function

    Private Sub ShowReactionEdit(name As String, ec$, note$)
        TextBox1.Text = name
        TextBox2.Text = ec
        TextBox3.Text = note
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

    Private Async Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click
        Dim id As String = Strings.Trim(ToolStripTextBox2.Text)
        Dim rxn = Await MyApplication.biocad_registry.reaction _
            .async _
            .where(field("id") = id) _
            .find(Of biocad_registryModel.reaction)

        Await ShowReaction(rxn.id)
        Call ShowReactionEdit(rxn.name, rxn.ec_number, rxn.note)
    End Sub

    Private Sub ToolStripButton5_Click(sender As Object, e As EventArgs) Handles ToolStripButton5.Click
        Call InputDialog.Input(Of FormBuildReaction)()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim prompt As String = $"请为我使用英文介绍 '{TextBox1.Text}' 这个代谢反应，请直接返回无格式标记的纯文本内容，以方便我自动化的放入到报告文本中。"
        Dim msg As DeepSeekResponse = TaskProgress.LoadData(Function(println As Action(Of String)) MyApplication.ollama.Chat(prompt).GetAwaiter.GetResult)

        If msg IsNot Nothing Then
            TextBox3.Text = msg.output
        End If
    End Sub

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If reaction_id > 0 Then
            Await MyApplication.biocad_registry.reaction.async.where(field("id") = reaction_id).save(field("name") = TextBox1.Text)
        End If
    End Sub

    Private Async Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If reaction_id > 0 Then
            Await MyApplication.biocad_registry.reaction.async.where(field("id") = reaction_id).save(field("ec_number") = TextBox2.Text)
        End If
    End Sub

    Private Async Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If reaction_id > 0 Then
            Await MyApplication.biocad_registry.reaction.async.where(field("id") = reaction_id).save(field("note") = TextBox3.Text)
        End If
    End Sub
End Class