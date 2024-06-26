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
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("vocabulary", Database:="cad_registry", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `vocabulary` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `term` VARCHAR(255) NOT NULL,
  `hashcode` VARCHAR(32) NOT NULL DEFAULT 'NA',
  `parent_id` INT UNSIGNED NOT NULL DEFAULT 0 COMMENT 'the id of current vocabulary term that could be classify to, the id of another vocabulary term',
  `add_time` DATETIME NOT NULL DEFAULT now(),
  `description` VARCHAR(8192) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;
")>
Public Class vocabulary: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("term"), NotNull, DataType(MySqlDbType.VarChar, "255"), Column(Name:="term")> Public Property term As String
    <DatabaseField("hashcode"), NotNull, DataType(MySqlDbType.VarChar, "32"), Column(Name:="hashcode")> Public Property hashcode As String
''' <summary>
''' the id of current vocabulary term that could be classify to, the id of another vocabulary term
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("parent_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="parent_id")> Public Property parent_id As UInteger
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
    <DatabaseField("description"), DataType(MySqlDbType.VarChar, "8192"), Column(Name:="description")> Public Property description As String
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `vocabulary` (`term`, `hashcode`, `parent_id`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `vocabulary` (`id`, `term`, `hashcode`, `parent_id`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `vocabulary` (`term`, `hashcode`, `parent_id`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `vocabulary` (`id`, `term`, `hashcode`, `parent_id`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `vocabulary` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `vocabulary` SET `id`='{0}', `term`='{1}', `hashcode`='{2}', `parent_id`='{3}', `add_time`='{4}', `description`='{5}' WHERE `id` = '{6}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `vocabulary` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `vocabulary` (`id`, `term`, `hashcode`, `parent_id`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, term, hashcode, parent_id, MySqlScript.ToMySqlDateTimeString(add_time), description)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `vocabulary` (`id`, `term`, `hashcode`, `parent_id`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, term, hashcode, parent_id, MySqlScript.ToMySqlDateTimeString(add_time), description)
        Else
        Return String.Format(INSERT_SQL, term, hashcode, parent_id, MySqlScript.ToMySqlDateTimeString(add_time), description)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{term}', '{hashcode}', '{parent_id}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{description}')"
        Else
            Return $"('{term}', '{hashcode}', '{parent_id}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{description}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `vocabulary` (`id`, `term`, `hashcode`, `parent_id`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, term, hashcode, parent_id, MySqlScript.ToMySqlDateTimeString(add_time), description)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `vocabulary` (`id`, `term`, `hashcode`, `parent_id`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, term, hashcode, parent_id, MySqlScript.ToMySqlDateTimeString(add_time), description)
        Else
        Return String.Format(REPLACE_SQL, term, hashcode, parent_id, MySqlScript.ToMySqlDateTimeString(add_time), description)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `vocabulary` SET `id`='{0}', `term`='{1}', `hashcode`='{2}', `parent_id`='{3}', `add_time`='{4}', `description`='{5}' WHERE `id` = '{6}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, term, hashcode, parent_id, MySqlScript.ToMySqlDateTimeString(add_time), description, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As vocabulary
                         Return DirectCast(MyClass.MemberwiseClone, vocabulary)
                     End Function
End Class


End Namespace
