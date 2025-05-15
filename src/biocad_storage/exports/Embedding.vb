Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel

Public Module Embedding

    ''' <summary>
    ''' export the genomics sequence fingerprint matrix
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <returns>
    ''' only exports of the complete genomics sequence data
    ''' </returns>
    <Extension>
    Public Iterator Function ExportGenomicsFingerprint(registry As biocad_registry) As IEnumerable(Of NamedCollection(Of Double))
        Dim page_size As Integer = 100
        Dim offset As UInteger
        Dim page_data As biocad_registryModel.genomics()

    End Function

End Module
