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
''' DROP TABLE IF EXISTS `kinetic_substrate`;
''' /*!40101 SET @saved_cs_client     = @@character_set_client */;
''' /*!50503 SET character_set_client = utf8mb4 */;
''' CREATE TABLE `kinetic_substrate` (
'''   `id` int unsigned NOT NULL AUTO_INCREMENT,
'''   `kinetic_id` int unsigned NOT NULL,
'''   `metabolite_id` int unsigned NOT NULL,
'''   `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
'''   PRIMARY KEY (`id`),
'''   UNIQUE KEY `id_UNIQUE` (`id`),
'''   KEY `check_link` (`kinetic_id`,`metabolite_id`)
''' ) ENGINE=InnoDB AUTO_INCREMENT=141797 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='the link to the kinetic reaction substrate metabolite molecules';
''' /*!40101 SET character_set_client = @saved_cs_client */;
''' 
''' --
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("kinetic_substrate", Database:="cad_registry", SchemaSQL:="
CREATE TABLE `kinetic_substrate` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `kinetic_id` int unsigned NOT NULL,
  `metabolite_id` int unsigned NOT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `check_link` (`kinetic_id`,`metabolite_id`)
) ENGINE=InnoDB AUTO_INCREMENT=141797 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='the link to the kinetic reaction substrate metabolite molecules';")>
Public Class kinetic_substrate: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("kinetic_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="kinetic_id")> Public Property kinetic_id As UInteger
    <DatabaseField("metabolite_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="metabolite_id")> Public Property metabolite_id As UInteger
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `kinetic_substrate` (`kinetic_id`, `metabolite_id`, `add_time`) VALUES ('{0}', '{1}', '{2}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `kinetic_substrate` (`id`, `kinetic_id`, `metabolite_id`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `kinetic_substrate` (`kinetic_id`, `metabolite_id`, `add_time`) VALUES ('{0}', '{1}', '{2}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `kinetic_substrate` (`id`, `kinetic_id`, `metabolite_id`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `kinetic_substrate` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `kinetic_substrate` SET `id`='{0}', `kinetic_id`='{1}', `metabolite_id`='{2}', `add_time`='{3}' WHERE `id` = '{4}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `kinetic_substrate` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `kinetic_substrate` (`id`, `kinetic_id`, `metabolite_id`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, kinetic_id, metabolite_id, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `kinetic_substrate` (`id`, `kinetic_id`, `metabolite_id`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, kinetic_id, metabolite_id, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(INSERT_SQL, kinetic_id, metabolite_id, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{kinetic_id}', '{metabolite_id}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        Else
            Return $"('{kinetic_id}', '{metabolite_id}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `kinetic_substrate` (`id`, `kinetic_id`, `metabolite_id`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, kinetic_id, metabolite_id, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `kinetic_substrate` (`id`, `kinetic_id`, `metabolite_id`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, kinetic_id, metabolite_id, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(REPLACE_SQL, kinetic_id, metabolite_id, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `kinetic_substrate` SET `id`='{0}', `kinetic_id`='{1}', `metabolite_id`='{2}', `add_time`='{3}' WHERE `id` = '{4}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, kinetic_id, metabolite_id, MySqlScript.ToMySqlDateTimeString(add_time), id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As kinetic_substrate
                         Return DirectCast(MyClass.MemberwiseClone, kinetic_substrate)
                     End Function
End Class


End Namespace
