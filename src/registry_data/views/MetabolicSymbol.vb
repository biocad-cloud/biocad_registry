Imports System.Runtime.CompilerServices
Imports registry_data.biocad_registryModel
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Module MetabolicSymbol

    <Extension>
    Public Sub RegisterMetabolicSymbols(registry As biocad_registry)
        Dim page_size As Integer = 3000
        Dim role = (registry.MetabolicSubstrateRole.id, registry.MetabolicProductRole.id)

        For page As Integer = 1 To Integer.MaxValue
            Dim offset As UInteger = (page - 1) * page_size
            Dim page_data As reaction() = registry.reaction _
                .limit(offset, page_size) _
                .select(Of reaction)

            If page_data.IsNullOrEmpty Then
                Exit For
            Else
                Call $"process metabolic symbol data page {page}".info
            End If

            Dim set_hash = registry.reaction.open_transaction

            For Each reaction As reaction In page_data
                ' re-calculate the reaction hashcode
                Dim hashcode As String = registry.RegisterMetabolicSymbols(reaction, role).CalculateReactionHashCode

                If hashcode <> reaction.hashcode Then
                    Call set_hash.add(registry.reaction.where(field("id") = reaction.id).save_sql(field("hashcode") = hashcode))
                End If
            Next

            Call set_hash.commit()
        Next
    End Sub

    <Extension>
    Private Function RegisterMetabolicSymbols(registry As biocad_registry, reaction As reaction, role As (left As UInteger, right As UInteger)) As List(Of (UInteger, UInteger))
        Dim metabolite_type As UInteger = registry.biocad_vocabulary.metabolite_type
        Dim metabolites As metabolic_network() = registry.metabolic_network _
            .where(field("reaction_id") = reaction.id,
                   field("role").in({role.left, role.right})) _
            .select(Of metabolic_network)
        Dim links As New List(Of (UInteger, UInteger))

        For Each compound As metabolic_network In metabolites
            Dim metab As metabolites = registry.metabolites _
                .where(field("id") = compound.species_id) _
                .find(Of metabolites)

            If metab IsNot Nothing Then
                Call links.Add((compound.role, metab.id))
            Else
                Call links.Add((compound.role, 0))
            End If

            ' current no symbol mapping, required of the re-mapping and
            ' then try to run this function again
            If metab Is Nothing Then
                Continue For
            Else
                If registry.SymbolRegister(meta:=metab) Is Nothing Then
                    Call $"make register of '{metab.name}' error!".warning
                End If
            End If
        Next

        Return links
    End Function

End Module
