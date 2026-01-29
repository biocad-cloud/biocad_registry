Imports Galaxy.Workbench.CommonDialogs

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



        Me.DialogResult = DialogResult.OK
    End Sub

    ''' <summary>
    ''' add left
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub AddToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddToolStripMenuItem.Click
        Call InputDialog.Input(Of FormSetSubstrate)(Sub(s)

                                                    End Sub)
    End Sub

    ''' <summary>
    ''' add right
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub AddToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles AddToolStripMenuItem1.Click

    End Sub
End Class