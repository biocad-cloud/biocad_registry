Imports System.Runtime.CompilerServices
Imports SMRUCC.genomics.Assembly.Uniprot.XML

Public Module ImportsUniProt

    <Extension>
    Public Sub importsUniProt(registry As biocad_registry, proteins As IEnumerable(Of entry))
        Dim db_uniprot As UInteger = registry.biocad_vocabulary.db_uniprot

        For Each block As entry() In proteins.SplitIterator(5000)

        Next
    End Sub

End Module
