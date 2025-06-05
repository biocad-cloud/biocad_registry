Imports System.Reflection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel

Public Class FormDbView

    Public Delegate Function SourceProvider(Of T)() As T()

    Dim _sourceProvider As Func(Of DataTable)
    Dim _binding As New BindingSource

    Dim filterField As String = Nothing

    Public Sub DisableFilter()
        ToolStripComboBox1.Visible = False
        ToolStripLabel1.Visible = False
    End Sub

    Public Sub SetFilter(name As String, fieldname As String)
        ToolStripLabel1.Text = $"Filter [{name}]"
        filterField = fieldname
    End Sub

    Public Sub LoadTableView(Of T)(source As SourceProvider(Of T))
        Dim schema As PropertyInfo() = DataFramework _
            .Schema(Of T)(PropertyAccess.Readable,
                          nonIndex:=True,
                          primitive:=True).Values _
            .ToArray

        _sourceProvider =
            Function()
                Dim dt As New DataTable
                Dim catList As New List(Of String)
                Dim filter As PropertyInfo = Nothing

                For Each field As PropertyInfo In schema
                    Call dt.Columns.Add(field.Name, field.PropertyType)

                    If filterField IsNot Nothing AndAlso filterField = field.Name Then
                        filter = field
                    End If
                Next

                For Each row As T In source()
                    Dim rowData As Object() = schema _
                        .Select(Function(col) col.GetValue(row)) _
                        .ToArray

                    Call dt.Rows.Add(rowData)

                    If Not filter Is Nothing Then
                        Call catList.Add(CStr(filter.GetValue(row)))
                    End If
                Next

                For Each item As String In catList.Distinct
                    Call ToolStripComboBox1.Items.Add(item)
                Next

                Return dt
            End Function

        Call LoadTable()
    End Sub

    Private Sub LoadTable()
        If _sourceProvider Is Nothing Then
            Return
        End If

        ToolStripComboBox1.Items.Clear()
        _binding.DataSource = _sourceProvider()
        DataGridView1.DataSource = _binding
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Call LoadTable()
    End Sub
End Class