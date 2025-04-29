Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes

''' <summary>
''' query of the biocad_registry
''' </summary>
''' 
<Package("registry")>
Module registry

    ''' <summary>
    ''' get taxonomy node information via the taxonomy name or the taxonomy id
    ''' </summary>
    ''' <param name="registry"></param>
    ''' <param name="tax">the taxonomy name or the ncbi taxonomy id</param>
    ''' <returns></returns>
    <ExportAPI("get_taxinfo")>
    Public Function get_taxinfo(registry As biocad_registry, tax As String) As taxonomyInfo
        Dim q As FieldAssert

        If tax.IsPattern("\d+") Then
            ' is tax id
            q = field("`ncbi_taxonomy`.id") = tax
        Else
            ' is tax name
            q = field("`ncbi_taxonomy`.taxname") = tax
        End If

        Return registry.ncbi_taxonomy _
            .left_join("vocabulary") _
            .on(field("`vocabulary`.id") = field("`ncbi_taxonomy`.`rank`")) _
            .where(q) _
            .find(Of taxonomyInfo)("ncbi_taxonomy.id AS ncbi_taxid",
    "taxname",
    "term AS `rank`",
    "parent_id",
    "description")
    End Function

    <ExportAPI("find_taxinfo")>
    Public Function find_taxinfo(registry As biocad_registry, tax As String) As taxonomyInfo()
        Return registry.ncbi_taxonomy _
            .left_join("vocabulary") _
            .on(field("`vocabulary`.id") = field("`ncbi_taxonomy`.`rank`")) _
            .where(match("taxname", "description").against(tax, booleanMode:=True)) _
            .select(Of taxonomyInfo)("ncbi_taxonomy.id AS ncbi_taxid",
    "taxname",
    "term AS `rank`",
    "parent_id",
    "description")
    End Function

End Module

Public Class taxonomyInfo

    <DatabaseField> Public Property ncbi_taxid As UInteger
    <DatabaseField> Public Property taxname As String
    <DatabaseField> Public Property rank As String
    <DatabaseField> Public Property parent_id As UInteger
    <DatabaseField> Public Property description As String

End Class
