Imports SMRUCC.genomics.Assembly.Uniprot.XML

Public Class UniProtImporter

End Class

Public Class UniProtPageLoader

    ReadOnly repo As String

    Sub New(repo As String)
        Me.repo = repo
    End Sub

    Public Iterator Function LoadPageData() As IEnumerable(Of entry())
        Dim tmp As New List(Of entry)

        For Each prot As entry In UniProtXML.EnumerateEntries({repo}, ignoreError:=True, tqdm:=True)
            Call tmp.Add(prot)

            If tmp.Count > 3000 Then
                Yield tmp.ToArray
                Call tmp.Clear()
            End If
        Next
    End Function

End Class