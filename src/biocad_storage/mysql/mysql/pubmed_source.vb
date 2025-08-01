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
''' DROP TABLE IF EXISTS `pubmed_source`;
''' /*!40101 SET @saved_cs_client     = @@character_set_client */;
''' /*!50503 SET character_set_client = utf8mb4 */;
''' CREATE TABLE `pubmed_source` (
'''   `id` int unsigned NOT NULL AUTO_INCREMENT,
'''   `molecule_id` int unsigned NOT NULL,
'''   `pubmed_id` int unsigned NOT NULL,
'''   `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
'''   `note` mediumtext COLLATE utf8mb3_bin,
'''   PRIMARY KEY (`id`),
'''   UNIQUE KEY `unique_check` (`molecule_id`,`pubmed_id`),
'''   KEY `pubmed_data_idx` (`pubmed_id`),
'''   KEY `molecule_data_idx` (`molecule_id`)
''' ) ENGINE=InnoDB AUTO_INCREMENT=4174584 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;
''' /*!40101 SET character_set_client = @saved_cs_client */;
''' 
''' --
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("pubmed_source", Database:="cad_registry", SchemaSQL:="
CREATE TABLE `pubmed_source` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `molecule_id` int unsigned NOT NULL,
  `pubmed_id` int unsigned NOT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` mediumtext COLLATE utf8mb3_bin,
  PRIMARY KEY (`id`),
  UNIQUE KEY `unique_check` (`molecule_id`,`pubmed_id`),
  KEY `pubmed_data_idx` (`pubmed_id`),
  KEY `molecule_data_idx` (`molecule_id`)
) ENGINE=InnoDB AUTO_INCREMENT=4174584 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;")>
Public Class pubmed_source: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("molecule_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="molecule_id")> Public Property molecule_id As UInteger
    <DatabaseField("pubmed_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="pubmed_id")> Public Property pubmed_id As UInteger
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
    <DatabaseField("note"), DataType(MySqlDbType.Text), Column(Name:="note")> Public Property note As String
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `pubmed_source` (`molecule_id`, `pubmed_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `pubmed_source` (`id`, `molecule_id`, `pubmed_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `pubmed_source` (`molecule_id`, `pubmed_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `pubmed_source` (`id`, `molecule_id`, `pubmed_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `pubmed_source` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `pubmed_source` SET `id`='{0}', `molecule_id`='{1}', `pubmed_id`='{2}', `add_time`='{3}', `note`='{4}' WHERE `id` = '{5}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `pubmed_source` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `pubmed_source` (`id`, `molecule_id`, `pubmed_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, molecule_id, pubmed_id, MySqlScript.ToMySqlDateTimeString(add_time), note)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `pubmed_source` (`id`, `molecule_id`, `pubmed_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, molecule_id, pubmed_id, MySqlScript.ToMySqlDateTimeString(add_time), note)
        Else
        Return String.Format(INSERT_SQL, molecule_id, pubmed_id, MySqlScript.ToMySqlDateTimeString(add_time), note)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{molecule_id}', '{pubmed_id}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{note}')"
        Else
            Return $"('{molecule_id}', '{pubmed_id}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{note}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `pubmed_source` (`id`, `molecule_id`, `pubmed_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, molecule_id, pubmed_id, MySqlScript.ToMySqlDateTimeString(add_time), note)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `pubmed_source` (`id`, `molecule_id`, `pubmed_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, molecule_id, pubmed_id, MySqlScript.ToMySqlDateTimeString(add_time), note)
        Else
        Return String.Format(REPLACE_SQL, molecule_id, pubmed_id, MySqlScript.ToMySqlDateTimeString(add_time), note)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `pubmed_source` SET `id`='{0}', `molecule_id`='{1}', `pubmed_id`='{2}', `add_time`='{3}', `note`='{4}' WHERE `id` = '{5}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, molecule_id, pubmed_id, MySqlScript.ToMySqlDateTimeString(add_time), note, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As pubmed_source
                         Return DirectCast(MyClass.MemberwiseClone, pubmed_source)
                     End Function
End Class


End Namespace
