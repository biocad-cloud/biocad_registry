Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Class biocad_vocabulary

    Public Const ExternalDatabase As String = "External Database"
    Public Const EntityType As String = "Entity Type"
    Public Const Topic As String = "Topic"
    Public Const FastaData As String = "Fasta Data"

    Public Const NCBITaxonomyRank As String = "NCBI Taxonomy Rank"

    ' reference model type
    Public Const EntityMetabolite As String = "Metabolite"
    Public Const EntityNucleotide As String = "Nucleotide"
    Public Const EntityProtein As String = "Protein"
    Public Const EntityReaction As String = "Reaction"
    Public Const EntitySubcellularLocation As String = "Subcellular Location"

    ' object instance type
    Public Const ObjectProtein As String = "Protein Sequence"
    Public Const ObjectNucleotide As String = "Nucleotide Sequence"

    ''' <summary>
    ''' Metabolic Role
    ''' </summary>
    Public Const RoleEnzyme As String = "Enzyme"

    ReadOnly registry As biocad_registry

    Public ReadOnly Property db_cas As UInteger
    Public ReadOnly Property db_pubchem As UInteger
    Public ReadOnly Property db_chebi As UInteger
    Public ReadOnly Property db_hmdb As UInteger
    Public ReadOnly Property db_lipidmaps As UInteger
    Public ReadOnly Property db_kegg As UInteger
    Public ReadOnly Property db_biocyc As UInteger
    Public ReadOnly Property db_mesh As UInteger
    Public ReadOnly Property db_wikipedia As UInteger
    Public ReadOnly Property db_drugbank As UInteger

    Public ReadOnly Property db_genbank As UInteger
    Public ReadOnly Property db_uniprot As UInteger
    Public ReadOnly Property db_ECNumber As UInteger

    Public ReadOnly Property metabolite_type As UInteger
    ''' <summary>
    ''' the protein reference model object
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property protein_type As UInteger
    Public ReadOnly Property reaction_type As UInteger

    ''' <summary>
    ''' the protein fasta seuqnece data
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property protein_data As UInteger
    Public ReadOnly Property nucleotide_data As UInteger

    Sub New(registry As biocad_registry)
        Me.registry = registry

        db_cas = GetDatabaseResource("CAS Registry Number").id
        db_pubchem = GetDatabaseResource("PubChem").id
        db_chebi = GetDatabaseResource("ChEBI").id
        db_hmdb = GetDatabaseResource("HMDB").id
        db_lipidmaps = GetDatabaseResource("LipidMaps").id
        db_kegg = GetDatabaseResource("KEGG").id
        db_biocyc = GetDatabaseResource("BioCyc").id
        db_mesh = GetDatabaseResource("NCBI MeSH").id
        db_wikipedia = GetDatabaseResource("Wikipedia").id
        db_drugbank = GetDatabaseResource("DrugBank").id

        db_genbank = GetDatabaseResource("NCBI GenBank").id
        db_uniprot = GetDatabaseResource("UniProt").id
        db_ECNumber = GetDatabaseResource("EC Number").id

        metabolite_type = GetRegistryEntity(biocad_vocabulary.EntityMetabolite).id
        protein_type = GetRegistryEntity(biocad_vocabulary.EntityProtein).id
        reaction_type = GetRegistryEntity(biocad_vocabulary.EntityReaction).id

        protein_data = GetVocabulary(biocad_vocabulary.FastaData, biocad_vocabulary.ObjectProtein).id
        nucleotide_data = GetVocabulary(biocad_vocabulary.FastaData, biocad_vocabulary.ObjectNucleotide).id
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function GetDatabaseResource(name As String) As biocad_registryModel.vocabulary
        Static cache As New Dictionary(Of String, biocad_registryModel.vocabulary)
        Return cache.ComputeIfAbsent(
            name.ToLower,
            lazyValue:=Function(dbname)
                           Return GetVocabulary(ExternalDatabase, name)
                       End Function)
    End Function

    Public Function GetTopic(name As String) As biocad_registryModel.vocabulary
        Static cache As New Dictionary(Of String, biocad_registryModel.vocabulary)
        Return cache.ComputeIfAbsent(
            name.ToLower,
            lazyValue:=Function(topic)
                           Return GetVocabulary(biocad_vocabulary.Topic, topic)
                       End Function)
    End Function

    Public Function GetVocabulary(category As String, term As String) As biocad_registryModel.vocabulary
        Dim q = registry.vocabulary _
            .where(field("category") = category,
                    field("term") = term) _
            .find(Of biocad_registryModel.vocabulary)

        If q Is Nothing Then
            Call registry.vocabulary.add(
                field("category") = category,
                field("term") = term
            )

            q = registry.vocabulary _
                .where(field("category") = category,
                        field("term") = term) _
                .find(Of biocad_registryModel.vocabulary)
        End If

        Return q
    End Function

    Public Function GetRegistryEntity(type As String) As biocad_registryModel.vocabulary
        Static cache As New Dictionary(Of String, biocad_registryModel.vocabulary)
        Return cache.ComputeIfAbsent(
            type.ToLower,
            lazyValue:=Function(dbname)
                           Return GetVocabulary(EntityType, type)
                       End Function)
    End Function

End Class
