Imports System.IO
Imports Microsoft.VisualBasic.Net.Http
Imports SMRUCC.genomics.Assembly.NCBI.GenBank

Public Class GenBankScanner

    ReadOnly repo As String

    Sub New(repo As String)
        Me.repo = repo
    End Sub

    Public Function LoadData() As IEnumerable(Of GBFF.File)
        Return LoadData(pagefiles:=repo.ListFiles("*.gz", "*.gb", "*.txt"))
    End Function

    Public Iterator Function LoadPageData() As IEnumerable(Of GBFF.File())
        For Each page As String() In repo.ListFiles("*.gz", "*.gb", "*.txt").SplitIterator(20)
            Yield LoadData(pagefiles:=page).ToArray
        Next
    End Function

    Public Shared Iterator Function LoadData(pagefiles As IEnumerable(Of String)) As IEnumerable(Of GBFF.File)
        For Each file As String In pagefiles
            Dim s As Stream = file.Open(FileMode.Open, doClear:=False, [readOnly]:=True)

            If file.ExtensionSuffix("gz") Then
                Try
                    s = s.UnGzipStream
                Catch ex As Exception
                    Call App.LogException(ex, file)
                    Continue For
                End Try
            End If

            For Each genome As GBFF.File In GBFF.File.LoadDatabase(s, defaultAccession:=file.BaseName, suppressError:=True)
                If Not genome Is Nothing Then
                    Yield genome
                End If
            Next

            Call s.Dispose()
        Next
    End Function
End Class
