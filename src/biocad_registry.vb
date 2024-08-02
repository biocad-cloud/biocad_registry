Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Uri

''' <summary>
''' a wrapper of the biocad registry database
''' </summary>
Public Class biocad_registry : Inherits biocad_registryModel.db_mysql

    Sub New(mysql As ConnectionUri)
        Call MyBase.New(mysql)
    End Sub

    Public Function getVocabulary(term As String, category As String) As UInteger
        Static cache As New Dictionary(Of String, UInteger)
        SyncLock cache
            Return cache.ComputeIfAbsent($"{Strings.LCase(category)}:{Strings.LCase(term)}",
                lazyValue:=Function()
                               Dim check = m_vocabulary.where(
                                  field("category") = category,
                                  field("term") = term
                               ).find(Of biocad_registryModel.vocabulary)

                               If check Is Nothing Then
                                   Call m_vocabulary.add(
                                       field("category") = category,
                                       field("term") = term
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
                           End Function)
        End SyncLock
    End Function

    Public Function getSubcellularLocation(name As String, topology As String) As UInteger
        Static cache As New Dictionary(Of String, UInteger)
        SyncLock cache
            Return cache.ComputeIfAbsent(name.ToLower,
                lazyValue:=Function()
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
                           End Function)
        End SyncLock
    End Function
End Class
