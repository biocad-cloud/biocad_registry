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
''' DROP TABLE IF EXISTS `subcellular_location`;
''' /*!40101 SET @saved_cs_client     = @@character_set_client */;
''' /*!50503 SET character_set_client = utf8mb4 */;
''' CREATE TABLE `subcellular_location` (
'''   `id` int unsigned NOT NULL AUTO_INCREMENT,
'''   `compartment_id` int unsigned NOT NULL,
'''   `obj_id` int unsigned NOT NULL,
'''   `entity` int unsigned NOT NULL COMMENT 'the vocabulary type id of the entity object, could be molecule, reaction or pathways',
'''   `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
'''   `note` longtext CHARACTER SET utf8mb3 COLLATE utf8mb3_bin,
'''   PRIMARY KEY (`id`),
'''   UNIQUE KEY `id_UNIQUE` (`id`),
'''   UNIQUE KEY `unique_reference` (`compartment_id`,`obj_id`,`entity`) /*!80000 INVISIBLE */,
'''   KEY `subcellular_compartments_idx` (`compartment_id`),
'''   KEY `molecule_obj_idx` (`obj_id`),
'''   KEY `link_entity_type_idx` (`entity`)
''' ) ENGINE=InnoDB AUTO_INCREMENT=867305 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='associates the subcellular_compartments and the molecule objects';
''' /*!40101 SET character_set_client = @saved_cs_client */;
''' 
''' --
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("subcellular_location", Database:="cad_registry", SchemaSQL:="
CREATE TABLE `subcellular_location` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `compartment_id` int unsigned NOT NULL,
  `obj_id` int unsigned NOT NULL,
  `entity` int unsigned NOT NULL COMMENT 'the vocabulary type id of the entity object, could be molecule, reaction or pathways',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext CHARACTER SET utf8mb3 COLLATE utf8mb3_bin,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `unique_reference` (`compartment_id`,`obj_id`,`entity`) /*!80000 INVISIBLE */,
  KEY `subcellular_compartments_idx` (`compartment_id`),
  KEY `molecule_obj_idx` (`obj_id`),
  KEY `link_entity_type_idx` (`entity`)
) ENGINE=InnoDB AUTO_INCREMENT=867305 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='associates the subcellular_compartments and the molecule objects';")>
Public Class subcellular_location: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("compartment_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="compartment_id")> Public Property compartment_id As UInteger
    <DatabaseField("obj_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="obj_id")> Public Property obj_id As UInteger
''' <summary>
''' the vocabulary type id of the entity object, could be molecule, reaction or pathways
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("entity"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="entity")> Public Property entity As UInteger
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
    <DatabaseField("note"), DataType(MySqlDbType.Text), Column(Name:="note")> Public Property note As String
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `subcellular_location` (`compartment_id`, `obj_id`, `entity`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `subcellular_location` (`id`, `compartment_id`, `obj_id`, `entity`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `subcellular_location` (`compartment_id`, `obj_id`, `entity`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `subcellular_location` (`id`, `compartment_id`, `obj_id`, `entity`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `subcellular_location` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `subcellular_location` SET `id`='{0}', `compartment_id`='{1}', `obj_id`='{2}', `entity`='{3}', `add_time`='{4}', `note`='{5}' WHERE `id` = '{6}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `subcellular_location` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `subcellular_location` (`id`, `compartment_id`, `obj_id`, `entity`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, compartment_id, obj_id, entity, MySqlScript.ToMySqlDateTimeString(add_time), note)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `subcellular_location` (`id`, `compartment_id`, `obj_id`, `entity`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, compartment_id, obj_id, entity, MySqlScript.ToMySqlDateTimeString(add_time), note)
        Else
        Return String.Format(INSERT_SQL, compartment_id, obj_id, entity, MySqlScript.ToMySqlDateTimeString(add_time), note)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{compartment_id}', '{obj_id}', '{entity}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{note}')"
        Else
            Return $"('{compartment_id}', '{obj_id}', '{entity}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{note}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `subcellular_location` (`id`, `compartment_id`, `obj_id`, `entity`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, compartment_id, obj_id, entity, MySqlScript.ToMySqlDateTimeString(add_time), note)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `subcellular_location` (`id`, `compartment_id`, `obj_id`, `entity`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, compartment_id, obj_id, entity, MySqlScript.ToMySqlDateTimeString(add_time), note)
        Else
        Return String.Format(REPLACE_SQL, compartment_id, obj_id, entity, MySqlScript.ToMySqlDateTimeString(add_time), note)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `subcellular_location` SET `id`='{0}', `compartment_id`='{1}', `obj_id`='{2}', `entity`='{3}', `add_time`='{4}', `note`='{5}' WHERE `id` = '{6}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, compartment_id, obj_id, entity, MySqlScript.ToMySqlDateTimeString(add_time), note, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As subcellular_location
                         Return DirectCast(MyClass.MemberwiseClone, subcellular_location)
                     End Function
End Class


End Namespace
