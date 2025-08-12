Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Class BioCadTaxonomy

    ReadOnly unknown_taxonomy As UInteger
    ReadOnly species As UInteger

    ReadOnly registry As biocad_registry
    ReadOnly cache As New Dictionary(Of String, biocad_registryModel.ncbi_taxonomy)

    Sub New(registry As biocad_registry)
        Me.registry = registry
        Me.unknown_taxonomy = registry.ncbi_taxonomy _
            .where(field("taxname") = "Unknown Taxonomy") _
            .find(Of biocad_registryModel.ncbi_taxonomy) _
            .id
        Me.species = registry.vocabulary _
            .where(field("category") = "Taxonomy Rank",
                   field("term") = "species") _
            .find(Of biocad_registryModel.vocabulary) _
            .id
    End Sub

    Public Function taxid(name As String, Optional find_new As Boolean = False) As biocad_registryModel.ncbi_taxonomy
        If cache.ContainsKey(name) Then
            Return cache(name)
        End If

        Dim find As biocad_registryModel.ncbi_taxonomy = registry.ncbi_taxonomy _
            .where(field("taxname") = name) _
            .order_by("id", desc:=find_new) _
            .find(Of biocad_registryModel.ncbi_taxonomy)

        If find IsNot Nothing Then
            Call cache.Add(name, find)
        End If

        Return find
    End Function

    Public Function addUnknownTaxonomy(name As String) As UInteger
        Dim nsize As UInteger = registry.ncbi_taxonomy _
            .where(field("parent_id") = unknown_taxonomy) _
            .count

        Call registry.ncbi_taxonomy.add(
            field("taxname") = name,
            field("nsize") = 0,
            field("parent_id") = unknown_taxonomy,
            field("rank") = species,
            field("id") = unknown_taxonomy + nsize + 1
        )

        ' update tree
        Call registry.ncbi_taxonomy _
            .where(field("id") = unknown_taxonomy) _
            .save(field("nsize") = nsize + 1)

        Dim unknown_id = taxid(name, find_new:=True)

        Call registry.taxonomy_tree.add(
            field("tax_id") = unknown_taxonomy,
            field("child_tax") = unknown_id.id
        )

        Return unknown_id.id
    End Function

End Class
