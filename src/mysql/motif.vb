REM  Oracle.LinuxCompatibility.MySQL.CodeSolution.VisualBasic.CodeGenerator
REM  MYSQL Schema Mapper
REM      for Microsoft VisualBasic.NET 1.0.0.0

REM  Dump @4/4/2024 5:05:27 PM


Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports MySqlScript = Oracle.LinuxCompatibility.MySQL.Scripting.Extensions

Namespace biocad_registryModel

''' <summary>
''' ```SQL
''' organism non-specific data
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("motif", Database:="cad_registry", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `motif` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `family` VARCHAR(45) NOT NULL COMMENT 'the motif family name',
  `family_id` INT UNSIGNED NOT NULL,
  `type` VARCHAR(3) NOT NULL DEFAULT 'TF' COMMENT 'RNA/TF',
  `width` INT UNSIGNED NOT NULL COMMENT 'the motif PWM width',
  `variant` VARCHAR(4096) NOT NULL COMMENT 'family may contains multiple version of the PWM variant matrix data, this data field is the sequence string representative of the PWM matrix object',
  `checmsum` VARCHAR(32) NULL COMMENT 'check sum of the pwm binary pack data',
  `pwm` VARCHAR(8192) NOT NULL COMMENT 'pwm data bson encoded in gzip compression base64 string',
  `add_time` DATETIME NOT NULL DEFAULT now(),
  `note` VARCHAR(4096) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
COMMENT = 'organism non-specific data';
")>
Public Class motif: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
''' <summary>
''' the motif family name
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("family"), NotNull, DataType(MySqlDbType.VarChar, "45"), Column(Name:="family")> Public Property family As String
    <DatabaseField("family_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="family_id")> Public Property family_id As UInteger
''' <summary>
''' RNA/TF
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("type"), NotNull, DataType(MySqlDbType.VarChar, "3"), Column(Name:="type")> Public Property type As String
''' <summary>
''' the motif PWM width
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("width"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="width")> Public Property width As UInteger
''' <summary>
''' family may contains multiple version of the PWM variant matrix data, this data field is the sequence string representative of the PWM matrix object
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("variant"), NotNull, DataType(MySqlDbType.VarChar, "4096"), Column(Name:="variant")> Public Property [variant] As String
''' <summary>
''' check sum of the pwm binary pack data
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("checmsum"), DataType(MySqlDbType.VarChar, "32"), Column(Name:="checmsum")> Public Property checmsum As String
''' <summary>
''' pwm data bson encoded in gzip compression base64 string
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("pwm"), NotNull, DataType(MySqlDbType.VarChar, "8192"), Column(Name:="pwm")> Public Property pwm As String
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
    <DatabaseField("note"), DataType(MySqlDbType.VarChar, "4096"), Column(Name:="note")> Public Property note As String
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `motif` (`family`, `family_id`, `type`, `width`, `variant`, `checmsum`, `pwm`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `motif` (`id`, `family`, `family_id`, `type`, `width`, `variant`, `checmsum`, `pwm`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `motif` (`family`, `family_id`, `type`, `width`, `variant`, `checmsum`, `pwm`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `motif` (`id`, `family`, `family_id`, `type`, `width`, `variant`, `checmsum`, `pwm`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `motif` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `motif` SET `id`='{0}', `family`='{1}', `family_id`='{2}', `type`='{3}', `width`='{4}', `variant`='{5}', `checmsum`='{6}', `pwm`='{7}', `add_time`='{8}', `note`='{9}' WHERE `id` = '{10}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `motif` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `motif` (`id`, `family`, `family_id`, `type`, `width`, `variant`, `checmsum`, `pwm`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, family, family_id, type, width, [variant], checmsum, pwm, MySqlScript.ToMySqlDateTimeString(add_time), note)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `motif` (`id`, `family`, `family_id`, `type`, `width`, `variant`, `checmsum`, `pwm`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, family, family_id, type, width, [variant], checmsum, pwm, MySqlScript.ToMySqlDateTimeString(add_time), note)
        Else
        Return String.Format(INSERT_SQL, family, family_id, type, width, [variant], checmsum, pwm, MySqlScript.ToMySqlDateTimeString(add_time), note)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{family}', '{family_id}', '{type}', '{width}', '{[variant]}', '{checmsum}', '{pwm}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{note}')"
        Else
            Return $"('{family}', '{family_id}', '{type}', '{width}', '{[variant]}', '{checmsum}', '{pwm}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{note}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `motif` (`id`, `family`, `family_id`, `type`, `width`, `variant`, `checmsum`, `pwm`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, family, family_id, type, width, [variant], checmsum, pwm, MySqlScript.ToMySqlDateTimeString(add_time), note)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `motif` (`id`, `family`, `family_id`, `type`, `width`, `variant`, `checmsum`, `pwm`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, family, family_id, type, width, [variant], checmsum, pwm, MySqlScript.ToMySqlDateTimeString(add_time), note)
        Else
        Return String.Format(REPLACE_SQL, family, family_id, type, width, [variant], checmsum, pwm, MySqlScript.ToMySqlDateTimeString(add_time), note)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `motif` SET `id`='{0}', `family`='{1}', `family_id`='{2}', `type`='{3}', `width`='{4}', `variant`='{5}', `checmsum`='{6}', `pwm`='{7}', `add_time`='{8}', `note`='{9}' WHERE `id` = '{10}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, family, family_id, type, width, [variant], checmsum, pwm, MySqlScript.ToMySqlDateTimeString(add_time), note, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As motif
                         Return DirectCast(MyClass.MemberwiseClone, motif)
                     End Function
End Class


End Namespace
