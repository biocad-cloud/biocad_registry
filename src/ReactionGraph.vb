Imports System.Runtime.CompilerServices
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Module ReactionGraph

    <Extension>
    Public Iterator Function FindReaction(biocad_registry As biocad_registry, metabolites As IEnumerable(Of String)) As IEnumerable(Of UInteger)
        Dim speciesSet As String() = metabolites.ToArray
        Dim pull = biocad_registry.reaction_graph _
            .where(field("db_xref").in(speciesSet)) _
            .select(Of biocad_registryModel.reaction_graph)

        For Each rxn In pull.GroupBy(Function(r) r.reaction)
            If speciesSet.Length = rxn.Count Then
                Yield rxn.Key
            End If
        Next
    End Function

    <Extension>
    Public Iterator Function FindReaction(biocad_registry As biocad_registry,
                                          ec_number As String,
                                          metabolites As IEnumerable(Of String)) As IEnumerable(Of biocad_registryModel.regulation_graph)

        For Each reaction_id As UInteger In biocad_registry.FindReaction(metabolites)
            Dim check = biocad_registry.regulation_graph _
                .where(field("reaction_id") = reaction_id,
                       field("term") = ec_number) _
                .find(Of biocad_registryModel.regulation_graph)

            If check IsNot Nothing Then
                Yield check
            End If
        Next
    End Function

    ''' <summary>
    ''' Find the molecule function that associated with the given enzymatic reaction context information.
    ''' </summary>
    ''' <param name="biocad_registry"></param>
    ''' <param name="enzyme_id">the external database source id of the enzyme molecule, 
    ''' usually be the uniprot protein id.</param>
    ''' <param name="ec_number">the ec number of the enzyme and the reaction</param>
    ''' <param name="metabolites">a collection of the external database metabolite id</param>
    ''' <returns></returns>
    <Extension>
    Public Function FindFunction(biocad_registry As biocad_registry,
                                 enzyme_id As String,
                                 ec_number As String,
                                 metabolites As IEnumerable(Of String)) As IEnumerable(Of biocad_registryModel.molecule_function)

        Dim molecule_terms As UInteger() = biocad_registry.vocabulary _
            .where(field("category") = "Molecule Type") _
            .project(Of UInteger)("id")
        Dim enzyme_molecule = biocad_registry.db_xrefs _
            .where(field("type").in(molecule_terms),
                   field("xref") = enzyme_id) _
            .project(Of UInteger)("obj_id")
        Dim regulations = biocad_registry _
            .FindReaction(ec_number, metabolites) _
            .Select(Function(r) r.id) _
            .ToArray

        If regulations.IsNullOrEmpty Then
            Return New biocad_registryModel.molecule_function() {}
        End If

        Return biocad_registry.molecule_function _
            .where(field("molecule_id").in(enzyme_molecule),
                   field("regulation_term").in(regulations)) _
            .select(Of biocad_registryModel.molecule_function)
    End Function
End Module
