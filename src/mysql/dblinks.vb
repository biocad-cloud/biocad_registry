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
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("dblinks", Database:="cad_registry", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `dblinks` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `db_src` INT UNSIGNED NOT NULL COMMENT 'the external database',
  `xref_id` VARCHAR(255) NOT NULL COMMENT 'the external database id of current entity',
  `entity_id` INT UNSIGNED NOT NULL COMMENT 'the biocad internal registry id of the entity object reference to',
  `entity_type` INT UNSIGNED NOT NULL COMMENT 'the data type of the current entity object\n\n1: molecules\n2: reactions\n3: gene ontology term\n4: compartment term',
  `add_time` DATETIME NOT NULL DEFAULT now(),
  PRIMARY KEY (`id`))
ENGINE = InnoDB;
")>
Public Class dblinks: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
''' <summary>
''' the external database
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("db_src"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="db_src")> Public Property db_src As UInteger
''' <summary>
''' the external database id of current entity
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("xref_id"), NotNull, DataType(MySqlDbType.VarChar, "255"), Column(Name:="xref_id")> Public Property xref_id As String
''' <summary>
''' the biocad internal registry id of the entity object reference to
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("entity_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="entity_id")> Public Property entity_id As UInteger
''' <summary>
''' the data type of the current entity object\n\n1: molecules\n2: reactions\n3: gene ontology term\n4: compartment term
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("entity_type"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="entity_type")> Public Property entity_type As UInteger
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `dblinks` (`db_src`, `xref_id`, `entity_id`, `entity_type`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `dblinks` (`id`, `db_src`, `xref_id`, `entity_id`, `entity_type`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `dblinks` (`db_src`, `xref_id`, `entity_id`, `entity_type`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `dblinks` (`id`, `db_src`, `xref_id`, `entity_id`, `entity_type`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `dblinks` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `dblinks` SET `id`='{0}', `db_src`='{1}', `xref_id`='{2}', `entity_id`='{3}', `entity_type`='{4}', `add_time`='{5}' WHERE `id` = '{6}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `dblinks` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `dblinks` (`id`, `db_src`, `xref_id`, `entity_id`, `entity_type`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, db_src, xref_id, entity_id, entity_type, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `dblinks` (`id`, `db_src`, `xref_id`, `entity_id`, `entity_type`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, db_src, xref_id, entity_id, entity_type, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(INSERT_SQL, db_src, xref_id, entity_id, entity_type, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{db_src}', '{xref_id}', '{entity_id}', '{entity_type}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        Else
            Return $"('{db_src}', '{xref_id}', '{entity_id}', '{entity_type}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `dblinks` (`id`, `db_src`, `xref_id`, `entity_id`, `entity_type`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, db_src, xref_id, entity_id, entity_type, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `dblinks` (`id`, `db_src`, `xref_id`, `entity_id`, `entity_type`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, db_src, xref_id, entity_id, entity_type, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(REPLACE_SQL, db_src, xref_id, entity_id, entity_type, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `dblinks` SET `id`='{0}', `db_src`='{1}', `xref_id`='{2}', `entity_id`='{3}', `entity_type`='{4}', `add_time`='{5}' WHERE `id` = '{6}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, db_src, xref_id, entity_id, entity_type, MySqlScript.ToMySqlDateTimeString(add_time), id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As dblinks
                         Return DirectCast(MyClass.MemberwiseClone, dblinks)
                     End Function
End Class


End Namespace
