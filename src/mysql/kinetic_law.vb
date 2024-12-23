REM  Oracle.LinuxCompatibility.MySQL.CodeSolution.VisualBasic.CodeGenerator
REM  MYSQL Schema Mapper
REM      for Microsoft VisualBasic.NET 1.0.0.0

REM  Dump @12/23/2024 6:51:58 PM


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
''' DROP TABLE IF EXISTS `kinetic_law`;
''' /*!40101 SET @saved_cs_client     = @@character_set_client */;
''' /*!50503 SET character_set_client = utf8mb4 */;
''' CREATE TABLE `kinetic_law` (
'''   `id` int unsigned NOT NULL AUTO_INCREMENT,
'''   `db_xref` varchar(64) COLLATE utf8mb3_bin NOT NULL COMMENT 'the external reference id of current kinetics lambda model',
'''   `lambda` varchar(1024) COLLATE utf8mb3_bin NOT NULL COMMENT 'the lambda expression of the kinetics',
'''   `params` varchar(1024) CHARACTER SET utf8mb3 COLLATE utf8mb3_bin NOT NULL COMMENT 'parameter set of the current kinetics lambda epxression, in json string format',
'''   `temperature` double NOT NULL DEFAULT '37' COMMENT 'temperature of the enzyme catalytic kinetics',
'''   `pH` double unsigned NOT NULL DEFAULT '7.5' COMMENT 'pH of the enzyme catalytic kinetics',
'''   `substrate_id` int unsigned NOT NULL COMMENT 'id reference to the metabolite molecule',
'''   `uniprot` varchar(45) COLLATE utf8mb3_bin NOT NULL COMMENT 'the uniprot id of the current enzyme model, the kinetics parameter is associated with a specific molecule instance',
'''   `function_id` int unsigned NOT NULL COMMENT 'the internal reference id of the molecule function record, usually link to the ec_number of current kineticis',
'''   `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
'''   `note` longtext COLLATE utf8mb3_bin COMMENT 'description note text about current enzyme kinetics lambda model',
'''   PRIMARY KEY (`id`),
'''   UNIQUE KEY `id_UNIQUE` (`id`),
'''   KEY `regulation_id_idx` (`function_id`) /*!80000 INVISIBLE */,
'''   KEY `xrefs_index` (`db_xref`) /*!80000 INVISIBLE */,
'''   KEY `ph_filter` (`pH`) /*!80000 INVISIBLE */,
'''   KEY `temperature_filter` (`temperature`),
'''   KEY `uniprot_index` (`uniprot`)
''' ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='the enzymatic catalytic kinetics lambda model';
''' /*!40101 SET character_set_client = @saved_cs_client */;
''' 
''' --
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("kinetic_law", Database:="cad_registry", SchemaSQL:="
CREATE TABLE `kinetic_law` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `db_xref` varchar(64) COLLATE utf8mb3_bin NOT NULL COMMENT 'the external reference id of current kinetics lambda model',
  `lambda` varchar(1024) COLLATE utf8mb3_bin NOT NULL COMMENT 'the lambda expression of the kinetics',
  `params` varchar(1024) CHARACTER SET utf8mb3 COLLATE utf8mb3_bin NOT NULL COMMENT 'parameter set of the current kinetics lambda epxression, in json string format',
  `temperature` double NOT NULL DEFAULT '37' COMMENT 'temperature of the enzyme catalytic kinetics',
  `pH` double unsigned NOT NULL DEFAULT '7.5' COMMENT 'pH of the enzyme catalytic kinetics',
  `substrate_id` int unsigned NOT NULL COMMENT 'id reference to the metabolite molecule',
  `uniprot` varchar(45) COLLATE utf8mb3_bin NOT NULL COMMENT 'the uniprot id of the current enzyme model, the kinetics parameter is associated with a specific molecule instance',
  `function_id` int unsigned NOT NULL COMMENT 'the internal reference id of the molecule function record, usually link to the ec_number of current kineticis',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext COLLATE utf8mb3_bin COMMENT 'description note text about current enzyme kinetics lambda model',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `regulation_id_idx` (`function_id`) /*!80000 INVISIBLE */,
  KEY `xrefs_index` (`db_xref`) /*!80000 INVISIBLE */,
  KEY `ph_filter` (`pH`) /*!80000 INVISIBLE */,
  KEY `temperature_filter` (`temperature`),
  KEY `uniprot_index` (`uniprot`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='the enzymatic catalytic kinetics lambda model';")>
Public Class kinetic_law: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
''' <summary>
''' the external reference id of current kinetics lambda model
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("db_xref"), NotNull, DataType(MySqlDbType.VarChar, "64"), Column(Name:="db_xref")> Public Property db_xref As String
''' <summary>
''' the lambda expression of the kinetics
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("lambda"), NotNull, DataType(MySqlDbType.VarChar, "1024"), Column(Name:="lambda")> Public Property lambda As String
''' <summary>
''' parameter set of the current kinetics lambda epxression, in json string format
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("params"), NotNull, DataType(MySqlDbType.VarChar, "1024"), Column(Name:="params")> Public Property params As String
''' <summary>
''' temperature of the enzyme catalytic kinetics
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("temperature"), NotNull, DataType(MySqlDbType.Double), Column(Name:="temperature")> Public Property temperature As Double
''' <summary>
''' pH of the enzyme catalytic kinetics
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("pH"), NotNull, DataType(MySqlDbType.Double), Column(Name:="pH")> Public Property pH As Double
''' <summary>
''' id reference to the metabolite molecule
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("substrate_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="substrate_id")> Public Property substrate_id As UInteger
''' <summary>
''' the uniprot id of the current enzyme model, the kinetics parameter is associated with a specific molecule instance
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("uniprot"), NotNull, DataType(MySqlDbType.VarChar, "45"), Column(Name:="uniprot")> Public Property uniprot As String
''' <summary>
''' the internal reference id of the molecule function record, usually link to the ec_number of current kineticis
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("function_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="function_id")> Public Property function_id As UInteger
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
''' <summary>
''' description note text about current enzyme kinetics lambda model
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("note"), DataType(MySqlDbType.Text), Column(Name:="note")> Public Property note As String
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `kinetic_law` (`db_xref`, `lambda`, `params`, `temperature`, `pH`, `substrate_id`, `uniprot`, `function_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `kinetic_law` (`id`, `db_xref`, `lambda`, `params`, `temperature`, `pH`, `substrate_id`, `uniprot`, `function_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `kinetic_law` (`db_xref`, `lambda`, `params`, `temperature`, `pH`, `substrate_id`, `uniprot`, `function_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `kinetic_law` (`id`, `db_xref`, `lambda`, `params`, `temperature`, `pH`, `substrate_id`, `uniprot`, `function_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `kinetic_law` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `kinetic_law` SET `id`='{0}', `db_xref`='{1}', `lambda`='{2}', `params`='{3}', `temperature`='{4}', `pH`='{5}', `substrate_id`='{6}', `uniprot`='{7}', `function_id`='{8}', `add_time`='{9}', `note`='{10}' WHERE `id` = '{11}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `kinetic_law` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `kinetic_law` (`id`, `db_xref`, `lambda`, `params`, `temperature`, `pH`, `substrate_id`, `uniprot`, `function_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, db_xref, lambda, params, temperature, pH, substrate_id, uniprot, function_id, MySqlScript.ToMySqlDateTimeString(add_time), note)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `kinetic_law` (`id`, `db_xref`, `lambda`, `params`, `temperature`, `pH`, `substrate_id`, `uniprot`, `function_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, db_xref, lambda, params, temperature, pH, substrate_id, uniprot, function_id, MySqlScript.ToMySqlDateTimeString(add_time), note)
        Else
        Return String.Format(INSERT_SQL, db_xref, lambda, params, temperature, pH, substrate_id, uniprot, function_id, MySqlScript.ToMySqlDateTimeString(add_time), note)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{db_xref}', '{lambda}', '{params}', '{temperature}', '{pH}', '{substrate_id}', '{uniprot}', '{function_id}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{note}')"
        Else
            Return $"('{db_xref}', '{lambda}', '{params}', '{temperature}', '{pH}', '{substrate_id}', '{uniprot}', '{function_id}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{note}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `kinetic_law` (`id`, `db_xref`, `lambda`, `params`, `temperature`, `pH`, `substrate_id`, `uniprot`, `function_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, db_xref, lambda, params, temperature, pH, substrate_id, uniprot, function_id, MySqlScript.ToMySqlDateTimeString(add_time), note)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `kinetic_law` (`id`, `db_xref`, `lambda`, `params`, `temperature`, `pH`, `substrate_id`, `uniprot`, `function_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, db_xref, lambda, params, temperature, pH, substrate_id, uniprot, function_id, MySqlScript.ToMySqlDateTimeString(add_time), note)
        Else
        Return String.Format(REPLACE_SQL, db_xref, lambda, params, temperature, pH, substrate_id, uniprot, function_id, MySqlScript.ToMySqlDateTimeString(add_time), note)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `kinetic_law` SET `id`='{0}', `db_xref`='{1}', `lambda`='{2}', `params`='{3}', `temperature`='{4}', `pH`='{5}', `substrate_id`='{6}', `uniprot`='{7}', `function_id`='{8}', `add_time`='{9}', `note`='{10}' WHERE `id` = '{11}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, db_xref, lambda, params, temperature, pH, substrate_id, uniprot, function_id, MySqlScript.ToMySqlDateTimeString(add_time), note, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As kinetic_law
                         Return DirectCast(MyClass.MemberwiseClone, kinetic_law)
                     End Function
End Class


End Namespace
