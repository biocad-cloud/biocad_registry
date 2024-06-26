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
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("family", Database:="cad_registry", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `family` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(255) NOT NULL,
  `parent_id` INT UNSIGNED NOT NULL,
  `n_childs` INT UNSIGNED NOT NULL DEFAULT 0,
  `add_time` DATETIME NOT NULL DEFAULT now(),
  `description` VARCHAR(8192) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;
")>
Public Class family: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("name"), NotNull, DataType(MySqlDbType.VarChar, "255"), Column(Name:="name")> Public Property name As String
    <DatabaseField("parent_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="parent_id")> Public Property parent_id As UInteger
    <DatabaseField("n_childs"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="n_childs")> Public Property n_childs As UInteger
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
    <DatabaseField("description"), DataType(MySqlDbType.VarChar, "8192"), Column(Name:="description")> Public Property description As String
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `family` (`name`, `parent_id`, `n_childs`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `family` (`id`, `name`, `parent_id`, `n_childs`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `family` (`name`, `parent_id`, `n_childs`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `family` (`id`, `name`, `parent_id`, `n_childs`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `family` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `family` SET `id`='{0}', `name`='{1}', `parent_id`='{2}', `n_childs`='{3}', `add_time`='{4}', `description`='{5}' WHERE `id` = '{6}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `family` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `family` (`id`, `name`, `parent_id`, `n_childs`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, name, parent_id, n_childs, MySqlScript.ToMySqlDateTimeString(add_time), description)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `family` (`id`, `name`, `parent_id`, `n_childs`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, name, parent_id, n_childs, MySqlScript.ToMySqlDateTimeString(add_time), description)
        Else
        Return String.Format(INSERT_SQL, name, parent_id, n_childs, MySqlScript.ToMySqlDateTimeString(add_time), description)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{name}', '{parent_id}', '{n_childs}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{description}')"
        Else
            Return $"('{name}', '{parent_id}', '{n_childs}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{description}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `family` (`id`, `name`, `parent_id`, `n_childs`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, name, parent_id, n_childs, MySqlScript.ToMySqlDateTimeString(add_time), description)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `family` (`id`, `name`, `parent_id`, `n_childs`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, name, parent_id, n_childs, MySqlScript.ToMySqlDateTimeString(add_time), description)
        Else
        Return String.Format(REPLACE_SQL, name, parent_id, n_childs, MySqlScript.ToMySqlDateTimeString(add_time), description)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `family` SET `id`='{0}', `name`='{1}', `parent_id`='{2}', `n_childs`='{3}', `add_time`='{4}', `description`='{5}' WHERE `id` = '{6}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, name, parent_id, n_childs, MySqlScript.ToMySqlDateTimeString(add_time), description, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As family
                         Return DirectCast(MyClass.MemberwiseClone, family)
                     End Function
End Class


End Namespace
