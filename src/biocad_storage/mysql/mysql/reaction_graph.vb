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
''' DROP TABLE IF EXISTS `reaction_graph`;
''' /*!40101 SET @saved_cs_client     = @@character_set_client */;
''' /*!50503 SET character_set_client = utf8mb4 */;
''' CREATE TABLE `reaction_graph` (
'''   `id` int unsigned NOT NULL AUTO_INCREMENT,
'''   `reaction` int unsigned NOT NULL COMMENT 'the id of the reaction',
'''   `molecule_id` int unsigned NOT NULL COMMENT 'the id of the molecule entity',
'''   `db_xref` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_bin NOT NULL COMMENT 'the reaction source definition of the molecule to reference the molecule entity inside biocad registry, used for link the molecule entity with current relationship record ',
'''   `role` int unsigned NOT NULL COMMENT 'the vocabulary term of the molecule role inside this reaction model, usually be the substrate or product',
'''   `factor` double NOT NULL DEFAULT '1',
'''   `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
'''   `note` longtext CHARACTER SET utf8mb3 COLLATE utf8mb3_bin,
'''   PRIMARY KEY (`id`),
'''   UNIQUE KEY `id_UNIQUE` (`id`) /*!80000 INVISIBLE */,
'''   KEY `reaction_info_idx` (`reaction`),
'''   KEY `molecule_data_idx` (`molecule_id`),
'''   KEY `role_term_idx` (`role`),
'''   KEY `check_duplicated` (`reaction`,`db_xref`,`role`)
''' ) ENGINE=InnoDB AUTO_INCREMENT=466048 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='the relationship between the reaction model and molecule objects';
''' /*!40101 SET character_set_client = @saved_cs_client */;
''' 
''' --
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("reaction_graph", Database:="cad_registry", SchemaSQL:="
CREATE TABLE `reaction_graph` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `reaction` int unsigned NOT NULL COMMENT 'the id of the reaction',
  `molecule_id` int unsigned NOT NULL COMMENT 'the id of the molecule entity',
  `db_xref` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_bin NOT NULL COMMENT 'the reaction source definition of the molecule to reference the molecule entity inside biocad registry, used for link the molecule entity with current relationship record ',
  `role` int unsigned NOT NULL COMMENT 'the vocabulary term of the molecule role inside this reaction model, usually be the substrate or product',
  `factor` double NOT NULL DEFAULT '1',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext CHARACTER SET utf8mb3 COLLATE utf8mb3_bin,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`) /*!80000 INVISIBLE */,
  KEY `reaction_info_idx` (`reaction`),
  KEY `molecule_data_idx` (`molecule_id`),
  KEY `role_term_idx` (`role`),
  KEY `check_duplicated` (`reaction`,`db_xref`,`role`)
) ENGINE=InnoDB AUTO_INCREMENT=466048 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='the relationship between the reaction model and molecule objects';")>
Public Class reaction_graph: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
''' <summary>
''' the id of the reaction
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("reaction"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="reaction")> Public Property reaction As UInteger
''' <summary>
''' the id of the molecule entity
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("molecule_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="molecule_id")> Public Property molecule_id As UInteger
''' <summary>
''' the reaction source definition of the molecule to reference the molecule entity inside biocad registry, used for link the molecule entity with current relationship record 
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("db_xref"), NotNull, DataType(MySqlDbType.VarChar, "255"), Column(Name:="db_xref")> Public Property db_xref As String
''' <summary>
''' the vocabulary term of the molecule role inside this reaction model, usually be the substrate or product
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("role"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="role")> Public Property role As UInteger
    <DatabaseField("factor"), NotNull, DataType(MySqlDbType.Double), Column(Name:="factor")> Public Property factor As Double
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
    <DatabaseField("note"), DataType(MySqlDbType.Text), Column(Name:="note")> Public Property note As String
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `reaction_graph` (`reaction`, `molecule_id`, `db_xref`, `role`, `factor`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `reaction_graph` (`id`, `reaction`, `molecule_id`, `db_xref`, `role`, `factor`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `reaction_graph` (`reaction`, `molecule_id`, `db_xref`, `role`, `factor`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `reaction_graph` (`id`, `reaction`, `molecule_id`, `db_xref`, `role`, `factor`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `reaction_graph` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `reaction_graph` SET `id`='{0}', `reaction`='{1}', `molecule_id`='{2}', `db_xref`='{3}', `role`='{4}', `factor`='{5}', `add_time`='{6}', `note`='{7}' WHERE `id` = '{8}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `reaction_graph` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `reaction_graph` (`id`, `reaction`, `molecule_id`, `db_xref`, `role`, `factor`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, reaction, molecule_id, db_xref, role, factor, MySqlScript.ToMySqlDateTimeString(add_time), note)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `reaction_graph` (`id`, `reaction`, `molecule_id`, `db_xref`, `role`, `factor`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, reaction, molecule_id, db_xref, role, factor, MySqlScript.ToMySqlDateTimeString(add_time), note)
        Else
        Return String.Format(INSERT_SQL, reaction, molecule_id, db_xref, role, factor, MySqlScript.ToMySqlDateTimeString(add_time), note)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{reaction}', '{molecule_id}', '{db_xref}', '{role}', '{factor}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{note}')"
        Else
            Return $"('{reaction}', '{molecule_id}', '{db_xref}', '{role}', '{factor}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{note}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `reaction_graph` (`id`, `reaction`, `molecule_id`, `db_xref`, `role`, `factor`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, reaction, molecule_id, db_xref, role, factor, MySqlScript.ToMySqlDateTimeString(add_time), note)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `reaction_graph` (`id`, `reaction`, `molecule_id`, `db_xref`, `role`, `factor`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, reaction, molecule_id, db_xref, role, factor, MySqlScript.ToMySqlDateTimeString(add_time), note)
        Else
        Return String.Format(REPLACE_SQL, reaction, molecule_id, db_xref, role, factor, MySqlScript.ToMySqlDateTimeString(add_time), note)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `reaction_graph` SET `id`='{0}', `reaction`='{1}', `molecule_id`='{2}', `db_xref`='{3}', `role`='{4}', `factor`='{5}', `add_time`='{6}', `note`='{7}' WHERE `id` = '{8}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, reaction, molecule_id, db_xref, role, factor, MySqlScript.ToMySqlDateTimeString(add_time), note, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As reaction_graph
                         Return DirectCast(MyClass.MemberwiseClone, reaction_graph)
                     End Function
End Class


End Namespace
