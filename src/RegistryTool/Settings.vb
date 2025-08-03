Imports Microsoft.VisualBasic.Serialization.JSON

Public Class Settings

    Public Property dbname As String
    Public Property host As String
    Public Property password As String
    Public Property port As UInteger
    Public Property user As String

    Public Property model As String = "qwen3:30b"
    Public Property ollama_server As String = "127.0.0.1"
    Public Property ollama_service As Integer = 11434

    Public Shared Function GetDefault() As Settings
        Return New Settings With {
            .dbname = "cad_registry",
            .host = "127.0.0.1",
            .password = 123456,
            .port = 3306,
            .user = "root",
            .model = "qwen3:30b",
            .ollama_server = "127.0.0.1",
            .ollama_service = 11434
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
