Imports System.ComponentModel
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace Configs

    Public Class Settings

        <Category("MySQL")> Public Property dbname As String
        <Category("MySQL")> Public Property host As String
        <Category("MySQL")> Public Property password As String
        <Category("MySQL")> Public Property port As UInteger
        <Category("MySQL")> Public Property user As String

        <Category("LLMs")> Public Property model As String = "qwen3:30b"
        <Category("LLMs")> Public Property ollama_server As String = "127.0.0.1"
        <Category("LLMs")> Public Property ollama_service As Integer = 11434

        Public Property website As String = "http://biocad.innovation.ac.cn"

        <Browsable(False)>
        Public Property molecule_history As MoleculeEditHistory()

        Public Shared Function GetDefault() As Settings
            Return New Settings With {
                .dbname = "cad_registry",
                .host = "127.0.0.1",
                .password = 123456,
                .port = 3306,
                .user = "root",
                .model = "qwen3:30b",
                .ollama_server = "127.0.0.1",
                .ollama_service = 11434,
                .website = "http://biocad.innovation.ac.cn"
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

    Public Class MoleculeEditHistory

        Public Property id As String
        Public Property name As String

        Public Overrides Function ToString() As String
            Return $"{id} - {name}"
        End Function

    End Class
End Namespace