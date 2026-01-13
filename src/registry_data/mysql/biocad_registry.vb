Imports Oracle.LinuxCompatibility.MySQL
Imports Oracle.LinuxCompatibility.MySQL.Uri
Imports registry_data.biocad_registryModel

Public Class biocad_registry : Inherits biocad_registryModel.db_mysql

    Public ReadOnly Property metabolites As TableModel(Of metabolites)
        Get
            Return m_metabolites
        End Get
    End Property

    Public ReadOnly Property vocabulary As TableModel(Of vocabulary)
        Get
            Return m_vocabulary
        End Get
    End Property

    Public ReadOnly Property db_xrefs As TableModel(Of db_xrefs)
        Get
            Return m_db_xrefs
        End Get
    End Property

    Public ReadOnly Property ontology As TableModel(Of ontology)
        Get
            Return m_ontology
        End Get
    End Property

    Public ReadOnly Property ontology_relation As TableModel(Of ontology_relation)
        Get
            Return m_ontology_relation
        End Get
    End Property

    Public ReadOnly Property metabolite_class As TableModel(Of metabolite_class)
        Get
            Return m_metabolite_class
        End Get
    End Property

    Public ReadOnly Property synonym As TableModel(Of synonym)
        Get
            Return m_synonym
        End Get
    End Property

    Public ReadOnly Property struct_data As TableModel(Of struct_data)
        Get
            Return m_struct_data
        End Get
    End Property

    Public ReadOnly Property topic As TableModel(Of topic)
        Get
            Return m_topic
        End Get
    End Property

    Public ReadOnly Property registry_resolver As TableModel(Of registry_resolver)
        Get
            Return m_registry_resolver
        End Get
    End Property

    Public ReadOnly Property motif As TableModel(Of motif)
        Get
            Return m_motif
        End Get
    End Property

    Public ReadOnly Property nucleotide_data As TableModel(Of nucleotide_data)
        Get
            Return m_nucleotide_data
        End Get
    End Property

    Public ReadOnly Property protein_data As TableModel(Of protein_data)
        Get
            Return m_protein_data
        End Get
    End Property

    Public ReadOnly Property compartment_location As TableModel(Of compartment_location)
        Get
            Return m_compartment_location
        End Get
    End Property

    Public ReadOnly Property compartment_enrich As TableModel(Of compartment_enrich)
        Get
            Return m_compartment_enrich
        End Get
    End Property

    Public ReadOnly Property subcellular_location As TableModel(Of subcellular_location)
        Get
            Return m_subcellular_location
        End Get
    End Property

    Public ReadOnly Property protein As TableModel(Of protein)
        Get
            Return m_protein
        End Get
    End Property

    Public ReadOnly Property reaction As TableModel(Of reaction)
        Get
            Return m_reaction
        End Get
    End Property

    Public ReadOnly Property metabolic_network As TableModel(Of metabolic_network)
        Get
            Return m_metabolic_network
        End Get
    End Property

    Public ReadOnly Property ncbi_taxonomy As TableModel(Of ncbi_taxonomy)
        Get
            Return m_ncbi_taxonomy
        End Get
    End Property

    Public ReadOnly Property biocad_vocabulary As biocad_vocabulary

    Public Sub New(mysqli As ConnectionUri)
        MyBase.New(mysqli)
        Me.biocad_vocabulary = New biocad_vocabulary(Me)
    End Sub
End Class