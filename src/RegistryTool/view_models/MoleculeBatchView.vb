Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports RegistryTool.My

Public Class MoleculeBatchView

    <DatabaseField> Public Property id As UInteger
    <DatabaseField> Public Property name As String
    <DatabaseField> Public Property tags As String
    <DatabaseField> Public Property note As String

    Public Shared Function FromIdSet(ids As IEnumerable(Of UInteger)) As MoleculeBatchView()
        Return MyApplication.biocad_registry.metabolites _
            .left_join("molecule_tags").on(field("`molecule`.id") = field("molecule_id")) _
            .left_join("vocabulary").on(field("vocabulary.id") = field("tag_id")) _
            .where(field("molecule.id").in(ids)) _
            .group_by("`molecule`.id") _
            .select(Of MoleculeBatchView)("`molecule`.id",
                                          "min(name) as name",
                                          "group_concat(distinct term) as tags",
                                          "min(`molecule`.note) as note")
    End Function

End Class
