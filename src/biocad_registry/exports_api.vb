Imports biocad_storage
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.Models
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.Scripting.Runtime
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop
Imports SMRUCC.Rsharp.Runtime.Vectorization
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

    ''' <summary>
    ''' Export the metabolite information from the biocad registry database
    ''' 
    ''' This function will export all the metabolite information including the
    ''' metabolite ID, name, formula, exact mass, and other related information.
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="page_size"></param>
    ''' <returns>
    ''' a tuple list of the <see cref="MetaInfo"/> clr object
    ''' </returns>
    <ExportAPI("export_metabolites")>
    <RApiReturn(TypeCodes.list)>
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

    ''' <summary>
    ''' export struct data for run cfm-id workflow
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="tag_name"></param>
    ''' <returns></returns>
    <ExportAPI("export_topic_structdata")>
    <RApiReturn(GetType(MetaboliteStructData))>
    Public Function export_topic_structdata(registry As biocad_registry, tag_name As String) As Object
        Return registry.ExportTopicMetabolites(tag_name).IteratesALL.ToArray
    End Function

    ''' <summary>
    ''' export metabolite by pubchem cid query
    ''' </summary>
    ''' <param name="registry">a mysql connection ot the biocad registry database</param>
    ''' <param name="cid">a vector of the pubchem cids</param>
    ''' <returns></returns>
    <ExportAPI("export_by_cids")>
    <RApiReturn(GetType(MetaInfo))>
    Public Function exportByCIDs(registry As biocad_registry,
                                 <RRawVectorArgument>
                                 cid As Object,
                                 Optional wrap_tqdm As Boolean = True) As Object

        Dim cidMaps As String() = registry.db_xrefs _
            .where(field("db_key") = registry.vocabulary_terms.pubchem_term,
                   field("xref").in(CLRVector.asLong(cid))) _
            .distinct _
            .project(Of UInteger)("obj_id") _
            .AsCharacter _
            .ToArray
        Dim exports As MetaInfo() = New ExportMetabolites(registry) _
            .ExportByID(cidMaps, wrap_tqdm:=wrap_tqdm) _
            .ToArray

        Return exports
    End Function
End Module
