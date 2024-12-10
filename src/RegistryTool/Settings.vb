Imports Microsoft.VisualBasic.Serialization.JSON

Public Class Settings

    Public Property dbname As String
    Public Property host As String
    Public Property password As String
    Public Property port As UInteger
    Public Property user As String

    Public Shared Function GetDefault() As Settings
        Return New Settings With {
            .dbname = "cad_registry",
            .host = "127.0.0.1",
            .password = 123456,
            .port = 3306,
            .user = "root"
        }
    End Function

    Public Sub Save()
        Call Me.GetJson.SaveTo(defaultConfig)
    End Sub

    Shared ReadOnly defaultConfig As String = App.ProductProgramData & "/settings.json"

    Public Shared Function Load() As Settings
        Dim config As Settings = defaultConfig.LoadJsonFile(Of Settings)(throwEx:=False)

        If config Is Nothing Then
            config = GetDefault()
        End If

        Return config
    End Function

End Class
