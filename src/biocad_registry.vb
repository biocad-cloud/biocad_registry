Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Uri

Public Class biocad_registry : Inherits biocad_registryModel.db_mysql

    Sub New(mysql As ConnectionUri)
        Call MyBase.New(mysql)
    End Sub

    Public Function getVocabulary(term As String, category As String) As UInteger
        Static cache As New Dictionary(Of String, UInteger)
        SyncLock cache
            Return cache.ComputeIfAbsent($"{Strings.LCase(category)}:{Strings.LCase(term)}",
                lazyValue:=Function()
                               Call m_vocabulary.add(
                                   field("category") = category,
                                   field("term") = term
                               )

                               Dim check = m_vocabulary.where(
                                  field("category") = category,
                                  field("term") = term
                               ).find(Of biocad_registryModel.vocabulary)

                               If check Is Nothing Then
                                   Throw New InvalidProgramException
                               Else
                                   Return check.id
                               End If
                           End Function)
        End SyncLock
    End Function
End Class
