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

    Public Sub New(mysqli As ConnectionUri)
        MyBase.New(mysqli)
    End Sub
End Class