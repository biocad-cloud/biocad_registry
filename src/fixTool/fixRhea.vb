Imports Microsoft.VisualBasic.Language.UnixBash
Imports Microsoft.VisualBasic.Linq
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data
Imports registry_data.biocad_registryModel
Imports SMRUCC.genomics.ComponentModel.EquaionModel.DefaultTypes
Imports SMRUCC.genomics.MetabolicModel
Imports biopax = SMRUCC.genomics.Model.Biopax.Level3.File

Module fixRhea

    Sub Main2()
        Dim rhea_key As UInteger = registry.biocad_vocabulary.GetDatabaseResource("Rhea")
        Dim role_left As UInteger = registry.MetabolicSubstrateRole
        Dim role_right As UInteger = registry.MetabolicProductRole

        For Each filepath As String In ls - l - r - "*.owl" <= "D:\datapool\reactions\chunks"
            Dim xml As biopax = biopax.LoadDoc(filepath)
            Dim loader As SMRUCC.genomics.Model.Biopax.Level3.ResourceReader = SMRUCC.genomics.Model.Biopax.Level3.ResourceReader.LoadResource(file:=xml)
            Dim reactions = loader.GetAllReactions(entity_refs:=True).ToArray
            Dim compounds = loader.GetAllCompounds.ToDictionary(Function(a) a.id)
            Dim trans As CommitTransaction = registry.metabolic_network.ignore.open_transaction

            For Each rxn As MetabolicReaction In reactions
                Dim db_xref As String = "RHEA:" & rxn.id
                Dim check = registry.reaction.where(field("db_xref") = db_xref, field("db_source") = rhea_key).find(Of reaction)

                If check Is Nothing Then
                    ' add new 
                    registry.reaction.add(field("db_xref") = db_xref,
                                          field("db_source") = rhea_key,
                                          field("hashcode") = "-",
                                          field("main_id") = 0,
                                          field("name") = rxn.name,
                                          field("ec_number") = rxn.ECNumbers.DefaultFirst,
                                          field("equation") = rxn.ToString,
                                          field("note") = rxn.description)

                    check = registry.reaction _
                        .where(field("db_xref") = db_xref, field("db_source") = rhea_key) _
                        .find(Of reaction)

                    For Each spc As CompoundSpecieReference In rxn.left
                        Dim symbol As MetabolicCompound = compounds(spc.ID)
                        Dim chebi_id As String = symbol("CHEBI")

                        Call trans.add(registry.metabolic_network.add_sql(field("reaction_id") = check.id,
                                                                          field("factor") = spc.Stoichiometry,
                                                                          field("species_id") = 0,
                                                                          field("symbol_id") = chebi_id,
                                                                          field("role") = role_left,
                                                                          field("compartment_id") = 1))
                    Next
                    For Each spc As CompoundSpecieReference In rxn.right
                        Dim symbol As MetabolicCompound = compounds(spc.ID)
                        Dim chebi_id As String = symbol("CHEBI")

                        Call trans.add(registry.metabolic_network.add_sql(field("reaction_id") = check.id,
                                                                          field("factor") = spc.Stoichiometry,
                                                                          field("species_id") = 0,
                                                                          field("symbol_id") = chebi_id,
                                                                          field("role") = role_right,
                                                                          field("compartment_id") = 1))
                    Next
                Else
                    ' update role reference
                    For Each spc As CompoundSpecieReference In rxn.left
                        Dim symbol As MetabolicCompound = compounds(spc.ID)
                        Dim chebi_id As String = symbol("CHEBI")

                        Call trans.add(registry.metabolic_network.where(field("reaction_id") = check.id, field("symbol_id") = chebi_id).save_sql(field("role") = role_left))
                    Next
                    For Each spc As CompoundSpecieReference In rxn.right
                        Dim symbol As MetabolicCompound = compounds(spc.ID)
                        Dim chebi_id As String = symbol("CHEBI")

                        Call trans.add(registry.metabolic_network.where(field("reaction_id") = check.id, field("symbol_id") = chebi_id).save_sql(field("role") = role_right))
                    Next
                End If
            Next

            Call trans.commit()
        Next
    End Sub
End Module
