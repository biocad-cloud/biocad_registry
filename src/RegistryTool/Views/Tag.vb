Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports RegistryTool.My

Public Class Tag

    Public Property term As String
    Public Property tag_id As UInteger
    Public Property molecule_id As UInteger

    Public Overrides Function ToString() As String
        Return term
    End Function

    Public Shared Function GetTags(mol As UInteger) As Tag()
        Return MyApplication.biocad_registry.molecule_tags _
            .left_join("vocabulary") _
            .on(field("`vocabulary`.id") = field("molecule_tags.tag_id")) _
            .where(field("molecule_id") = mol) _
            .select(Of Tag)("term", "tag_id", "molecule_id")
    End Function

End Class
