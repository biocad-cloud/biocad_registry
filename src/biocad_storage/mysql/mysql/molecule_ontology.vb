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
''' DROP TABLE IF EXISTS `molecule_ontology`;
''' /*!40101 SET @saved_cs_client     = @@character_set_client */;
''' /*!50503 SET character_set_client = utf8mb4 */;
''' CREATE TABLE `molecule_ontology` (
'''   `id` int unsigned NOT NULL AUTO_INCREMENT,
'''   `molecule_id` int unsigned NOT NULL,
'''   `ontology_id` int unsigned NOT NULL,
'''   `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
'''   `evidence` varchar(8192) COLLATE utf8mb3_bin DEFAULT NULL,
'''   PRIMARY KEY (`id`),
'''   UNIQUE KEY `id_UNIQUE` (`id`),
'''   KEY `molecule_obj_idx` (`molecule_id`),
'''   KEY `ontology_term_idx` (`ontology_id`)
''' ) ENGINE=InnoDB AUTO_INCREMENT=2191769 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;
''' /*!40101 SET character_set_client = @saved_cs_client */;
''' 
''' --
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("molecule_ontology", Database:="cad_registry", SchemaSQL:="
CREATE TABLE `molecule_ontology` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `molecule_id` int unsigned NOT NULL,
  `ontology_id` int unsigned NOT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `evidence` varchar(8192) COLLATE utf8mb3_bin DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `molecule_obj_idx` (`molecule_id`),
  KEY `ontology_term_idx` (`ontology_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2191769 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;")>
Public Class molecule_ontology: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("molecule_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="molecule_id")> Public Property molecule_id As UInteger
    <DatabaseField("ontology_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="ontology_id")> Public Property ontology_id As UInteger
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
    <DatabaseField("evidence"), DataType(MySqlDbType.VarChar, "8192"), Column(Name:="evidence")> Public Property evidence As String
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `molecule_ontology` (`molecule_id`, `ontology_id`, `add_time`, `evidence`) VALUES ('{0}', '{1}', '{2}', '{3}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `molecule_ontology` (`id`, `molecule_id`, `ontology_id`, `add_time`, `evidence`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `molecule_ontology` (`molecule_id`, `ontology_id`, `add_time`, `evidence`) VALUES ('{0}', '{1}', '{2}', '{3}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `molecule_ontology` (`id`, `molecule_id`, `ontology_id`, `add_time`, `evidence`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `molecule_ontology` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `molecule_ontology` SET `id`='{0}', `molecule_id`='{1}', `ontology_id`='{2}', `add_time`='{3}', `evidence`='{4}' WHERE `id` = '{5}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `molecule_ontology` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `molecule_ontology` (`id`, `molecule_id`, `ontology_id`, `add_time`, `evidence`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, molecule_id, ontology_id, MySqlScript.ToMySqlDateTimeString(add_time), evidence)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `molecule_ontology` (`id`, `molecule_id`, `ontology_id`, `add_time`, `evidence`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, molecule_id, ontology_id, MySqlScript.ToMySqlDateTimeString(add_time), evidence)
        Else
        Return String.Format(INSERT_SQL, molecule_id, ontology_id, MySqlScript.ToMySqlDateTimeString(add_time), evidence)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{molecule_id}', '{ontology_id}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{evidence}')"
        Else
            Return $"('{molecule_id}', '{ontology_id}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{evidence}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `molecule_ontology` (`id`, `molecule_id`, `ontology_id`, `add_time`, `evidence`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, molecule_id, ontology_id, MySqlScript.ToMySqlDateTimeString(add_time), evidence)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `molecule_ontology` (`id`, `molecule_id`, `ontology_id`, `add_time`, `evidence`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, molecule_id, ontology_id, MySqlScript.ToMySqlDateTimeString(add_time), evidence)
        Else
        Return String.Format(REPLACE_SQL, molecule_id, ontology_id, MySqlScript.ToMySqlDateTimeString(add_time), evidence)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `molecule_ontology` SET `id`='{0}', `molecule_id`='{1}', `ontology_id`='{2}', `add_time`='{3}', `evidence`='{4}' WHERE `id` = '{5}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, molecule_id, ontology_id, MySqlScript.ToMySqlDateTimeString(add_time), evidence, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As molecule_ontology
                         Return DirectCast(MyClass.MemberwiseClone, molecule_ontology)
                     End Function
End Class


End Namespace
