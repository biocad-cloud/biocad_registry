Imports biocad_storage
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports rdataframe = SMRUCC.Rsharp.Runtime.Internal.[Object].dataframe

<Package("data_exports")>
Module exports_api

    <ExportAPI("export_fingerprints")>
    Public Function export_fingerprint(registry As biocad_registry) As Object
        Dim mat As NamedCollection(Of Double)() = Embedding.ExportGenomicsFingerprint(registry).ToArray
        Dim df As New rdataframe With {
            .columns = New Dictionary(Of String, Array),
            .rownames = mat _
                .Select(Function(i) i.name) _
                .ToArray
        }
        Dim len As Integer = mat(0).Length
        Dim offset As Integer

        For i As Integer = 0 To len - 1
            offset = i
            df.add($"v{i + 1}", From nt As NamedCollection(Of Double)
                                In mat
                                Select nt(offset))
        Next

        df.setAttribute("names", mat.Select(Function(i) i.description).ToArray)

        Return df
    End Function

End Module
