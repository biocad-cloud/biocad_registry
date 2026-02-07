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
        Return MyApplication.biocad_registry.vocabulary _
            .where(field("category") = "Topic") _
            .select(Of TopicTerm) _
            .OrderBy(Function(t) t.term.ToLower) _
            .ToArray
    End Function

    Public Shared Function GetDatabaseTerm() As TopicTerm()
        Return MyApplication.biocad_registry.vocabulary _
            .where(field("category") = "External Database") _
            .select(Of TopicTerm) _
            .OrderBy(Function(t) t.term.ToLower) _
            .ToArray
    End Function

End Class
