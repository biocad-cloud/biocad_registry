REM  Oracle.LinuxCompatibility.MySQL.CodeSolution.VisualBasic.CodeGenerator
REM  MYSQL Schema Mapper
REM      for Microsoft VisualBasic.NET 1.0.0.0

REM  Dump @4/10/2025 11:27:19 PM


Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports MySqlScript = Oracle.LinuxCompatibility.MySQL.Scripting.Extensions

Namespace biocad_registryModel

''' <summary>
''' ```SQL
''' the ncbi taxonomy tree information
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("ncbi_taxonomy", Database:="cad_registry", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `ncbi_taxonomy` (
  `id` INT UNSIGNED NOT NULL COMMENT 'ncbi tax id, not auto incremental',
  `taxname` VARCHAR(255) NOT NULL,
  `nsize` INT UNSIGNED NOT NULL DEFAULT 0 COMMENT 'size of the child nodes',
  `parent_id` INT UNSIGNED NULL DEFAULT NULL,
  `rank` INT UNSIGNED NOT NULL,
  `add_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `description` LONGTEXT COLLATE 'utf8mb3_bin' NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE,
  INDEX `match_name` (`taxname` ASC) VISIBLE,
  INDEX `find_parent` (`parent_id` ASC) VISIBLE,
  INDEX `check_leaf_node` (`nsize` ASC) INVISIBLE,
  FULLTEXT INDEX `search_name_text` (`taxname`, `description`) VISIBLE,
  INDEX `rank_level_idx` (`rank` ASC) VISIBLE)
ENGINE = InnoDB
COMMENT = 'the ncbi taxonomy tree information';
")>
Public Class ncbi_taxonomy: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
''' <summary>
''' ncbi tax id, not auto incremental
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("id"), PrimaryKey, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("taxname"), NotNull, DataType(MySqlDbType.VarChar, "255"), Column(Name:="taxname")> Public Property taxname As String
''' <summary>
''' size of the child nodes
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("nsize"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="nsize")> Public Property nsize As UInteger
    <DatabaseField("parent_id"), DataType(MySqlDbType.UInt32, "11"), Column(Name:="parent_id")> Public Property parent_id As UInteger
    <DatabaseField("rank"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="rank")> Public Property rank As UInteger
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
    <DatabaseField("description"), DataType(MySqlDbType.Text), Column(Name:="description")> Public Property description As String
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `ncbi_taxonomy` (`id`, `taxname`, `nsize`, `parent_id`, `rank`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `ncbi_taxonomy` (`id`, `taxname`, `nsize`, `parent_id`, `rank`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `ncbi_taxonomy` (`id`, `taxname`, `nsize`, `parent_id`, `rank`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `ncbi_taxonomy` (`id`, `taxname`, `nsize`, `parent_id`, `rank`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `ncbi_taxonomy` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `ncbi_taxonomy` SET `id`='{0}', `taxname`='{1}', `nsize`='{2}', `parent_id`='{3}', `rank`='{4}', `add_time`='{5}', `description`='{6}' WHERE `id` = '{7}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `ncbi_taxonomy` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `ncbi_taxonomy` (`id`, `taxname`, `nsize`, `parent_id`, `rank`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, id, taxname, nsize, parent_id, rank, MySqlScript.ToMySqlDateTimeString(add_time), description)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `ncbi_taxonomy` (`id`, `taxname`, `nsize`, `parent_id`, `rank`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, taxname, nsize, parent_id, rank, MySqlScript.ToMySqlDateTimeString(add_time), description)
        Else
        Return String.Format(INSERT_SQL, id, taxname, nsize, parent_id, rank, MySqlScript.ToMySqlDateTimeString(add_time), description)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{taxname}', '{nsize}', '{parent_id}', '{rank}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{description}')"
        Else
            Return $"('{id}', '{taxname}', '{nsize}', '{parent_id}', '{rank}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{description}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `ncbi_taxonomy` (`id`, `taxname`, `nsize`, `parent_id`, `rank`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, id, taxname, nsize, parent_id, rank, MySqlScript.ToMySqlDateTimeString(add_time), description)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `ncbi_taxonomy` (`id`, `taxname`, `nsize`, `parent_id`, `rank`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, taxname, nsize, parent_id, rank, MySqlScript.ToMySqlDateTimeString(add_time), description)
        Else
        Return String.Format(REPLACE_SQL, id, taxname, nsize, parent_id, rank, MySqlScript.ToMySqlDateTimeString(add_time), description)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `ncbi_taxonomy` SET `id`='{0}', `taxname`='{1}', `nsize`='{2}', `parent_id`='{3}', `rank`='{4}', `add_time`='{5}', `description`='{6}' WHERE `id` = '{7}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, taxname, nsize, parent_id, rank, MySqlScript.ToMySqlDateTimeString(add_time), description, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As ncbi_taxonomy
                         Return DirectCast(MyClass.MemberwiseClone, ncbi_taxonomy)
                     End Function
End Class


End Namespace
