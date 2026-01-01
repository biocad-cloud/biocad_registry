Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports registry_data.biocad_registryModel

Public Class biocad_vocabulary

    Public Const ExternalDatabase As String = "External Database"
    Public Const EntityType As String = "Entity Type"

    Public Const EntityMetabolite As String = "Metabolite"
    Public Const EntityNucleotide As String = "Nucleotide"
    Public Const EntityProtein As String = "Protein"
    Public Const EntityReaction As String = "Reaction"
    Public Const EntitySubcellularLocation As String = "Subcellular Location"

    ReadOnly registry As biocad_registry

    Sub New(registry As biocad_registry)
        Me.registry = registry
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function GetDatabaseResource(name As String) As vocabulary
        Static cache As New Dictionary(Of String, vocabulary)
        Return cache.ComputeIfAbsent(
            name.ToLower,
            lazyValue:=Function(dbname)
                           Return GetVocabulary(ExternalDatabase, name)
                       End Function)
    End Function

    Public Function GetVocabulary(category As String, term As String) As vocabulary
        Dim q = registry.vocabulary _
            .where(field("category") = category,
                    field("term") = term) _
            .find(Of vocabulary)

        If q Is Nothing Then
            Call registry.vocabulary.add(
                field("category") = category,
                field("term") = term
            )

            q = registry.vocabulary _
                .where(field("category") = category,
                        field("term") = term) _
                .find(Of vocabulary)
        End If

        Return q
    End Function

    Public Function GetRegistryEntity(type As String) As vocabulary
        Static cache As New Dictionary(Of String, vocabulary)
        Return cache.ComputeIfAbsent(
            type.ToLower,
            lazyValue:=Function(dbname)
                           Return GetVocabulary(EntityType, type)
                       End Function)
    End Function

End Class
