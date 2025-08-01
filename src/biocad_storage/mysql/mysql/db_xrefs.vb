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
''' DROP TABLE IF EXISTS `db_xrefs`;
''' /*!40101 SET @saved_cs_client     = @@character_set_client */;
''' /*!50503 SET character_set_client = utf8mb4 */;
''' CREATE TABLE `db_xrefs` (
'''   `id` int unsigned NOT NULL AUTO_INCREMENT,
'''   `obj_id` int unsigned NOT NULL,
'''   `db_key` int unsigned NOT NULL,
'''   `xref` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_bin NOT NULL,
'''   `type` int unsigned NOT NULL COMMENT 'the target type of obj_id that point to, could be molecules, reactions, pathways',
'''   `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
'''   PRIMARY KEY (`id`),
'''   UNIQUE KEY `id_UNIQUE` (`id`),
'''   UNIQUE KEY `unique_dblink` (`obj_id`,`db_key`,`xref`,`type`),
'''   KEY `molecule_id_idx` (`obj_id`),
'''   KEY `db_name_idx` (`db_key`),
'''   KEY `object_type_idx` (`type`),
'''   KEY `find_xrefs` (`type`,`xref`),
'''   KEY `dbid_index` (`xref`),
'''   KEY `search_dbxrefs` (`obj_id`,`db_key`,`xref`,`type`)
''' ) ENGINE=InnoDB AUTO_INCREMENT=23235678 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
''' /*!40101 SET character_set_client = @saved_cs_client */;
''' 
''' --
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("db_xrefs", Database:="cad_registry", SchemaSQL:="
CREATE TABLE `db_xrefs` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `obj_id` int unsigned NOT NULL,
  `db_key` int unsigned NOT NULL,
  `xref` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_bin NOT NULL,
  `type` int unsigned NOT NULL COMMENT 'the target type of obj_id that point to, could be molecules, reactions, pathways',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `unique_dblink` (`obj_id`,`db_key`,`xref`,`type`),
  KEY `molecule_id_idx` (`obj_id`),
  KEY `db_name_idx` (`db_key`),
  KEY `object_type_idx` (`type`),
  KEY `find_xrefs` (`type`,`xref`),
  KEY `dbid_index` (`xref`),
  KEY `search_dbxrefs` (`obj_id`,`db_key`,`xref`,`type`)
) ENGINE=InnoDB AUTO_INCREMENT=23235678 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;")>
Public Class db_xrefs: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("obj_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="obj_id")> Public Property obj_id As UInteger
    <DatabaseField("db_key"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="db_key")> Public Property db_key As UInteger
    <DatabaseField("xref"), NotNull, DataType(MySqlDbType.VarChar, "255"), Column(Name:="xref")> Public Property xref As String
''' <summary>
''' the target type of obj_id that point to, could be molecules, reactions, pathways
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("type"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="type")> Public Property type As UInteger
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `db_xrefs` (`obj_id`, `db_key`, `xref`, `type`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `db_xrefs` (`id`, `obj_id`, `db_key`, `xref`, `type`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `db_xrefs` (`obj_id`, `db_key`, `xref`, `type`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `db_xrefs` (`id`, `obj_id`, `db_key`, `xref`, `type`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `db_xrefs` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `db_xrefs` SET `id`='{0}', `obj_id`='{1}', `db_key`='{2}', `xref`='{3}', `type`='{4}', `add_time`='{5}' WHERE `id` = '{6}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `db_xrefs` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `db_xrefs` (`id`, `obj_id`, `db_key`, `xref`, `type`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, obj_id, db_key, xref, type, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `db_xrefs` (`id`, `obj_id`, `db_key`, `xref`, `type`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, obj_id, db_key, xref, type, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(INSERT_SQL, obj_id, db_key, xref, type, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{obj_id}', '{db_key}', '{xref}', '{type}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        Else
            Return $"('{obj_id}', '{db_key}', '{xref}', '{type}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `db_xrefs` (`id`, `obj_id`, `db_key`, `xref`, `type`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, obj_id, db_key, xref, type, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `db_xrefs` (`id`, `obj_id`, `db_key`, `xref`, `type`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, obj_id, db_key, xref, type, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(REPLACE_SQL, obj_id, db_key, xref, type, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `db_xrefs` SET `id`='{0}', `obj_id`='{1}', `db_key`='{2}', `xref`='{3}', `type`='{4}', `add_time`='{5}' WHERE `id` = '{6}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, obj_id, db_key, xref, type, MySqlScript.ToMySqlDateTimeString(add_time), id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As db_xrefs
                         Return DirectCast(MyClass.MemberwiseClone, db_xrefs)
                     End Function
End Class


End Namespace
