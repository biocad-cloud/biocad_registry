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


    Public Sub New(mysqli As ConnectionUri)
        MyBase.New(mysqli)
    End Sub
End Class