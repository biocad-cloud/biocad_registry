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
''' DROP TABLE IF EXISTS `molecule_tags`;
''' /*!40101 SET @saved_cs_client     = @@character_set_client */;
''' /*!50503 SET character_set_client = utf8mb4 */;
''' CREATE TABLE `molecule_tags` (
'''   `id` int unsigned NOT NULL AUTO_INCREMENT,
'''   `tag_id` int unsigned NOT NULL COMMENT 'id reference to the vocabulary table, the topic tag about the molecule, example tags as: pathogenicity,toxic,drug',
'''   `molecule_id` int unsigned NOT NULL,
'''   `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
'''   `description` longtext CHARACTER SET utf8mb3 COLLATE utf8mb3_bin,
'''   PRIMARY KEY (`id`),
'''   UNIQUE KEY `id_UNIQUE` (`id`),
'''   KEY `check_label` (`tag_id`,`molecule_id`),
'''   KEY `filter_tag` (`tag_id`),
'''   KEY `filter_obj` (`molecule_id`)
''' ) ENGINE=InnoDB AUTO_INCREMENT=7351732 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='keywords, tags, labels of the molecule entity';
''' /*!40101 SET character_set_client = @saved_cs_client */;
''' 
''' --
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("molecule_tags", Database:="cad_registry", SchemaSQL:="
CREATE TABLE `molecule_tags` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `tag_id` int unsigned NOT NULL COMMENT 'id reference to the vocabulary table, the topic tag about the molecule, example tags as: pathogenicity,toxic,drug',
  `molecule_id` int unsigned NOT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `description` longtext CHARACTER SET utf8mb3 COLLATE utf8mb3_bin,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `check_label` (`tag_id`,`molecule_id`),
  KEY `filter_tag` (`tag_id`),
  KEY `filter_obj` (`molecule_id`)
) ENGINE=InnoDB AUTO_INCREMENT=7351732 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='keywords, tags, labels of the molecule entity';")>
Public Class molecule_tags: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
''' <summary>
''' id reference to the vocabulary table, the topic tag about the molecule, example tags as: pathogenicity,toxic,drug
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("tag_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="tag_id")> Public Property tag_id As UInteger
    <DatabaseField("molecule_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="molecule_id")> Public Property molecule_id As UInteger
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
    <DatabaseField("description"), DataType(MySqlDbType.Text), Column(Name:="description")> Public Property description As String
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `molecule_tags` (`tag_id`, `molecule_id`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `molecule_tags` (`id`, `tag_id`, `molecule_id`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `molecule_tags` (`tag_id`, `molecule_id`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `molecule_tags` (`id`, `tag_id`, `molecule_id`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `molecule_tags` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `molecule_tags` SET `id`='{0}', `tag_id`='{1}', `molecule_id`='{2}', `add_time`='{3}', `description`='{4}' WHERE `id` = '{5}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `molecule_tags` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `molecule_tags` (`id`, `tag_id`, `molecule_id`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, tag_id, molecule_id, MySqlScript.ToMySqlDateTimeString(add_time), description)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `molecule_tags` (`id`, `tag_id`, `molecule_id`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, tag_id, molecule_id, MySqlScript.ToMySqlDateTimeString(add_time), description)
        Else
        Return String.Format(INSERT_SQL, tag_id, molecule_id, MySqlScript.ToMySqlDateTimeString(add_time), description)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{tag_id}', '{molecule_id}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{description}')"
        Else
            Return $"('{tag_id}', '{molecule_id}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{description}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `molecule_tags` (`id`, `tag_id`, `molecule_id`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, tag_id, molecule_id, MySqlScript.ToMySqlDateTimeString(add_time), description)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `molecule_tags` (`id`, `tag_id`, `molecule_id`, `add_time`, `description`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, tag_id, molecule_id, MySqlScript.ToMySqlDateTimeString(add_time), description)
        Else
        Return String.Format(REPLACE_SQL, tag_id, molecule_id, MySqlScript.ToMySqlDateTimeString(add_time), description)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `molecule_tags` SET `id`='{0}', `tag_id`='{1}', `molecule_id`='{2}', `add_time`='{3}', `description`='{4}' WHERE `id` = '{5}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, tag_id, molecule_id, MySqlScript.ToMySqlDateTimeString(add_time), description, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As molecule_tags
                         Return DirectCast(MyClass.MemberwiseClone, molecule_tags)
                     End Function
End Class


End Namespace
