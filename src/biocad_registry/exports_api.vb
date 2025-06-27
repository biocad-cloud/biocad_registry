Imports biocad_storage
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime.Internal.Object
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

    <ExportAPI("export_metabolites")>
    Public Function export_metabolites(registry As biocad_registry, Optional page_size As Integer = 10000) As Object
        Dim mapping As New Dictionary(Of String, String)
        Dim list As list = list.empty
        Dim export As New ExportMetabolites(registry)

        For Each meta As MetaInfo In export.ExportAll(page_size, mapping)
            Call list.add(meta.ID, meta)
        Next

        ' mapping the spectrum reference id to the metabolite reference id
        Call list.setAttribute("mapping", New list(mapping))

        Return list
    End Function
End Module
