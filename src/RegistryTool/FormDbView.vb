Imports System.Reflection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel

Public Class FormDbView

    Public Delegate Function SourceProvider(Of T)() As T()

    Dim _sourceProvider As Func(Of DataTable)
    Dim _binding As New BindingSource

    Public Sub LoadTableView(Of T)(source As SourceProvider(Of T))
        Dim schema As PropertyInfo() = DataFramework _
            .Schema(Of T)(PropertyAccess.Readable,
                          nonIndex:=True,
                          primitive:=True).Values _
            .ToArray

        _sourceProvider =
            Function()
                Dim dt As New DataTable

                For Each field As PropertyInfo In schema
                    Call dt.Columns.Add(field.Name, field.PropertyType)
                Next

                For Each row As T In source()
                    Dim rowData As Object() = schema _
                        .Select(Function(col) col.GetValue(row)) _
                        .ToArray

                    Call dt.Rows.Add(rowData)
                Next

                Return dt
            End Function

        Call LoadTable()
    End Sub

    Private Sub LoadTable()
        If _sourceProvider Is Nothing Then
            Return
        End If

        _binding.DataSource = _sourceProvider()
        DataGridView1.DataSource = _binding
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Call LoadTable()
    End Sub
End Class