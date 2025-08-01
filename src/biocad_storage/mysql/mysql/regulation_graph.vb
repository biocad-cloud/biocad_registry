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
''' DROP TABLE IF EXISTS `regulation_graph`;
''' /*!40101 SET @saved_cs_client     = @@character_set_client */;
''' /*!50503 SET character_set_client = utf8mb4 */;
''' CREATE TABLE `regulation_graph` (
'''   `id` int unsigned NOT NULL AUTO_INCREMENT,
'''   `term` varchar(128) COLLATE utf8mb3_bin NOT NULL COMMENT 'usually be the EC number for general model or molecule object reference name',
'''   `role` int unsigned NOT NULL COMMENT 'the regulation role type of the entity object, for ec_number this could be the enzymatic catalysis function, for other small molecule metabolite, this function may be the activity suppression',
'''   `reaction_id` int unsigned NOT NULL COMMENT 'the id reference to the reaction table',
'''   `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
'''   `note` longtext CHARACTER SET utf8mb3 COLLATE utf8mb3_bin,
'''   PRIMARY KEY (`id`,`term`),
'''   UNIQUE KEY `id_UNIQUE` (`id`) /*!80000 INVISIBLE */,
'''   KEY `role_term_idx` (`role`),
'''   KEY `reaction_process_idx` (`reaction_id`),
'''   KEY `search_term` (`term`)
''' ) ENGINE=InnoDB AUTO_INCREMENT=73464 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='the enzyme associates with the reactions';
''' /*!40101 SET character_set_client = @saved_cs_client */;
''' 
''' --
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("regulation_graph", Database:="cad_registry", SchemaSQL:="
CREATE TABLE `regulation_graph` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `term` varchar(128) COLLATE utf8mb3_bin NOT NULL COMMENT 'usually be the EC number for general model or molecule object reference name',
  `role` int unsigned NOT NULL COMMENT 'the regulation role type of the entity object, for ec_number this could be the enzymatic catalysis function, for other small molecule metabolite, this function may be the activity suppression',
  `reaction_id` int unsigned NOT NULL COMMENT 'the id reference to the reaction table',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext CHARACTER SET utf8mb3 COLLATE utf8mb3_bin,
  PRIMARY KEY (`id`,`term`),
  UNIQUE KEY `id_UNIQUE` (`id`) /*!80000 INVISIBLE */,
  KEY `role_term_idx` (`role`),
  KEY `reaction_process_idx` (`reaction_id`),
  KEY `search_term` (`term`)
) ENGINE=InnoDB AUTO_INCREMENT=73464 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='the enzyme associates with the reactions';")>
Public Class regulation_graph: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
''' <summary>
''' usually be the EC number for general model or molecule object reference name
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("term"), PrimaryKey, NotNull, DataType(MySqlDbType.VarChar, "128"), Column(Name:="term"), XmlAttribute> Public Property term As String
''' <summary>
''' the regulation role type of the entity object, for ec_number this could be the enzymatic catalysis function, for other small molecule metabolite, this function may be the activity suppression
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("role"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="role")> Public Property role As UInteger
''' <summary>
''' the id reference to the reaction table
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("reaction_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="reaction_id")> Public Property reaction_id As UInteger
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
    <DatabaseField("note"), DataType(MySqlDbType.Text), Column(Name:="note")> Public Property note As String
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `regulation_graph` (`term`, `role`, `reaction_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `regulation_graph` (`id`, `term`, `role`, `reaction_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `regulation_graph` (`term`, `role`, `reaction_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `regulation_graph` (`id`, `term`, `role`, `reaction_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `regulation_graph` WHERE `id`='{0}' and `term`='{1}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `regulation_graph` SET `id`='{0}', `term`='{1}', `role`='{2}', `reaction_id`='{3}', `add_time`='{4}', `note`='{5}' WHERE `id`='{6}' and `term`='{7}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `regulation_graph` WHERE `id`='{0}' and `term`='{1}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id, term)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `regulation_graph` (`id`, `term`, `role`, `reaction_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, term, role, reaction_id, MySqlScript.ToMySqlDateTimeString(add_time), note)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `regulation_graph` (`id`, `term`, `role`, `reaction_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, term, role, reaction_id, MySqlScript.ToMySqlDateTimeString(add_time), note)
        Else
        Return String.Format(INSERT_SQL, term, role, reaction_id, MySqlScript.ToMySqlDateTimeString(add_time), note)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{term}', '{role}', '{reaction_id}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{note}')"
        Else
            Return $"('{term}', '{role}', '{reaction_id}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{note}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `regulation_graph` (`id`, `term`, `role`, `reaction_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, term, role, reaction_id, MySqlScript.ToMySqlDateTimeString(add_time), note)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `regulation_graph` (`id`, `term`, `role`, `reaction_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, term, role, reaction_id, MySqlScript.ToMySqlDateTimeString(add_time), note)
        Else
        Return String.Format(REPLACE_SQL, term, role, reaction_id, MySqlScript.ToMySqlDateTimeString(add_time), note)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `regulation_graph` SET `id`='{0}', `term`='{1}', `role`='{2}', `reaction_id`='{3}', `add_time`='{4}', `note`='{5}' WHERE `id`='{6}' and `term`='{7}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, term, role, reaction_id, MySqlScript.ToMySqlDateTimeString(add_time), note, id, term)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As regulation_graph
                         Return DirectCast(MyClass.MemberwiseClone, regulation_graph)
                     End Function
End Class


End Namespace
