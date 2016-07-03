Imports Microsoft.VisualBasic.Serialization.JSON

Public Class Configs

    ''' <summary>
    ''' GCModeller/bin
    ''' </summary>
    ''' <returns></returns>
    Public Property bin As String

    Public Shared ReadOnly Property DefaultFile As String =
        App.HOME & "/GCModeller.webApp.json"

    Public Shared Function Load() As Configs
        Try
            Dim json As String =
                FileIO.FileSystem.ReadAllText(DefaultFile)
            Return json.LoadObject(Of Configs)
        Catch ex As Exception
            Dim config As New Configs With {
                .bin = App.HOME & "/GCModeller/"
            }
            Call config.GetJson.SaveTo(DefaultFile)
            Return config
        End Try
    End Function
End Class
