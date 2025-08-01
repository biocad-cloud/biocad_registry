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
''' DROP TABLE IF EXISTS `odor`;
''' /*!40101 SET @saved_cs_client     = @@character_set_client */;
''' /*!50503 SET character_set_client = utf8mb4 */;
''' CREATE TABLE `odor` (
'''   `id` int unsigned NOT NULL AUTO_INCREMENT,
'''   `molecule_id` int unsigned NOT NULL COMMENT 'the metabolite molecule reference id',
'''   `category` int unsigned NOT NULL COMMENT 'odor category vocabulary term of this odor term',
'''   `odor` varchar(512) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL COMMENT 'the odor term',
'''   `hashcode` varchar(32) CHARACTER SET utf8mb3 COLLATE utf8mb3_bin NOT NULL COMMENT 'md5 hashcode index of the odor term',
'''   `value` double NOT NULL DEFAULT '0',
'''   `unit` int unsigned DEFAULT NULL,
'''   `text` text CHARACTER SET utf8mb3 COLLATE utf8mb3_bin NOT NULL COMMENT 'the raw text of the text mining of the odor term',
'''   `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
'''   PRIMARY KEY (`id`),
'''   UNIQUE KEY `id_UNIQUE` (`id`),
'''   KEY `odor_term_index` (`odor`),
'''   KEY `class_index` (`category`),
'''   KEY `mol_index` (`molecule_id`),
'''   KEY `check_odor` (`molecule_id`,`category`,`odor`),
'''   FULLTEXT KEY `search_text` (`text`)
''' ) ENGINE=InnoDB AUTO_INCREMENT=178128 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='odor information about the metabolite molecules';
''' /*!40101 SET character_set_client = @saved_cs_client */;
''' 
''' --
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("odor", Database:="cad_registry", SchemaSQL:="
CREATE TABLE `odor` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `molecule_id` int unsigned NOT NULL COMMENT 'the metabolite molecule reference id',
  `category` int unsigned NOT NULL COMMENT 'odor category vocabulary term of this odor term',
  `odor` varchar(512) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL COMMENT 'the odor term',
  `hashcode` varchar(32) CHARACTER SET utf8mb3 COLLATE utf8mb3_bin NOT NULL COMMENT 'md5 hashcode index of the odor term',
  `value` double NOT NULL DEFAULT '0',
  `unit` int unsigned DEFAULT NULL,
  `text` text CHARACTER SET utf8mb3 COLLATE utf8mb3_bin NOT NULL COMMENT 'the raw text of the text mining of the odor term',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `odor_term_index` (`odor`),
  KEY `class_index` (`category`),
  KEY `mol_index` (`molecule_id`),
  KEY `check_odor` (`molecule_id`,`category`,`odor`),
  FULLTEXT KEY `search_text` (`text`)
) ENGINE=InnoDB AUTO_INCREMENT=178128 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='odor information about the metabolite molecules';")>
Public Class odor: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
''' <summary>
''' the metabolite molecule reference id
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("molecule_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="molecule_id")> Public Property molecule_id As UInteger
''' <summary>
''' odor category vocabulary term of this odor term
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("category"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="category")> Public Property category As UInteger
''' <summary>
''' the odor term
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("odor"), DataType(MySqlDbType.VarChar, "512"), Column(Name:="odor")> Public Property odor As String
''' <summary>
''' md5 hashcode index of the odor term
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("hashcode"), NotNull, DataType(MySqlDbType.VarChar, "32"), Column(Name:="hashcode")> Public Property hashcode As String
    <DatabaseField("value"), NotNull, DataType(MySqlDbType.Double), Column(Name:="value")> Public Property value As Double
    <DatabaseField("unit"), DataType(MySqlDbType.UInt32, "11"), Column(Name:="unit")> Public Property unit As UInteger
''' <summary>
''' the raw text of the text mining of the odor term
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("text"), NotNull, DataType(MySqlDbType.Text), Column(Name:="text")> Public Property text As String
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `odor` (`molecule_id`, `category`, `odor`, `hashcode`, `value`, `unit`, `text`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `odor` (`id`, `molecule_id`, `category`, `odor`, `hashcode`, `value`, `unit`, `text`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `odor` (`molecule_id`, `category`, `odor`, `hashcode`, `value`, `unit`, `text`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `odor` (`id`, `molecule_id`, `category`, `odor`, `hashcode`, `value`, `unit`, `text`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `odor` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `odor` SET `id`='{0}', `molecule_id`='{1}', `category`='{2}', `odor`='{3}', `hashcode`='{4}', `value`='{5}', `unit`='{6}', `text`='{7}', `add_time`='{8}' WHERE `id` = '{9}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `odor` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `odor` (`id`, `molecule_id`, `category`, `odor`, `hashcode`, `value`, `unit`, `text`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, molecule_id, category, odor, hashcode, value, unit, text, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `odor` (`id`, `molecule_id`, `category`, `odor`, `hashcode`, `value`, `unit`, `text`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, molecule_id, category, odor, hashcode, value, unit, text, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(INSERT_SQL, molecule_id, category, odor, hashcode, value, unit, text, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{molecule_id}', '{category}', '{odor}', '{hashcode}', '{value}', '{unit}', '{text}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        Else
            Return $"('{molecule_id}', '{category}', '{odor}', '{hashcode}', '{value}', '{unit}', '{text}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `odor` (`id`, `molecule_id`, `category`, `odor`, `hashcode`, `value`, `unit`, `text`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, molecule_id, category, odor, hashcode, value, unit, text, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `odor` (`id`, `molecule_id`, `category`, `odor`, `hashcode`, `value`, `unit`, `text`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, molecule_id, category, odor, hashcode, value, unit, text, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(REPLACE_SQL, molecule_id, category, odor, hashcode, value, unit, text, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `odor` SET `id`='{0}', `molecule_id`='{1}', `category`='{2}', `odor`='{3}', `hashcode`='{4}', `value`='{5}', `unit`='{6}', `text`='{7}', `add_time`='{8}' WHERE `id` = '{9}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, molecule_id, category, odor, hashcode, value, unit, text, MySqlScript.ToMySqlDateTimeString(add_time), id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As odor
                         Return DirectCast(MyClass.MemberwiseClone, odor)
                     End Function
End Class


End Namespace
