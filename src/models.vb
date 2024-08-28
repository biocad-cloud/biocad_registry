Imports biocad_registry.biocad_registryModel
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Interop
Imports SMRUCC.Rsharp.Runtime.Vectorization

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
    Public Function vocabulary_id(biocad_registry As biocad_registry, term As String, category As String, Optional desc As String = "") As UInteger
        Return biocad_registry.getVocabulary(term, category, desc)
    End Function

    <ExportAPI("subcellular_location")>
    Public Function subcellular_location(biocad_registry As biocad_registry, name As String, topology As String) As UInteger
        Return biocad_registry.getSubcellularLocation(name, topology)
    End Function

    <ExportAPI("enzyme_function")>
    Public Function find_function(biocad_registry As biocad_registry,
                                  enzyme_id As String,
                                  ec_number As String,
                                  <RRawVectorArgument>
                                  metabolites As Object,
                                  Optional env As Environment = Nothing) As Object

        Dim funcs = biocad_registry _
            .FindFunction(enzyme_id, ec_number, CLRVector.asCharacter(metabolites)) _
            .ToArray
        Dim d As New dataframe With {
            .columns = New Dictionary(Of String, Array),
            .rownames = funcs _
                .Select(Function(a) a.id.ToString) _
                .ToArray
        }

        Call d.add("molecule_id", funcs.Select(Function(f) f.molecule_id))
        Call d.add("regulation_term", funcs.Select(Function(f) f.regulation_term))
        Call d.add("add_time", funcs.Select(Function(f) f.add_time))
        Call d.add("note", funcs.Select(Function(f) f.note))

        Return d
    End Function

    <ExportAPI("enzyme")>
    Public Function enzyme_query(biocad_registry As biocad_registry, ec_number As String) As Object
        Dim q = biocad_registry.regulation_graph _
            .left_join("reaction") _
            .on(field("`reaction`.id") = field("reaction_id")) _
            .where(ReactionGraph.buildEcNumberQuery(ec_number)) _
            .select(Of EnzymeReaction)("`reaction`.id", "term", "db_xref", "name")
        Dim tbl As New dataframe With {
            .columns = New Dictionary(Of String, Array),
            .rownames = q _
                .Select(Function(r) r.id.ToString) _
                .ToArray
        }

        Call tbl.add("term", q.Select(Function(i) i.term))
        Call tbl.add("db_xrefs", q.Select(Function(i) i.db_xref))
        Call tbl.add("name", q.Select(Function(i) i.name))

        Return tbl
    End Function
End Module

Public Class EnzymeReaction
    <DatabaseField> Public Property id As UInteger
    <DatabaseField> Public Property term As String
    <DatabaseField> Public Property db_xref As String
    <DatabaseField> Public Property name As String
End Class
