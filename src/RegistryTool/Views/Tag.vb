Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports registry_data
Imports RegistryTool.My

Public Class Tag

    <DatabaseField> Public Property term As String
    <DatabaseField> Public Property tag_id As UInteger
    <DatabaseField> Public Property molecule_id As Long

    Public Overrides Function ToString() As String
        Return term
    End Function

    Public Shared Function GetTags(mol As UInteger) As Tag()
        Dim model = MyApplication.biocad_registry.GetMetaboliteModel(mol)

        If model Is Nothing Then
            Return {}
        End If

        Return MyApplication.biocad_registry.topic _
            .left_join("vocabulary") _
            .on(field("`vocabulary`.id") = field("topic_id")) _
            .where(field("model_id") = model.id) _
            .order_by("term") _
            .select(Of Tag)("term", "topic_id as tag_id", $"{mol} as molecule_id")
    End Function

End Class
