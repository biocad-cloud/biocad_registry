Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports RegistryTool.My

Public Class TopicTerm

    <DatabaseField> Public Property term As String
    <DatabaseField> Public Property id As UInteger

    Public Overrides Function ToString() As String
        Return term
    End Function

    Public Shared Function GetTopics() As TopicTerm()
        Return MyApplication.biocad_registry.vocabulary.where(field("category") = "Topic").select(Of TopicTerm)
    End Function

    Public Shared Function GetDatabaseTerm() As TopicTerm()
        Return MyApplication.biocad_registry.vocabulary.where(field("category") = "External Database").select(Of TopicTerm)
    End Function

End Class
