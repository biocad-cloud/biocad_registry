Imports biocad_registry.biocad_registryModel
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime.Interop

''' <summary>
''' exports the biocad registry database models
''' </summary>
<Package("models")>
<RTypeExport("reaction_node", GetType(reaction))>
<RTypeExport("reaction_graph", GetType(reaction_graph))>
<RTypeExport("molecules", GetType(molecule))>
<RTypeExport("dblinks", GetType(db_xrefs))>
<RTypeExport("subcellular_compartments", GetType(subcellular_compartments))>
<RTypeExport("subcellular_locations", GetType(subcellular_location))>
<RTypeExport("vocabulary", GetType(vocabulary))>
<RTypeExport("pathway", GetType(pathway))>
<RTypeExport("complex", GetType(complex))>
<RTypeExport("biocad_registry", GetType(biocad_registry))>
<RTypeExport("cad_registry", GetType(biocad_registry))>
Module models

    Public Sub Main()

    End Sub

    ''' <summary>
    ''' get the vocabulary term id
    ''' </summary>
    ''' <param name="biocad_registry"></param>
    ''' <param name="term"></param>
    ''' <param name="category"></param>
    ''' <returns></returns>
    <ExportAPI("vocabulary_id")>
    Public Function vocabulary_id(biocad_registry As biocad_registry, term As String, category As String) As UInteger
        Return biocad_registry.getVocabulary(term, category)
    End Function

    <ExportAPI("subcellular_location")>
    Public Function subcellular_location(biocad_registry As biocad_registry, name As String, topology As String) As UInteger
        Return biocad_registry.getSubcellularLocation(name, topology)
    End Function
End Module
