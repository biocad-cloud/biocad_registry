Imports BioNovoGene.BioDeep.Chemistry.NCBI.PubChem

Public Class PubChemScanner

    ReadOnly repo As String

    Sub New(repo As String)
        Me.repo = repo
    End Sub

    Public Function LoadData() As IEnumerable(Of PugViewRecord)
        Return LoadData(repo.ListFiles("*.xml"))
    End Function

    Public Iterator Function LoadPageData() As IEnumerable(Of PugViewRecord())
        For Each page As String() In repo.ListFiles("*.xml").SplitIterator(3000)
            Yield LoadData(page).ToArray
        Next
    End Function

    Public Shared Iterator Function LoadData(pagefiles As IEnumerable(Of String)) As IEnumerable(Of PugViewRecord)
        For Each file As String In pagefiles
            Try
                Yield file.LoadXml(Of PugViewRecord)
            Catch ex As Exception
                Call App.LogException(ex)
            End Try
        Next
    End Function
End Class
