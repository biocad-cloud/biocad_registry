Imports RegistryTool.My
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes

Public Class FormMoleculeTable

    Dim page As Integer = 1
    Dim page_size As Integer = 5000

    Private Function GetVocabulary() As Integer
        Select Case ToolStripComboBox1.SelectedItem.ToString
            Case "*" : Return -1
            Case "Gene" : Return 1
            Case "Protein" : Return 3
            Case "Metabolite" : Return 4
            Case Else
                Return -1
        End Select
    End Function

    Private Sub ToolStripComboBox1_Click(sender As Object, e As EventArgs) Handles ToolStripComboBox1.Click
        page = 1
        LoadPage()
    End Sub

    Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click
        page += 1
        LoadPage()
    End Sub

    Private Sub LoadPage()
        Dim type As Integer = GetVocabulary()
        Dim offset As UInteger = (page - 1) * page_size

        ToolStripLabel2.Text = $"Page {page} data"

        Dim q = MyApplication.biocad_registry.molecule _
            .left_join("sequence_graph") _
            .on(field("sequence_graph.molecule_id") = field("molecule.id"))

        If type > -1 Then
            q = q.where(field("molecule.type") = type)
        End If

        Dim data = q.limit(offset, page_size).select(Of MoleculeData)("molecule.id AS molecule_id",
    "xref_id",
    "name",
    "formula",
    "mass",
    "sequence",
    "note")

        Call DataGridView1.Rows.Clear()

        For Each mol In data
            Dim i = DataGridView1.Rows.Add(mol.molecule_id, mol.xref_id, mol.name, mol.formula, mol.mass, mol.sequence, mol.note)
            Dim r = DataGridView1.Rows(i)

            r.Tag = mol
        Next

        Call DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        page -= 1

        If page < 1 Then
            page = 1
            Return
        Else
            LoadPage()
        End If
    End Sub

    Private Sub FormMoleculeTable_Load(sender As Object, e As EventArgs) Handles Me.Load
        ToolStripComboBox1.SelectedIndex = 0
        LoadPage()
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        LoadPage()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentDoubleClick
        If DataGridView1.SelectedRows.Count = 0 Then
            Return
        End If

        Dim row As DataGridViewRow = DataGridView1.SelectedRows.Item(0)
        Dim id As String = CStr(row.Cells(0).Value)
        Dim edit As New FormMoleculeEditor With {.id = id}

        edit.MdiParent = MyApplication.host
        edit.Text = $"BioCAD{id.PadLeft(11, "0"c)} - {row.Cells(2).Value}"
        edit.Show()
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