Imports Galaxy.Workbench.CommonDialogs
Imports registry_data
Imports RegistryTool.My

Public Class FormBuildReaction
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If TextBox1.Text.StringEmpty(, True) Then
            Return
        End If
        If ListBox1.Items.Count = 0 Then
            Return
        End If
        If ListBox2.Items.Count = 0 Then
            Return
        End If

        Call Create()

        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub Create()

    End Sub

    ''' <summary>
    ''' add left
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub AddToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddToolStripMenuItem.Click
        Call InputDialog.Input(Of FormSetSubstrate)(
            Sub(s)
                Dim symbol As SymbolHolder = s.GetSymbol
                symbol.role = MyApplication.biocad_registry.MetabolicSubstrateRole.id
                ListBox1.Items.Add(symbol)
            End Sub)
    End Sub

    ''' <summary>
    ''' add right
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub AddToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles AddToolStripMenuItem1.Click
        Call InputDialog.Input(Of FormSetSubstrate)(
            Sub(s)
                Dim symbol As SymbolHolder = s.GetSymbol
                symbol.role = MyApplication.biocad_registry.MetabolicProductRole.id
                ListBox2.Items.Add(symbol)
            End Sub)
    End Sub
End Class

Public Class SymbolHolder

    Public Property factor As Double
    Public Property species_id As UInteger
    Public Property role As UInteger
    Public Property symbol_id As String
    Public Property note As String

    Public Overrides Function ToString() As String
        Return $"{factor} {note}"
    End Function

End Class