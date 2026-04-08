Imports registry_data

Module Module1

    ReadOnly names_test As XElement =
        <names>

        </names>

    Sub cleanNameTest()
        For Each name As String In names_test.ToString.LineTokens.Select(AddressOf Strings.Trim)
            Call Console.WriteLine($"{name}{vbTab}{vbTab}{RegisterSymbol.CleanName(name)}")
        Next
    End Sub
End Module
