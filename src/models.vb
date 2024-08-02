Imports biocad_registry.biocad_registryModel
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
Module models

    Public Sub Main()

    End Sub
End Module
