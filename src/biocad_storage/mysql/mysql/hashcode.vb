REM  Oracle.LinuxCompatibility.MySQL.CodeSolution.VisualBasic.CodeGenerator
REM  MYSQL Schema Mapper
REM      for Microsoft VisualBasic.NET 1.0.0.0

REM  Dump @7/26/2025 9:16:16 PM


Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports MySqlScript = Oracle.LinuxCompatibility.MySQL.Scripting.Extensions

Namespace biocad_registryModel

''' <summary>
''' ```SQL
''' 
''' --
''' 
''' DROP TABLE IF EXISTS `hashcode`;
''' /*!40101 SET @saved_cs_client     = @@character_set_client */;
''' /*!50503 SET character_set_client = utf8mb4 */;
''' CREATE TABLE `hashcode` (
'''   `id` int unsigned NOT NULL AUTO_INCREMENT,
'''   `obj_id` int unsigned NOT NULL COMMENT 'cad registry entity, could be molecule, reaction,pathways,kinetic_law',
'''   `hashcode` char(32) CHARACTER SET utf8mb3 COLLATE utf8mb3_bin NOT NULL COMMENT 'md5 checksum of the entity',
'''   `type_id` int unsigned NOT NULL COMMENT 'the vocabulary term id of the entity object type',
'''   `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
'''   PRIMARY KEY (`id`),
'''   UNIQUE KEY `id_UNIQUE` (`id`),
'''   KEY `check_unique` (`hashcode`,`type_id`),
'''   KEY `sort_hash` (`hashcode`)
''' ) ENGINE=InnoDB AUTO_INCREMENT=70125 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='the union hashcode of the object entities, temp table for used for merge multiple duplictaed objects';
''' /*!40101 SET character_set_client = @saved_cs_client */;
''' 
''' --
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("hashcode", Database:="cad_registry", SchemaSQL:="
CREATE TABLE `hashcode` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `obj_id` int unsigned NOT NULL COMMENT 'cad registry entity, could be molecule, reaction,pathways,kinetic_law',
  `hashcode` char(32) CHARACTER SET utf8mb3 COLLATE utf8mb3_bin NOT NULL COMMENT 'md5 checksum of the entity',
  `type_id` int unsigned NOT NULL COMMENT 'the vocabulary term id of the entity object type',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `check_unique` (`hashcode`,`type_id`),
  KEY `sort_hash` (`hashcode`)
) ENGINE=InnoDB AUTO_INCREMENT=70125 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='the union hashcode of the object entities, temp table for used for merge multiple duplictaed objects';")>
Public Class hashcode: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
''' <summary>
''' cad registry entity, could be molecule, reaction,pathways,kinetic_law
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("obj_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="obj_id")> Public Property obj_id As UInteger
''' <summary>
''' md5 checksum of the entity
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("hashcode"), NotNull, DataType(MySqlDbType.VarChar, "32"), Column(Name:="hashcode")> Public Property hashcode As String
''' <summary>
''' the vocabulary term id of the entity object type
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("type_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="type_id")> Public Property type_id As UInteger
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `hashcode` (`obj_id`, `hashcode`, `type_id`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `hashcode` (`id`, `obj_id`, `hashcode`, `type_id`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `hashcode` (`obj_id`, `hashcode`, `type_id`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `hashcode` (`id`, `obj_id`, `hashcode`, `type_id`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `hashcode` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `hashcode` SET `id`='{0}', `obj_id`='{1}', `hashcode`='{2}', `type_id`='{3}', `add_time`='{4}' WHERE `id` = '{5}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `hashcode` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `hashcode` (`id`, `obj_id`, `hashcode`, `type_id`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, obj_id, hashcode, type_id, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `hashcode` (`id`, `obj_id`, `hashcode`, `type_id`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, obj_id, hashcode, type_id, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(INSERT_SQL, obj_id, hashcode, type_id, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{obj_id}', '{hashcode}', '{type_id}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        Else
            Return $"('{obj_id}', '{hashcode}', '{type_id}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `hashcode` (`id`, `obj_id`, `hashcode`, `type_id`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, obj_id, hashcode, type_id, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `hashcode` (`id`, `obj_id`, `hashcode`, `type_id`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, obj_id, hashcode, type_id, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(REPLACE_SQL, obj_id, hashcode, type_id, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `hashcode` SET `id`='{0}', `obj_id`='{1}', `hashcode`='{2}', `type_id`='{3}', `add_time`='{4}' WHERE `id` = '{5}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, obj_id, hashcode, type_id, MySqlScript.ToMySqlDateTimeString(add_time), id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As hashcode
                         Return DirectCast(MyClass.MemberwiseClone, hashcode)
                     End Function
End Class


End Namespace
