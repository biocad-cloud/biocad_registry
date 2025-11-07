Imports Microsoft.VisualBasic.MIME.application.json
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes

Module exportwebJSONDb

    Const db_cache As String = "G:\BlueprintCAD\App\net8.0-windows\data\local"

    Sub runlocalDbCache()
        Call exportOperonDb()


        Pause()
    End Sub

    Sub exportOperonDb()
        Dim list = registry.conserved_cluster _
            .left_join("cluster_link") _
            .on(field("`cluster_link`.`cluster_id`") = field("`conserved_cluster`.`id`")) _
            .where("NOT cluster_id IS NULL") _
            .group_by("cluster_id") _
            .select(Of localcacheViews.operonData)("cluster_id", "MIN(name) AS name", "GROUP_CONCAT(DISTINCT gene_id) AS members")
        Dim operons = list _
            .Select(Function(a)
                        Return New WebJSON.Operon With {
                            .cluster_id = a.cluster_id,
                            .name = a.name,
                            .members = a.members.Split(",")
                        }
                    End Function) _
            .ToArray

        Call operons.GetJson.SaveTo($"{db_cache}/all_operons.json")
    End Sub
End Module

Namespace localcacheViews

    Public Class operonData

        <DatabaseField> Public Property cluster_id As UInteger
        <DatabaseField> Public Property name As String
        <DatabaseField> Public Property members As String

    End Class

End Namespace