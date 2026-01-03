Imports System.Runtime.CompilerServices

Public Module RegisterSymbol

    <Extension>
    Public Function makeSymbol(name As String) As String
        Return Strings.Trim(name).Replace("(", "_").Replace(")", "_").Replace("""", "_").Replace("'", "_").Replace("\", "_").Replace("/", "_").StringReplace("\s", "_").StringReplace("[_-]{2,}", "_").Trim("-"c, "_"c)
    End Function
End Module
