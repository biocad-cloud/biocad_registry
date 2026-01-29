Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports RegistryTool.My

Public Class FormSetSubstrate

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text.StringEmpty(, True) Then
            Return
        End If

        Dim q_str As String = TextBox1.Text.Replace("+", " ").Replace("-", " ")
        Dim q = MyApplication.biocad_registry.metabolites.where(match("name", "note").against(q_str, booleanMode:=True)).select(Of SymbolView)

        Call ListBox1.Items.Clear()

        For Each item In q
            Call ListBox1.Items.Add(item)
        Next
    End Sub

    Dim sel As SymbolView

    Private Sub SetMoleculeReferenceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SetMoleculeReferenceToolStripMenuItem.Click
        If ListBox1.SelectedIndex = -1 Then
            Return
        End If

        sel = ListBox1.SelectedItem
        Label4.Text = sel.ToString
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If sel Is Nothing Then
            Return
        End If

        Me.DialogResult = DialogResult.OK
    End Sub

    Private Class SymbolView

        <DatabaseField> Public Property id As UInteger

        <DatabaseField> Public Property name As String
        <DatabaseField> Public Property formula As String
        <DatabaseField> Public Property exact_mass As Double
        <DatabaseField> Public Property cas_id As String
        <DatabaseField> Public Property hmdb_id As String
        <DatabaseField> Public Property lipidmaps_id As String
        <DatabaseField> Public Property kegg_id As String
        <DatabaseField> Public Property biocyc As String

        Public Overrides Function ToString() As String
            Return $"{name} ({formula} - {exact_mass})"
        End Function

        Public Function GetSymbolId() As String
            If Not cas_id.StringEmpty(, True) Then Return cas_id
            If Not hmdb_id.StringEmpty(, True) Then Return hmdb_id
            If Not lipidmaps_id.StringEmpty(, True) Then Return lipidmaps_id
            If Not kegg_id.StringEmpty(, True) Then Return kegg_id
            If Not biocyc.StringEmpty(, True) Then Return biocyc

            Return "BioCAD" & id.ToString.PadLeft(11, "0"c)
        End Function

    End Class
End Class

