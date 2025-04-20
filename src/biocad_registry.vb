Imports Microsoft.VisualBasic.ComponentModel.Collection
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

    Public ReadOnly Property subcellular_compartments As Model
        Get
            Return m_subcellular_compartments
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
    Public Function getVocabulary(term As String, category As String, Optional description As String = "") As UInteger
        Static cache As New Dictionary(Of String, UInteger)
        SyncLock cache
            Return cache.ComputeIfAbsent($"{Strings.LCase(category)}:{Strings.LCase(term)}",
                lazyValue:=Function()
                               Return queryVocabulary(term, category, description)
                           End Function)
        End SyncLock
    End Function

    Private Function queryVocabulary(term As String, category As String, description As String) As UInteger
        Dim check = m_vocabulary.where(
            field("category") = category,
            field("term") = term
        ).find(Of biocad_registryModel.vocabulary)

        If check Is Nothing Then
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
            Throw New InvalidProgramException
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
