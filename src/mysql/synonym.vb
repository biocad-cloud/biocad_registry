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
''' synonym names of the biocad registry object, example as: molecules, genomes, taxonomys, reactions, pathways
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("synonym", Database:="cad_registry", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `synonym` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `obj_id` INT UNSIGNED NOT NULL,
  `type_id` INT UNSIGNED NOT NULL COMMENT 'type reference of the target object, a vocabulary term reference to the vocabulary table',
  `hashcode` VARCHAR(32) NOT NULL COMMENT 'md5 hashcode of the synonym field lower case string ',
  `synonym` VARCHAR(4096) NOT NULL,
  `lang` VARCHAR(8) NOT NULL DEFAULT 'en',
  `add_time` DATETIME NOT NULL DEFAULT now(),
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE,
  INDEX `filter_type` (`type_id` ASC) INVISIBLE,
  INDEX `filter_obj` (`obj_id` ASC) VISIBLE)
ENGINE = InnoDB
COMMENT = 'synonym names of the biocad registry object, example as: molecules, genomes, taxonomys, reactions, pathways';
")>
Public Class synonym: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("obj_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="obj_id")> Public Property obj_id As UInteger
''' <summary>
''' type reference of the target object, a vocabulary term reference to the vocabulary table
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("type_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="type_id")> Public Property type_id As UInteger
''' <summary>
''' md5 hashcode of the synonym field lower case string 
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("hashcode"), NotNull, DataType(MySqlDbType.VarChar, "32"), Column(Name:="hashcode")> Public Property hashcode As String
    <DatabaseField("synonym"), NotNull, DataType(MySqlDbType.VarChar, "4096"), Column(Name:="synonym")> Public Property synonym As String
    <DatabaseField("lang"), NotNull, DataType(MySqlDbType.VarChar, "8"), Column(Name:="lang")> Public Property lang As String
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `synonym` (`obj_id`, `type_id`, `hashcode`, `synonym`, `lang`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `synonym` (`id`, `obj_id`, `type_id`, `hashcode`, `synonym`, `lang`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `synonym` (`obj_id`, `type_id`, `hashcode`, `synonym`, `lang`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `synonym` (`id`, `obj_id`, `type_id`, `hashcode`, `synonym`, `lang`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `synonym` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `synonym` SET `id`='{0}', `obj_id`='{1}', `type_id`='{2}', `hashcode`='{3}', `synonym`='{4}', `lang`='{5}', `add_time`='{6}' WHERE `id` = '{7}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `synonym` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `synonym` (`id`, `obj_id`, `type_id`, `hashcode`, `synonym`, `lang`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, obj_id, type_id, hashcode, synonym, lang, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `synonym` (`id`, `obj_id`, `type_id`, `hashcode`, `synonym`, `lang`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, obj_id, type_id, hashcode, synonym, lang, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(INSERT_SQL, obj_id, type_id, hashcode, synonym, lang, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{obj_id}', '{type_id}', '{hashcode}', '{synonym}', '{lang}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        Else
            Return $"('{obj_id}', '{type_id}', '{hashcode}', '{synonym}', '{lang}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `synonym` (`id`, `obj_id`, `type_id`, `hashcode`, `synonym`, `lang`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, obj_id, type_id, hashcode, synonym, lang, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `synonym` (`id`, `obj_id`, `type_id`, `hashcode`, `synonym`, `lang`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, obj_id, type_id, hashcode, synonym, lang, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(REPLACE_SQL, obj_id, type_id, hashcode, synonym, lang, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `synonym` SET `id`='{0}', `obj_id`='{1}', `type_id`='{2}', `hashcode`='{3}', `synonym`='{4}', `lang`='{5}', `add_time`='{6}' WHERE `id` = '{7}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, obj_id, type_id, hashcode, synonym, lang, MySqlScript.ToMySqlDateTimeString(add_time), id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As synonym
                         Return DirectCast(MyClass.MemberwiseClone, synonym)
                     End Function
End Class


End Namespace
