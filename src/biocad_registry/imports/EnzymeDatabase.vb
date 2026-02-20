Imports System.Runtime.CompilerServices
Imports registry_data
Imports SMRUCC.genomics.Data.Rhea

Module EnzymeDatabase

    <Extension>
    Public Function MakeImports(registry As biocad_registry, enzymes As IEnumerable(Of BrendaEnzymeData)) As Boolean

        Return True
    End Function
End Module
