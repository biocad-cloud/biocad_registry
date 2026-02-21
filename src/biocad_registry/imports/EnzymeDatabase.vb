Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Scripting.Runtime
Imports registry_data
Imports SMRUCC.genomics.Data.Rhea

Module EnzymeDatabase

    <Extension>
    Public Function MakeImports(registry As biocad_registry, enzymes As IEnumerable(Of BrendaEnzymeData)) As Boolean
        For Each enzyme As BrendaEnzymeData In enzymes
            Dim ec_number As String = enzyme.id

            If ec_number = "spontaneous" Then
                ec_number = "0.0.0.0"
            End If

            Dim digits As Integer() = ec_number.Split("."c).AsInteger
            Dim first As Integer = digits.ElementAtOrDefault(0, 0)
            Dim second As Integer = digits.ElementAtOrDefault(1, 0)
            Dim third As Integer = digits.ElementAtOrDefault(2, 0)
            Dim fourth As Integer = digits.ElementAtOrDefault(3, 0)
            Dim check As  

        Next

        Return True
    End Function
End Module
