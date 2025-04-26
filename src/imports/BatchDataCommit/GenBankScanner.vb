Imports System.IO
Imports Microsoft.VisualBasic.Net.Http
Imports SMRUCC.genomics.Assembly.NCBI.GenBank

Public Class GenBankScanner

    ReadOnly repo As String

    Sub New(repo As String)
        Me.repo = repo
    End Sub

    Public Iterator Function LoadData() As IEnumerable(Of GBFF.File)
        For Each file As String In repo.ListFiles("*.gz", "*.gb", "*.txt")
            Dim s As Stream = file.Open(FileMode.Open, doClear:=False, [readOnly]:=True)

            If file.ExtensionSuffix("gz") Then
                Try
                    s = s.UnGzipStream
                Catch ex As Exception
                    Call App.LogException(ex, file)
                    Continue For
                End Try
            End If

            For Each genome As GBFF.File In GBFF.File.LoadDatabase(s)
                Yield genome
            Next
        Next
    End Function
End Class
