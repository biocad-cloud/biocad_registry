REM  Oracle.LinuxCompatibility.MySQL.CodeSolution.VisualBasic.CodeGenerator
REM  MYSQL Schema Mapper
REM      for Microsoft VisualBasic.NET 1.0.0.0

REM  Dump @4/4/2024 5:05:27 PM


Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports MySqlScript = Oracle.LinuxCompatibility.MySQL.Scripting.Extensions

Namespace biocad_registryModel

''' <summary>
''' ```SQL
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("ncbi_taxonomy_tree", Database:="cad_registry", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `ncbi_taxonomy_tree` (
  `id` INT UNSIGNED NOT NULL COMMENT 'The ncbi taxonomy id',
  `name` VARCHAR(1024) NOT NULL,
  `parent` INT UNSIGNED NOT NULL,
  `rank` INT UNSIGNED NOT NULL COMMENT 'the vocabulary term id in the biocad registry',
  `n_childs` INT UNSIGNED NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;
")>
Public Class ncbi_taxonomy_tree: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
''' <summary>
''' The ncbi taxonomy id
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("id"), PrimaryKey, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("name"), NotNull, DataType(MySqlDbType.VarChar, "1024"), Column(Name:="name")> Public Property name As String
    <DatabaseField("parent"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="parent")> Public Property parent As UInteger
''' <summary>
''' the vocabulary term id in the biocad registry
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("rank"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="rank")> Public Property rank As UInteger
    <DatabaseField("n_childs"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="n_childs")> Public Property n_childs As UInteger
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `ncbi_taxonomy_tree` (`id`, `name`, `parent`, `rank`, `n_childs`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `ncbi_taxonomy_tree` (`id`, `name`, `parent`, `rank`, `n_childs`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `ncbi_taxonomy_tree` (`id`, `name`, `parent`, `rank`, `n_childs`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `ncbi_taxonomy_tree` (`id`, `name`, `parent`, `rank`, `n_childs`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `ncbi_taxonomy_tree` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `ncbi_taxonomy_tree` SET `id`='{0}', `name`='{1}', `parent`='{2}', `rank`='{3}', `n_childs`='{4}' WHERE `id` = '{5}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `ncbi_taxonomy_tree` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `ncbi_taxonomy_tree` (`id`, `name`, `parent`, `rank`, `n_childs`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, id, name, parent, rank, n_childs)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `ncbi_taxonomy_tree` (`id`, `name`, `parent`, `rank`, `n_childs`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, name, parent, rank, n_childs)
        Else
        Return String.Format(INSERT_SQL, id, name, parent, rank, n_childs)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{name}', '{parent}', '{rank}', '{n_childs}')"
        Else
            Return $"('{id}', '{name}', '{parent}', '{rank}', '{n_childs}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `ncbi_taxonomy_tree` (`id`, `name`, `parent`, `rank`, `n_childs`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, id, name, parent, rank, n_childs)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `ncbi_taxonomy_tree` (`id`, `name`, `parent`, `rank`, `n_childs`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, name, parent, rank, n_childs)
        Else
        Return String.Format(REPLACE_SQL, id, name, parent, rank, n_childs)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `ncbi_taxonomy_tree` SET `id`='{0}', `name`='{1}', `parent`='{2}', `rank`='{3}', `n_childs`='{4}' WHERE `id` = '{5}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, name, parent, rank, n_childs, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As ncbi_taxonomy_tree
                         Return DirectCast(MyClass.MemberwiseClone, ncbi_taxonomy_tree)
                     End Function
End Class


End Namespace
