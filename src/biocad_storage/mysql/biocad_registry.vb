﻿Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Oracle.LinuxCompatibility.MySQL
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Uri

''' <summary>
''' a wrapper of the biocad registry database
''' </summary>
Public Class biocad_registry : Inherits biocad_registryModel.db_mysql

    Public ReadOnly Property vocabulary As Model
        Get
            Return m_vocabulary
        End Get
    End Property

    Public ReadOnly Property hashcode As TableModel(Of biocad_registryModel.hashcode)
        Get
            Return m_hashcode
        End Get
    End Property

    Public ReadOnly Property conserved_cluster As TableModel(Of biocad_registryModel.conserved_cluster)
        Get
            Return m_conserved_cluster
        End Get
    End Property

    Public ReadOnly Property cluster_link As TableModel(Of biocad_registryModel.cluster_link)
        Get
            Return m_cluster_link
        End Get
    End Property

    Public ReadOnly Property subcellular_compartments As Model
        Get
            Return m_subcellular_compartments
        End Get
    End Property

    Public ReadOnly Property subcellular_location As TableModel(Of biocad_registryModel.subcellular_location)
        Get
            Return m_subcellular_location
        End Get
    End Property

    Public ReadOnly Property reaction As TableModel(Of biocad_registryModel.reaction)
        Get
            Return m_reaction
        End Get
    End Property

    Public ReadOnly Property reaction_graph As Model
        Get
            Return m_reaction_graph
        End Get
    End Property

    Public ReadOnly Property regulation_graph As Model
        Get
            Return m_regulation_graph
        End Get
    End Property

    Public ReadOnly Property db_xrefs As Model
        Get
            Return m_db_xrefs
        End Get
    End Property

    Public ReadOnly Property molecule_function As Model
        Get
            Return m_molecule_function
        End Get
    End Property

    Public ReadOnly Property molecule As TableModel(Of biocad_registryModel.molecule)
        Get
            Return m_molecule
        End Get
    End Property

    Public ReadOnly Property molecule_tags As TableModel(Of biocad_registryModel.molecule_tags)
        Get
            Return m_molecule_tags
        End Get
    End Property

    Public ReadOnly Property sequence_graph As TableModel(Of biocad_registryModel.sequence_graph)
        Get
            Return m_sequence_graph
        End Get
    End Property

    Public ReadOnly Property odor As Model
        Get
            Return m_odor
        End Get
    End Property

    Public ReadOnly Property ncbi_taxonomy As Model
        Get
            Return m_ncbi_taxonomy
        End Get
    End Property

    Public ReadOnly Property taxonomy_tree As Model
        Get
            Return m_taxonomy_tree
        End Get
    End Property

    Public ReadOnly Property taxonomy_source As TableModel(Of biocad_registryModel.taxonomy_source)
        Get
            Return m_taxonomy_source
        End Get
    End Property

    Public ReadOnly Property genomics As TableModel(Of biocad_registryModel.genomics)
        Get
            Return m_genomics
        End Get
    End Property

    Public ReadOnly Property synonym As TableModel(Of biocad_registryModel.synonym)
        Get
            Return m_synonym
        End Get
    End Property

    Public ReadOnly Property ontology As TableModel(Of biocad_registryModel.ontology)
        Get
            Return m_ontology
        End Get
    End Property

    Public ReadOnly Property molecule_ontology As TableModel(Of biocad_registryModel.molecule_ontology)
        Get
            Return m_molecule_ontology
        End Get
    End Property

    Public ReadOnly Property ontology_tree As TableModel(Of biocad_registryModel.ontology_tree)
        Get
            Return m_ontology_tree
        End Get
    End Property

    Public ReadOnly Property mesh As TableModel(Of biocad_registryModel.mesh)
        Get
            Return m_mesh
        End Get
    End Property

    Public ReadOnly Property mesh_link As TableModel(Of biocad_registryModel.mesh_link)
        Get
            Return m_mesh_link
        End Get
    End Property

    Public ReadOnly Property pubmed As TableModel(Of biocad_registryModel.pubmed)
        Get
            Return m_pubmed
        End Get
    End Property

    Public ReadOnly Property pubmed_source As TableModel(Of biocad_registryModel.pubmed_source)
        Get
            Return m_pubmed_source
        End Get
    End Property

    Public ReadOnly Property kinetic_law As TableModel(Of biocad_registryModel.kinetic_law)
        Get
            Return m_kinetic_law
        End Get
    End Property

    Public ReadOnly Property kinetic_substrate As TableModel(Of biocad_registryModel.kinetic_substrate)
        Get
            Return m_kinetic_substrate
        End Get
    End Property

    Public ReadOnly Property vocabulary_terms As BioCadVocabulary

    Sub New(mysql As ConnectionUri)
        Call MyBase.New(mysql)

        vocabulary_terms = New BioCadVocabulary(Me)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="term"></param>
    ''' <param name="category"></param>
    ''' <param name="description"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' has an internal cache of the vocabulary term data
    ''' </remarks>
    Public Function getVocabulary(term As String, category As String,
                                  Optional description As String = "",
                                  Optional [readonly] As Boolean = False) As UInteger

        Static cache As New Dictionary(Of String, UInteger)
        SyncLock cache
            Return cache.ComputeIfAbsent($"{Strings.LCase(category)}:{Strings.LCase(term)}",
                lazyValue:=Function()
                               Return queryVocabulary(term, category, description, [readonly])
                           End Function)
        End SyncLock
    End Function

    Private Function queryVocabulary(term As String, category As String, description As String, [readonly] As Boolean) As UInteger
        Dim check = m_vocabulary.where(
            field("category") = category,
            field("term") = term
        ).find(Of biocad_registryModel.vocabulary)

        If check Is Nothing AndAlso Not [readonly] Then
            Call m_vocabulary.add(
                field("category") = category,
                field("term") = term,
                field("note") = description
            )
            check = m_vocabulary.where(
                field("category") = category,
                field("term") = term
            ).find(Of biocad_registryModel.vocabulary)
        End If

        If check Is Nothing Then
            Throw New InvalidProgramException($"query of the biocad vocabulary term '{term}'@{category} error!")
        Else
            Return check.id
        End If
    End Function

    Public Function getSubcellularLocation(name As String, topology As String) As UInteger
        Static cache As New Dictionary(Of String, UInteger)
        SyncLock cache
            Return cache.ComputeIfAbsent(name.ToLower,
                lazyValue:=Function()
                               Return querySubcellularLocation(name, topology)
                           End Function)
        End SyncLock
    End Function

    Private Function querySubcellularLocation(name As String, topology As String) As UInteger
        Dim check = m_subcellular_compartments _
            .where(field("compartment_name") = name) _
            .find(Of biocad_registryModel.subcellular_compartments)

        If check Is Nothing Then
            Call m_subcellular_compartments.add(
                field("compartment_name") = name,
                field("topology") = topology
            )
            check = m_subcellular_compartments _
               .where(field("compartment_name") = name) _
               .find(Of biocad_registryModel.subcellular_compartments)
        End If

        If check Is Nothing Then
            Throw New InvalidProgramException
        Else
            Return check.id
        End If
    End Function
End Class
