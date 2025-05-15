Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Net.Http
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

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

        For page As Integer = 1 To Integer.MaxValue
            offset = (page - 1) * page_size
            page_data = registry.genomics _
                .where(match("def").against("+complete -plasmid", booleanMode:=True), Not field("fingerprint").is_nothing) _
                .limit(offset, page_size) _
                .select(Of biocad_registryModel.genomics)

            If page_data.IsNullOrEmpty Then
                Exit For
            End If

            For Each genome As biocad_registryModel.genomics In From nt As biocad_registryModel.genomics
                                                                In page_data
                                                                Where nt.fingerprint.Length > 0

                Dim fingerprint As Byte() = genome.fingerprint.UnGzipBase64.ToArray
                Dim v As Double() = fingerprint.Select(Function(b) CDbl(b)).ToArray

                Yield New NamedCollection(Of Double)(genome.id.ToString, v, genome.biom_string)
            Next
        Next
    End Function

End Module
