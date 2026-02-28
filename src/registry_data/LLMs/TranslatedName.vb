Imports Microsoft.VisualBasic.Serialization.JSON
Imports Ollama

Public Class TranslatedName

    Public Property zh_name As String

    Public Overrides Function ToString() As String
        Return zh_name
    End Function

    Public Shared Function DecodeLLMTranslateOutput(llms_output As String) As String
        If Not llms_output Is Nothing Then
            Try
                Dim json As String = llms_output.Match("[{].+[}]")
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