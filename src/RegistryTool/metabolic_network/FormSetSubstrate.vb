Imports Galaxy.Workbench
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports registry_data.biocad_registryModel
Imports RegistryTool.My

Public Class FormSetSubstrate

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click, Button5.Click
        If TextBox1.Text.StringEmpty(, True) Then
            Return
        End If

        Dim name = Trim(TextBox1.Text)
        Dim q_str = name.Replace("+", " ").Replace("-", " ").Replace("""", " ").Replace("'", " ")
        Dim hashcode = Trim(name).ToLower.MD5
        Dim sort = $"IF(hashcode = '{hashcode}',
    100000000,
    MATCH (name , note) AGAINST ('{q_str}' IN BOOLEAN MODE))"
        Dim q = TaskProgress.LoadData(Function(p As ITaskProgress)
                                                          Return MyApplication.biocad_registry.metabolites _
            .where(match("name", "note").against(q_str, booleanMode:=True) Or field("hashcode") = hashcode) _
            .order_by(sort, desc:=True) _
            .select(Of SymbolView)
                                                      End Function)
        ListBox1.Items.Clear

        For Each item In q
            ListBox1.Items.Add(item)
        Next
    End Sub

    Dim sel As SymbolView

    Public Function GetSymbol() As SymbolHolder
        Return New SymbolHolder With {
            .factor = NumericUpDown1.Value,
            .species_id = sel.id,
            .symbol_id = sel.GetSymbolId,
            .note = sel.ToString
        }
    End Function

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

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click, Button6.Click
        If TextBox1.Text.StringEmpty(, True) Then
            Return
        ElseIf MessageBox.Show($"Will create a new empty metabolite({NumericUpDown1.Value} {TextBox1.Text}) for this reaction model?", "Create new empty metabolite", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = DialogResult.OK Then
            Dim name = Trim(TextBox1.Text)
            Dim hashcode = LCase(name).MD5

            MyApplication.biocad_registry.metabolites.add(
               field("main_id") = 0,
               field("name") = name,
               field("hashcode") = hashcode,
               field("formula") = "",
               field("exact_mass") = 0
            )

            Dim m = MyApplication.biocad_registry.metabolites.where(field("hashcode") = hashcode).order_by("id", desc:=True).find(Of metabolites)

            If Not m Is Nothing Then
                sel = New SymbolView With {
                    .exact_mass = 0,
                    .formula = "",
                    .id = m.id,
                    .name = m.name
                }
            End If
        End If
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
            Return $"{name} ({formula} - {exact_mass.ToString("F4")})"
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

