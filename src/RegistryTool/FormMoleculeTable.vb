Public Class FormMoleculeTable

    Dim page As Integer
    Dim page_size As Integer = 5000



    Private Sub ToolStripComboBox1_Click(sender As Object, e As EventArgs) Handles ToolStripComboBox1.Click

    End Sub

    Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click
        page += 1
        LoadPage()
    End Sub

    Private Sub LoadPage()
        ToolStripLabel2.Text = $"Page {page} data"


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
End Class