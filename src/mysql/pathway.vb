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
''' define the biological pathway data
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("pathway", Database:="cad_registry", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `pathway` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(1024) NOT NULL,
  `add_time` DATETIME NOT NULL DEFAULT now(),
  `source` INT UNSIGNED NOT NULL COMMENT 'the vocabulary id of the database source name',
  `note` MEDIUMTEXT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
COMMENT = 'define the biological pathway data';
")>
Public Class pathway: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("name"), NotNull, DataType(MySqlDbType.VarChar, "1024"), Column(Name:="name")> Public Property name As String
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
''' <summary>
''' the vocabulary id of the database source name
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("source"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="source")> Public Property source As UInteger
    <DatabaseField("note"), DataType(MySqlDbType.Text), Column(Name:="note")> Public Property note As String
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `pathway` (`name`, `add_time`, `source`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `pathway` (`id`, `name`, `add_time`, `source`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `pathway` (`name`, `add_time`, `source`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `pathway` (`id`, `name`, `add_time`, `source`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `pathway` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `pathway` SET `id`='{0}', `name`='{1}', `add_time`='{2}', `source`='{3}', `note`='{4}' WHERE `id` = '{5}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `pathway` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `pathway` (`id`, `name`, `add_time`, `source`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, name, MySqlScript.ToMySqlDateTimeString(add_time), source, note)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `pathway` (`id`, `name`, `add_time`, `source`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, name, MySqlScript.ToMySqlDateTimeString(add_time), source, note)
        Else
        Return String.Format(INSERT_SQL, name, MySqlScript.ToMySqlDateTimeString(add_time), source, note)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{name}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{source}', '{note}')"
        Else
            Return $"('{name}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{source}', '{note}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `pathway` (`id`, `name`, `add_time`, `source`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, name, MySqlScript.ToMySqlDateTimeString(add_time), source, note)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `pathway` (`id`, `name`, `add_time`, `source`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, name, MySqlScript.ToMySqlDateTimeString(add_time), source, note)
        Else
        Return String.Format(REPLACE_SQL, name, MySqlScript.ToMySqlDateTimeString(add_time), source, note)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `pathway` SET `id`='{0}', `name`='{1}', `add_time`='{2}', `source`='{3}', `note`='{4}' WHERE `id` = '{5}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, name, MySqlScript.ToMySqlDateTimeString(add_time), source, note, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As pathway
                         Return DirectCast(MyClass.MemberwiseClone, pathway)
                     End Function
End Class


End Namespace
