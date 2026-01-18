Imports System.Runtime.CompilerServices
Imports registry_data.biocad_registryModel
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Module MetabolicSymbol

    <Extension>
    Public Sub RegisterMetabolicSymbols(registry As biocad_registry)
        Dim page_size As Integer = 1000
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

            For Each reaction As reaction In page_data
                Call registry.RegisterMetabolicSymbols(reaction, role)
            Next
        Next
    End Sub

    <Extension>
    Private Sub RegisterMetabolicSymbols(registry As biocad_registry, reaction As reaction, role As (left As UInteger, right As UInteger))
        Dim metabolite_type As UInteger = registry.biocad_vocabulary.metabolite_type
        Dim metabolites As metabolic_network() = registry.metabolic_network _
            .where(field("reaction_id") = reaction.id,
                   field("role").in({role.left, role.right})) _
            .select(Of metabolic_network)

        For Each compound As metabolic_network In metabolites
            Dim m As metabolites = registry.metabolites _
                .where(field("id") = compound.species_id) _
                .find(Of metabolites)

            ' current no symbol mapping, required of the re-mapping and
            ' then try to run this function again
            If m Is Nothing Then
                Continue For
            Else
                If registry.SymbolRegister(meta:=m) Is Nothing Then
                    Call $"make register of '{m.name}' error!".warning
                End If
            End If
        Next
    End Sub

End Module
