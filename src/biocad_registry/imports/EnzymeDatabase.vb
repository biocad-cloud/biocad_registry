Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Scripting.Runtime
Imports registry_data
Imports SMRUCC.genomics.Data.Rhea
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Module EnzymeDatabase

    <Extension>
    Public Function MakeImports(registry As biocad_registry, enzymes As IEnumerable(Of BrendaEnzymeData)) As Boolean
        Dim db_enzyme As UInteger = registry.biocad_vocabulary.db_ECNumber

        For Each enzyme As BrendaEnzymeData In enzymes
            Dim ec_number As String = enzyme.id

            If ec_number = "spontaneous" Then
                ec_number = "0.0.0.0"
            End If

            Dim check_number As Boolean = registry.db_xrefs _
                .where(field("type") = db_enzyme,
                       field("db_name") = db_enzyme,
                       field("db_xref") = ec_number,
                       field("db_source") = db_enzyme) _
                .find(Of biocad_registryModel.db_xrefs) IsNot Nothing

            If check_number Then
                Continue For
            End If

            Dim digits As Integer() = ec_number.Split("."c).AsInteger
            Dim first As Integer = digits.ElementAtOrDefault(0, 0)
            Dim second As Integer = digits.ElementAtOrDefault(1, 0)
            Dim third As Integer = digits.ElementAtOrDefault(2, 0)
            Dim fourth As Integer = digits.ElementAtOrDefault(3, 0)
            Dim check As biocad_registryModel.enzyme = registry.enzyme _
                .where(field("enzyme_class") = first,
                       field("sub_class") = second,
                       field("sub_category") = third,
                       field("enzyme_number") = fourth) _
                .find(Of biocad_registryModel.enzyme)

            If check Is Nothing Then

            End If
        Next

        Return True
    End Function
End Module
