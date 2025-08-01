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
''' DROP TABLE IF EXISTS `reaction`;
''' /*!40101 SET @saved_cs_client     = @@character_set_client */;
''' /*!50503 SET character_set_client = utf8mb4 */;
''' CREATE TABLE `reaction` (
'''   `id` int unsigned NOT NULL AUTO_INCREMENT,
'''   `db_xref` varchar(64) COLLATE utf8mb3_bin NOT NULL COMMENT 'the reference id of this reaction model from the external source database',
'''   `source_dbkey` int unsigned NOT NULL COMMENT 'the source database name reference vocabulary term index',
'''   `hashcode` varchar(64) COLLATE utf8mb3_bin DEFAULT NULL COMMENT 'the unique indexed hash code for the reaction, rule for evaluate this hashcode:  mapping metabolite to biocad registry internal id, build mapped equation string, then cal md5 hashcode of the left and right and combine the hashcode string into one',
'''   `name` varchar(1024) CHARACTER SET utf8mb3 COLLATE utf8mb3_bin NOT NULL COMMENT 'name of current reaction model',
'''   `equation` mediumtext CHARACTER SET utf8mb3 COLLATE utf8mb3_bin NOT NULL COMMENT 'reaction equation of current reaction model',
'''   `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
'''   `note` longtext CHARACTER SET utf8mb3 COLLATE utf8mb3_bin,
'''   PRIMARY KEY (`id`),
'''   UNIQUE KEY `id_UNIQUE` (`id`),
'''   UNIQUE KEY `db_xref_UNIQUE` (`db_xref`),
'''   KEY `source_database_idx` (`source_dbkey`),
'''   KEY `hashindex` (`hashcode`),
'''   FULLTEXT KEY `search_text` (`name`,`note`)
''' ) ENGINE=InnoDB AUTO_INCREMENT=133887 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='the definition of the biological reaction process';
''' /*!40101 SET character_set_client = @saved_cs_client */;
''' 
''' --
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("reaction", Database:="cad_registry", SchemaSQL:="
CREATE TABLE `reaction` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `db_xref` varchar(64) COLLATE utf8mb3_bin NOT NULL COMMENT 'the reference id of this reaction model from the external source database',
  `source_dbkey` int unsigned NOT NULL COMMENT 'the source database name reference vocabulary term index',
  `hashcode` varchar(64) COLLATE utf8mb3_bin DEFAULT NULL COMMENT 'the unique indexed hash code for the reaction, rule for evaluate this hashcode:  mapping metabolite to biocad registry internal id, build mapped equation string, then cal md5 hashcode of the left and right and combine the hashcode string into one',
  `name` varchar(1024) CHARACTER SET utf8mb3 COLLATE utf8mb3_bin NOT NULL COMMENT 'name of current reaction model',
  `equation` mediumtext CHARACTER SET utf8mb3 COLLATE utf8mb3_bin NOT NULL COMMENT 'reaction equation of current reaction model',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext CHARACTER SET utf8mb3 COLLATE utf8mb3_bin,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `db_xref_UNIQUE` (`db_xref`),
  KEY `source_database_idx` (`source_dbkey`),
  KEY `hashindex` (`hashcode`),
  FULLTEXT KEY `search_text` (`name`,`note`)
) ENGINE=InnoDB AUTO_INCREMENT=133887 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='the definition of the biological reaction process';")>
Public Class reaction: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
''' <summary>
''' the reference id of this reaction model from the external source database
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("db_xref"), NotNull, DataType(MySqlDbType.VarChar, "64"), Column(Name:="db_xref")> Public Property db_xref As String
''' <summary>
''' the source database name reference vocabulary term index
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("source_dbkey"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="source_dbkey")> Public Property source_dbkey As UInteger
''' <summary>
''' the unique indexed hash code for the reaction, rule for evaluate this hashcode:  mapping metabolite to biocad registry internal id, build mapped equation string, then cal md5 hashcode of the left and right and combine the hashcode string into one
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("hashcode"), DataType(MySqlDbType.VarChar, "64"), Column(Name:="hashcode")> Public Property hashcode As String
''' <summary>
''' name of current reaction model
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("name"), NotNull, DataType(MySqlDbType.VarChar, "1024"), Column(Name:="name")> Public Property name As String
''' <summary>
''' reaction equation of current reaction model
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("equation"), NotNull, DataType(MySqlDbType.Text), Column(Name:="equation")> Public Property equation As String
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
    <DatabaseField("note"), DataType(MySqlDbType.Text), Column(Name:="note")> Public Property note As String
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `reaction` (`db_xref`, `source_dbkey`, `hashcode`, `name`, `equation`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `reaction` (`id`, `db_xref`, `source_dbkey`, `hashcode`, `name`, `equation`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `reaction` (`db_xref`, `source_dbkey`, `hashcode`, `name`, `equation`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `reaction` (`id`, `db_xref`, `source_dbkey`, `hashcode`, `name`, `equation`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `reaction` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `reaction` SET `id`='{0}', `db_xref`='{1}', `source_dbkey`='{2}', `hashcode`='{3}', `name`='{4}', `equation`='{5}', `add_time`='{6}', `note`='{7}' WHERE `id` = '{8}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `reaction` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `reaction` (`id`, `db_xref`, `source_dbkey`, `hashcode`, `name`, `equation`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, db_xref, source_dbkey, hashcode, name, equation, MySqlScript.ToMySqlDateTimeString(add_time), note)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `reaction` (`id`, `db_xref`, `source_dbkey`, `hashcode`, `name`, `equation`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, db_xref, source_dbkey, hashcode, name, equation, MySqlScript.ToMySqlDateTimeString(add_time), note)
        Else
        Return String.Format(INSERT_SQL, db_xref, source_dbkey, hashcode, name, equation, MySqlScript.ToMySqlDateTimeString(add_time), note)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{db_xref}', '{source_dbkey}', '{hashcode}', '{name}', '{equation}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{note}')"
        Else
            Return $"('{db_xref}', '{source_dbkey}', '{hashcode}', '{name}', '{equation}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{note}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `reaction` (`id`, `db_xref`, `source_dbkey`, `hashcode`, `name`, `equation`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, db_xref, source_dbkey, hashcode, name, equation, MySqlScript.ToMySqlDateTimeString(add_time), note)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `reaction` (`id`, `db_xref`, `source_dbkey`, `hashcode`, `name`, `equation`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, db_xref, source_dbkey, hashcode, name, equation, MySqlScript.ToMySqlDateTimeString(add_time), note)
        Else
        Return String.Format(REPLACE_SQL, db_xref, source_dbkey, hashcode, name, equation, MySqlScript.ToMySqlDateTimeString(add_time), note)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `reaction` SET `id`='{0}', `db_xref`='{1}', `source_dbkey`='{2}', `hashcode`='{3}', `name`='{4}', `equation`='{5}', `add_time`='{6}', `note`='{7}' WHERE `id` = '{8}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, db_xref, source_dbkey, hashcode, name, equation, MySqlScript.ToMySqlDateTimeString(add_time), note, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As reaction
                         Return DirectCast(MyClass.MemberwiseClone, reaction)
                     End Function
End Class


End Namespace
