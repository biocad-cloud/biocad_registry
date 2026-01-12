Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Linq
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports SMRUCC.genomics.ComponentModel.EquaionModel

Public Module ImportsReaction

    Private Function CCNameMapping(cc As String) As String
        Select Case UCase(cc)
            Case "", "CCO-CYTOSOL", "NIL", "CCO-IN" : Return "Cytoplasm"
            Case "CCO-OUT" : Return "Extracellular"
            Case Else
                Throw New NotImplementedException(cc)
        End Select
    End Function

    <Extension>
    Public Sub importsReactions(registry As biocad_registry, reactions As IEnumerable(Of Reaction), db_name As String)
        Dim db_source As UInteger = registry.biocad_vocabulary.GetDatabaseResource(db_name).id
        Dim ec_num As UInteger = registry.biocad_vocabulary.db_ECNumber
        Dim dblinks As CommitTransaction = registry.db_xrefs.ignore.open_transaction
        Dim entityType As UInteger = registry.biocad_vocabulary.reaction_type
        Dim role_substrate As UInteger = registry.biocad_vocabulary.GetVocabulary("Metabolic Role", "Substrate").id
        Dim role_product As UInteger = registry.biocad_vocabulary.GetVocabulary("Metabolic Role", "Product").id
        Dim network As CommitTransaction = registry.metabolic_network.ignore.open_transaction
        Dim metabolite_type As UInteger = registry.biocad_vocabulary.metabolite_type
        Dim dbList As UInteger() = {registry.biocad_vocabulary.db_kegg, registry.biocad_vocabulary.db_chebi, registry.biocad_vocabulary.db_biocyc}
        Dim compartmentIndex As New Dictionary(Of String, biocad_registryModel.compartment_location)
        Dim pool As Reaction() = reactions.ToArray
        Dim enrich = registry.compartment_enrich.ignore.open_transaction

        For Each rxn As Reaction In TqdmWrapper.Wrap(pool)
            For Each cpd In rxn.equation.Reactants
                Call compartmentIndex _
                    .ComputeIfAbsent(If(cpd.Compartment, ""),
                                     Function(cc)
                                         cc = CCNameMapping(cc)
                                         Dim cc_obj = registry.compartment_location.where(field("name") = cc).find(Of biocad_registryModel.compartment_location)
                                         If cc_obj Is Nothing Then
                                             registry.compartment_location.add(
                                                field("name") = cc,
                                                field("fullname") = cc
                                             )
                                             cc_obj = registry.compartment_location.where(field("name") = cc).find(Of biocad_registryModel.compartment_location)
                                         End If

                                         Return cc_obj
                                     End Function)
            Next
            For Each cpd In rxn.equation.Products
                Call compartmentIndex _
                    .ComputeIfAbsent(If(cpd.Compartment, ""),
                                     Function(cc)
                                         cc = CCNameMapping(cc)
                                         Dim cc_obj = registry.compartment_location.where(field("name") = cc).find(Of biocad_registryModel.compartment_location)
                                         If cc_obj Is Nothing Then
                                             registry.compartment_location.add(
                                                field("name") = cc,
                                                field("fullname") = cc
                                             )
                                             cc_obj = registry.compartment_location.where(field("name") = cc).find(Of biocad_registryModel.compartment_location)
                                         End If

                                         Return cc_obj
                                     End Function)
            Next
        Next

        For Each rxn As Reaction In TqdmWrapper.Wrap(pool)
            Dim find As biocad_registryModel.reaction = registry.reaction _
                .where(field("db_source") = db_source,
                       field("db_xref") = rxn.entry) _
                .find(Of biocad_registryModel.reaction)

            If find Is Nothing Then
                Call registry.reaction.add(
                    field("db_source") = db_source,
                    field("db_xref") = rxn.entry,
                    field("hashcode") = "",
                    field("name") = rxn.definition,
                    field("ec_number") = rxn.enzyme.DefaultFirst,
                    field("equation") = rxn.equation.ToString,
                    field("note") = rxn.comment
                )
                find = registry.reaction _
                    .where(field("db_source") = db_source,
                           field("db_xref") = rxn.entry) _
                    .find(Of biocad_registryModel.reaction)
            End If

            For Each num As String In rxn.enzyme
                Call dblinks.ignore.add(
                    field("obj_id") = find.id,
                    field("type") = entityType,
                    field("db_name") = ec_num,
                    field("db_xref") = num,
                    field("db_source") = db_source
                )
            Next

            Call dblinks.ignore.add(
                field("obj_id") = find.id,
                field("type") = entityType,
                field("db_name") = db_source,
                field("db_xref") = rxn.entry,
                field("db_source") = db_source
            )

            Dim eq = rxn.equation
            Dim links As New List(Of (UInteger, UInteger))

            For Each sp As SideCompound In rxn.compounds
                Dim role As UInteger = If(sp.side = "left", role_substrate, role_product)
                Dim factor = If(role = role_substrate, eq.Reactants, eq.Products).KeyItem(sp.compound.entry)
                Dim metab = registry.db_xrefs _
                    .where(field("type") = metabolite_type,
                           field("db_name").in(dbList),
                           field("db_xref") = sp.compound.entry) _
                    .order_by("obj_id") _
                    .find(Of biocad_registryModel.db_xrefs)
                Dim loc As biocad_registryModel.compartment_location = compartmentIndex _
                    .ComputeIfAbsent(If(factor.Compartment, ""),
                                     Function(cc)
                                         cc = CCNameMapping(cc)
                                         Return registry.compartment_location.where(field("name") = cc).find(Of biocad_registryModel.compartment_location)
                                     End Function)

                Call network.ignore.add(
                    field("reaction_id") = find.id,
                    field("factor") = factor.Stoichiometry,
                    field("species_id") = If(metab Is Nothing, 0, metab.obj_id),
                    field("symbol_id") = sp.compound.entry,
                    field("role") = role,
                    field("compartment_id") = loc.id,
                    field("note") = sp.ToString
                )

                If metab IsNot Nothing Then
                    Call links.Add((role, metab.obj_id))
                Else
                    Call links.Add((role, 0))
                End If

                If metab IsNot Nothing AndAlso compartmentIndex.Count > 0 AndAlso compartmentIndex.Keys.First <> "" Then
                    Call enrich.ignore.add(
                        field("metabolite_id") = metab.obj_id,
                        field("location_id") = loc.id,
                        field("evidence") = rxn.entry
                    )
                End If
            Next

            If links.All(Function(a) a.Item2 > 0) Then
                Dim hashcode As String = links.OrderBy(Function(a) a.Item1).ThenBy(Function(a) a.Item2).Select(Function(a) {a.Item1, a.Item2}).IteratesALL.JoinBy(",").MD5

                registry.reaction.where(field("id") = find.id).save(field("hashcode") = hashcode)
            End If
        Next

        Call dblinks.commit()
        Call network.commit()
        Call enrich.commit()
    End Sub
End Module
