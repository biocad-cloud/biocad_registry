REM  Oracle.LinuxCompatibility.MySQL.CodeSolution.VisualBasic.CodeGenerator
REM  MYSQL Schema Mapper
REM      for Microsoft VisualBasic.NET 1.0.0.0

REM  Dump @4/10/2025 11:27:19 PM


Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports MySqlScript = Oracle.LinuxCompatibility.MySQL.Scripting.Extensions

Namespace biocad_registryModel

''' <summary>
''' ```SQL
''' the enzymatic catalytic kinetics lambda model
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("kinetic_law", Database:="cad_registry", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `kinetic_law` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `db_xref` VARCHAR(64) NOT NULL COMMENT 'the external reference id of current kinetics lambda model',
  `params` JSON NOT NULL COMMENT 'parameter set of the current kinetics lambda epxression, in json string format',
  `lambda` MEDIUMTEXT CHARACTER SET 'utf8mb3' NOT NULL COMMENT 'the lambda math expression of the kinetics',
  `temperature` DOUBLE NOT NULL DEFAULT '37' COMMENT 'temperature of the enzyme catalytic kinetics',
  `pH` DOUBLE UNSIGNED NOT NULL DEFAULT '7.5' COMMENT 'pH of the enzyme catalytic kinetics',
  `buffer` MEDIUMTEXT COLLATE 'utf8mb3_bin' NULL DEFAULT NULL COMMENT 'the buffer environment of the enzyme test',
  `substrate_id` INT UNSIGNED NOT NULL COMMENT 'id reference to the metabolite molecule',
  `uniprot` VARCHAR(45) CHARACTER SET 'utf8mb3' NULL DEFAULT NULL COMMENT 'the uniprot id of the current enzyme model, the kinetics parameter is associated with a specific molecule instance. could be missing',
  `ec_number` VARCHAR(128) NOT NULL DEFAULT '-' COMMENT 'the enzyme ec_number reference id of the molecule function record',
  `json_str` JSON NOT NULL COMMENT 'the raw json string data of current kinetics data',
  `add_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` LONGTEXT COLLATE 'utf8mb3_bin' NULL DEFAULT NULL COMMENT 'description note text about current enzyme kinetics lambda model',
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE,
  INDEX `regulation_id_idx` (`ec_number` ASC) VISIBLE,
  INDEX `xrefs_index` (`db_xref` ASC) VISIBLE,
  INDEX `ph_filter` (`pH` ASC) VISIBLE,
  INDEX `temperature_filter` (`temperature` ASC) VISIBLE,
  INDEX `uniprot_index` (`uniprot` ASC) VISIBLE,
  INDEX `substrate_molecule_idx` (`substrate_id` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 35204
COMMENT = 'the enzymatic catalytic kinetics lambda model';
")>
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
''' parameter set of the current kinetics lambda epxression, in json string format
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("params"), NotNull, DataType(MySqlDbType.Text), Column(Name:="params")> Public Property params As String
''' <summary>
''' the lambda math expression of the kinetics
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("lambda"), NotNull, DataType(MySqlDbType.Text), Column(Name:="lambda")> Public Property lambda As String
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
''' the buffer environment of the enzyme test
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("buffer"), DataType(MySqlDbType.Text), Column(Name:="buffer")> Public Property buffer As String
''' <summary>
''' id reference to the metabolite molecule
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("substrate_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="substrate_id")> Public Property substrate_id As UInteger
''' <summary>
''' the uniprot id of the current enzyme model, the kinetics parameter is associated with a specific molecule instance. could be missing
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("uniprot"), DataType(MySqlDbType.VarChar, "45"), Column(Name:="uniprot")> Public Property uniprot As String
''' <summary>
''' the enzyme ec_number reference id of the molecule function record
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("ec_number"), NotNull, DataType(MySqlDbType.VarChar, "128"), Column(Name:="ec_number")> Public Property ec_number As String
''' <summary>
''' the raw json string data of current kinetics data
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("json_str"), NotNull, DataType(MySqlDbType.Text), Column(Name:="json_str")> Public Property json_str As String
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
        <SQL>INSERT INTO `kinetic_law` (`db_xref`, `params`, `lambda`, `temperature`, `pH`, `buffer`, `substrate_id`, `uniprot`, `ec_number`, `json_str`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `kinetic_law` (`id`, `db_xref`, `params`, `lambda`, `temperature`, `pH`, `buffer`, `substrate_id`, `uniprot`, `ec_number`, `json_str`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `kinetic_law` (`db_xref`, `params`, `lambda`, `temperature`, `pH`, `buffer`, `substrate_id`, `uniprot`, `ec_number`, `json_str`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `kinetic_law` (`id`, `db_xref`, `params`, `lambda`, `temperature`, `pH`, `buffer`, `substrate_id`, `uniprot`, `ec_number`, `json_str`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `kinetic_law` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `kinetic_law` SET `id`='{0}', `db_xref`='{1}', `params`='{2}', `lambda`='{3}', `temperature`='{4}', `pH`='{5}', `buffer`='{6}', `substrate_id`='{7}', `uniprot`='{8}', `ec_number`='{9}', `json_str`='{10}', `add_time`='{11}', `note`='{12}' WHERE `id` = '{13}';</SQL>

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
''' INSERT INTO `kinetic_law` (`id`, `db_xref`, `params`, `lambda`, `temperature`, `pH`, `buffer`, `substrate_id`, `uniprot`, `ec_number`, `json_str`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, db_xref, params, lambda, temperature, pH, buffer, substrate_id, uniprot, ec_number, json_str, MySqlScript.ToMySqlDateTimeString(add_time), note)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `kinetic_law` (`id`, `db_xref`, `params`, `lambda`, `temperature`, `pH`, `buffer`, `substrate_id`, `uniprot`, `ec_number`, `json_str`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, db_xref, params, lambda, temperature, pH, buffer, substrate_id, uniprot, ec_number, json_str, MySqlScript.ToMySqlDateTimeString(add_time), note)
        Else
        Return String.Format(INSERT_SQL, db_xref, params, lambda, temperature, pH, buffer, substrate_id, uniprot, ec_number, json_str, MySqlScript.ToMySqlDateTimeString(add_time), note)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{db_xref}', '{params}', '{lambda}', '{temperature}', '{pH}', '{buffer}', '{substrate_id}', '{uniprot}', '{ec_number}', '{json_str}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{note}')"
        Else
            Return $"('{db_xref}', '{params}', '{lambda}', '{temperature}', '{pH}', '{buffer}', '{substrate_id}', '{uniprot}', '{ec_number}', '{json_str}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{note}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `kinetic_law` (`id`, `db_xref`, `params`, `lambda`, `temperature`, `pH`, `buffer`, `substrate_id`, `uniprot`, `ec_number`, `json_str`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, db_xref, params, lambda, temperature, pH, buffer, substrate_id, uniprot, ec_number, json_str, MySqlScript.ToMySqlDateTimeString(add_time), note)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `kinetic_law` (`id`, `db_xref`, `params`, `lambda`, `temperature`, `pH`, `buffer`, `substrate_id`, `uniprot`, `ec_number`, `json_str`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, db_xref, params, lambda, temperature, pH, buffer, substrate_id, uniprot, ec_number, json_str, MySqlScript.ToMySqlDateTimeString(add_time), note)
        Else
        Return String.Format(REPLACE_SQL, db_xref, params, lambda, temperature, pH, buffer, substrate_id, uniprot, ec_number, json_str, MySqlScript.ToMySqlDateTimeString(add_time), note)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `kinetic_law` SET `id`='{0}', `db_xref`='{1}', `params`='{2}', `lambda`='{3}', `temperature`='{4}', `pH`='{5}', `buffer`='{6}', `substrate_id`='{7}', `uniprot`='{8}', `ec_number`='{9}', `json_str`='{10}', `add_time`='{11}', `note`='{12}' WHERE `id` = '{13}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, db_xref, params, lambda, temperature, pH, buffer, substrate_id, uniprot, ec_number, json_str, MySqlScript.ToMySqlDateTimeString(add_time), note, id)
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
