Imports System.Text.RegularExpressions

Public Module Common

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="memeText">The text file content, not the path</param>
    ''' <returns></returns>
    Public Function getMEMEQueryName(memeText As String) As String
        Dim file As String = Regex.Match(memeText, "DATAFILE=.+?ALPHABET=", RegexOptions.IgnoreCase Or RegexOptions.Singleline).Value
        file = Mid(file, 10).Trim
        file = Mid(file, 1, Len(file) - 9).Trim
        file = IO.Path.GetFileNameWithoutExtension(file)
        Return file
    End Function
End Module
