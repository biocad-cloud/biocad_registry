Public Class FormTextEditor

    Public ReadOnly Property TextData As String
        Get
            Return TextBox1.Text
        End Get
    End Property

    Public ReadOnly Property TextLines As String()
        Get
            Return TextBox1.Text _
                .LineTokens _
                .Where(Function(s) Not s.StringEmpty()) _
                .ToArray
        End Get
    End Property

    Public Sub SetText(txt As String)
        TextBox1.Text = txt
    End Sub

    Public Sub SetText(lines As IEnumerable(Of String))
        TextBox1.Text = lines.JoinBy(vbCrLf)
    End Sub

End Class