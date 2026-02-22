Imports Microsoft.VisualBasic.Serialization.JSON
Imports Ollama

Public Class TranslatedName

    Public Property zh_name As String

    Public Overrides Function ToString() As String
        Return zh_name
    End Function

    Public Shared Function DecodeLLMTranslateOutput(msg As DeepSeekResponse) As String
        If Not msg Is Nothing Then
            Try
                Dim json As String = msg.output.Match("[{].+[}]")
                Dim zh_name = json.LoadJSON(Of TranslatedName)

                If zh_name Is Nothing Then
                    Return Nothing
                Else
                    Return zh_name.zh_name
                End If
            Catch ex As Exception
                Return Nothing
            End Try
        Else
            Return Nothing
        End If
    End Function

End Class