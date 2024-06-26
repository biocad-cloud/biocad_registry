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
''' Taxonomic group
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("taxonomic", Database:="cad_registry", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `taxonomic` (
  `id` INT UNSIGNED NOT NULL COMMENT 'assign external tax id, so this id index field is not auto-incremental',
  `name` VARCHAR(255) NOT NULL,
  `n_tax` INT UNSIGNED NOT NULL DEFAULT 0,
  `add_time` DATETIME NOT NULL DEFAULT now(),
  `note` VARCHAR(4096) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
COMMENT = 'Taxonomic group';
")>
Public Class taxonomic: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
''' <summary>
''' assign external tax id, so this id index field is not auto-incremental
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("id"), PrimaryKey, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("name"), NotNull, DataType(MySqlDbType.VarChar, "255"), Column(Name:="name")> Public Property name As String
    <DatabaseField("n_tax"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="n_tax")> Public Property n_tax As UInteger
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
    <DatabaseField("note"), DataType(MySqlDbType.VarChar, "4096"), Column(Name:="note")> Public Property note As String
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `taxonomic` (`id`, `name`, `n_tax`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `taxonomic` (`id`, `name`, `n_tax`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `taxonomic` (`id`, `name`, `n_tax`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `taxonomic` (`id`, `name`, `n_tax`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `taxonomic` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `taxonomic` SET `id`='{0}', `name`='{1}', `n_tax`='{2}', `add_time`='{3}', `note`='{4}' WHERE `id` = '{5}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `taxonomic` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `taxonomic` (`id`, `name`, `n_tax`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, id, name, n_tax, MySqlScript.ToMySqlDateTimeString(add_time), note)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `taxonomic` (`id`, `name`, `n_tax`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, name, n_tax, MySqlScript.ToMySqlDateTimeString(add_time), note)
        Else
        Return String.Format(INSERT_SQL, id, name, n_tax, MySqlScript.ToMySqlDateTimeString(add_time), note)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{name}', '{n_tax}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{note}')"
        Else
            Return $"('{id}', '{name}', '{n_tax}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{note}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `taxonomic` (`id`, `name`, `n_tax`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, id, name, n_tax, MySqlScript.ToMySqlDateTimeString(add_time), note)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `taxonomic` (`id`, `name`, `n_tax`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, name, n_tax, MySqlScript.ToMySqlDateTimeString(add_time), note)
        Else
        Return String.Format(REPLACE_SQL, id, name, n_tax, MySqlScript.ToMySqlDateTimeString(add_time), note)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `taxonomic` SET `id`='{0}', `name`='{1}', `n_tax`='{2}', `add_time`='{3}', `note`='{4}' WHERE `id` = '{5}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, name, n_tax, MySqlScript.ToMySqlDateTimeString(add_time), note, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As taxonomic
                         Return DirectCast(MyClass.MemberwiseClone, taxonomic)
                     End Function
End Class


End Namespace
