Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.Linq
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports SMRUCC.genomics.ComponentModel.EquaionModel

Public Module ImportsReaction

    <Extension>
    Public Sub importsReactions(registry As biocad_registry, reactions As IEnumerable(Of Reaction), db_name As String)
        Dim db_source As UInteger = registry.biocad_vocabulary.GetDatabaseResource(db_name).id
        Dim ec_num As UInteger = registry.biocad_vocabulary.db_ECNumber
        Dim dblinks As CommitTransaction = registry.db_xrefs.ignore.open_transaction
        Dim entityType As UInteger = registry.biocad_vocabulary.reaction_type
        Dim role_substrate As UInteger = registry.biocad_vocabulary.GetVocabulary("Metabolic Role", "Substrate").id
        Dim role_product As UInteger = registry.biocad_vocabulary.GetVocabulary("Metabolic Role", "Product").id
        Dim network As CommitTransaction = registry.metabolic_network.ignore.open_transaction

        For Each rxn As Reaction In TqdmWrapper.Wrap(reactions.ToArray)
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

            For Each sp As SideCompound In rxn.compounds
                Dim role As UInteger = If(sp.side = "left", role_substrate, role_product)

                Call network.ignore.add(
                    field("reaction_id") = find.id,
                    field("factor") = 0,
                    field("species_id") = 0,
                    field("symbol_id") = sp.compound.entry,
                    field("role") = role,
                    field("compartment_id") = 0,
                    field("note") = sp.ToString
                )
            Next
        Next

        Call dblinks.commit()
        Call network.commit()
    End Sub
End Module
