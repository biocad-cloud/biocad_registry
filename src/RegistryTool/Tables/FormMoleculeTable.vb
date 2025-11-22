Imports Galaxy.Workbench
Imports Microsoft.VisualBasic.Linq
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports RegistryTool.My

Public Class FormMoleculeTable

    Dim page As Integer = 1
    Dim page_size As Integer = 5000

    Private Function GetVocabulary() As Integer
        Select Case ToolStripComboBox1.SelectedItem.ToString
            Case "*" : Return -1
            Case "Gene" : Return MyApplication.biocad_registry.getVocabulary("Nucleic Acid", "Molecule Type")
            Case "Protein" : Return MyApplication.biocad_registry.getVocabulary("Polypeptide", "Molecule Type")
            Case "Metabolite" : Return MyApplication.biocad_registry.getVocabulary("Metabolite", "Molecule Type")
            Case Else
                Return -1
        End Select
    End Function

    Private Async Sub ShowMolbyType() Handles ToolStripComboBox1.SelectedIndexChanged
        Await ResetPage()
    End Sub

    Private Async Function ResetPage() As Task
        page = 1
        Await LoadPage()
    End Function

    Private Async Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click
        page += 1
        Await LoadPage()
    End Sub

    Private Async Function LoadPage() As Task
        Dim type As Integer = GetVocabulary()
        Dim offset As UInteger = (page - 1) * page_size

        ToolStripLabel2.Text = $"Page {page} data"

        Dim q = MyApplication.biocad_registry.molecule _
            .left_join("sequence_graph") _
            .on(field("sequence_graph.molecule_id") = field("molecule.id"))
        Dim qwhere As New List(Of FieldAssert)

        If type > -1 Then
            qwhere.Add(field("molecule.type") = type)
        End If
        If topic IsNot Nothing Then
            qwhere.Add(field("tag_id") = topic.id)
            q = q _
                .left_join("molecule_tags") _
                .on(field("molecule_tags.molecule_id") = field("molecule.id"))
        End If

        If qwhere.Any Then
            q = q.where(qwhere)
        End If

        Dim data = Await Task.Run(Function() q.limit(offset, page_size).distinct.select(Of MoleculeData)("molecule.id AS molecule_id",
    "xref_id",
    "name",
    "formula",
    "mass",
    "sequence",
    "note"))

        Call DataGridView1.Rows.Clear()

        ToolStripProgressBar1.Value = 0
        ToolStripProgressBar1.Maximum = data.Length
        ToolStripProgressBar1.Minimum = 0

        For Each mol As MoleculeData In data
            Dim i = DataGridView1.Rows.Add(mol.molecule_id, mol.xref_id, mol.name, mol.formula, mol.mass, mol.sequence, mol.note)
            Dim r = DataGridView1.Rows(i)

            r.Tag = mol
            ToolStripProgressBar1.Value += 1
        Next

        Call DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Function

    Private Async Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        page -= 1

        If page < 1 Then
            page = 1
            Return
        Else
            Await LoadPage()
        End If
    End Sub

    Private Async Sub FormMoleculeTable_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim topics = MyApplication.biocad_registry.vocabulary.where(field("category") = "Topic").select(Of TopicName)("id", "term")

        Call ToolStripComboBox2.Items.Add("*")

        For Each topic As TopicName In topics.SafeQuery
            Call ToolStripComboBox2.Items.Add(topic)
        Next

        ToolStripComboBox1.SelectedIndex = 0

        Call ApplyVsTheme(ToolStrip1, StatusStrip1, ContextMenuStrip1)

        Await LoadPage()
    End Sub

    Private Async Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Await LoadPage()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentDoubleClick
        If DataGridView1.SelectedRows.Count = 0 Then
            Return
        End If

        Dim row As DataGridViewRow = DataGridView1.SelectedRows.Item(0)
        Dim id As String = CStr(row.Cells(0).Value)

        Call Workbench.OpenMoleculeEditor(id, row.Cells(2).Value)
    End Sub

    Private Sub ViewOnTheWebToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewOnTheWebToolStripMenuItem.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            Return
        End If

        Dim row = DataGridView1.SelectedRows(0)
        Dim id As String = CStr(row.Cells(0).Value)

        id = $"BioCAD{id.PadLeft(11, "0"c)}"

        Dim url As String = $"http://biocad.innovation.ac.cn/molecule/{id}/"

        Call Tools.OpenUrlWithDefaultBrowser(url)
    End Sub

    Dim topic As TopicName

    Private Sub ToolStripComboBox2_Click(sender As Object, e As EventArgs) Handles ToolStripComboBox2.Click

    End Sub

    Private Async Sub ToolStripComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ToolStripComboBox2.SelectedIndexChanged
        If ToolStripComboBox2.SelectedIndex <= 0 Then
            topic = Nothing
        Else
            topic = DirectCast(ToolStripComboBox2.Items(ToolStripComboBox2.SelectedIndex), TopicName)
        End If

        Await ResetPage()
    End Sub

    Private Sub EditToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditToolStripMenuItem.Click
        Call DataGridView1_CellContentDoubleClick(Nothing, Nothing)
    End Sub

    Private Sub RemovesFromCurrentTopicToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemovesFromCurrentTopicToolStripMenuItem.Click
        If topic Is Nothing OrElse topic.id <= 0 Then
            Return
        End If
        If DataGridView1.SelectedRows.Count = 0 Then
            Return
        End If

        Dim row = DataGridView1.SelectedRows(0)
        Dim id As String = CStr(row.Cells(0).Value)

        MyApplication.biocad_registry.molecule_tags.where(field("tag_id") = topic.id, field("molecule_id") = id).delete()
    End Sub
End Class

Public Class TopicName

    <DatabaseField> Public Property id As UInteger
    <DatabaseField> Public Property term As String

    Public Overrides Function ToString() As String
        Return term
    End Function

End Class

Public Class MoleculeData

    <DatabaseField> Public Property molecule_id As UInteger
    <DatabaseField> Public Property xref_id As String
    <DatabaseField> Public Property name As String
    <DatabaseField> Public Property formula As String
    <DatabaseField> Public Property mass As Double
    <DatabaseField> Public Property sequence As String
    <DatabaseField> Public Property note As String

End Class