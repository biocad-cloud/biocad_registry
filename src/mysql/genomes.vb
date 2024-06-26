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
''' A subset of the ncbi taxonomy tree
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("genomes", Database:="cad_registry", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `genomes` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `taxonomic_group` INT UNSIGNED NOT NULL,
  `name` VARCHAR(255) NOT NULL,
  `ncbi_taxid` INT UNSIGNED NOT NULL DEFAULT 0 COMMENT 'default 0 means unknown genome ( has not been assigned in ncbi taxonomy database?)',
  `add_time` DATETIME NOT NULL DEFAULT now(),
  PRIMARY KEY (`id`))
ENGINE = InnoDB
COMMENT = 'A subset of the ncbi taxonomy tree';
")>
Public Class genomes: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("taxonomic_group"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="taxonomic_group")> Public Property taxonomic_group As UInteger
    <DatabaseField("name"), NotNull, DataType(MySqlDbType.VarChar, "255"), Column(Name:="name")> Public Property name As String
''' <summary>
''' default 0 means unknown genome ( has not been assigned in ncbi taxonomy database?)
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("ncbi_taxid"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="ncbi_taxid")> Public Property ncbi_taxid As UInteger
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `genomes` (`taxonomic_group`, `name`, `ncbi_taxid`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `genomes` (`id`, `taxonomic_group`, `name`, `ncbi_taxid`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `genomes` (`taxonomic_group`, `name`, `ncbi_taxid`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `genomes` (`id`, `taxonomic_group`, `name`, `ncbi_taxid`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `genomes` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `genomes` SET `id`='{0}', `taxonomic_group`='{1}', `name`='{2}', `ncbi_taxid`='{3}', `add_time`='{4}' WHERE `id` = '{5}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `genomes` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `genomes` (`id`, `taxonomic_group`, `name`, `ncbi_taxid`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, taxonomic_group, name, ncbi_taxid, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `genomes` (`id`, `taxonomic_group`, `name`, `ncbi_taxid`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, taxonomic_group, name, ncbi_taxid, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(INSERT_SQL, taxonomic_group, name, ncbi_taxid, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{taxonomic_group}', '{name}', '{ncbi_taxid}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        Else
            Return $"('{taxonomic_group}', '{name}', '{ncbi_taxid}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `genomes` (`id`, `taxonomic_group`, `name`, `ncbi_taxid`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, taxonomic_group, name, ncbi_taxid, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `genomes` (`id`, `taxonomic_group`, `name`, `ncbi_taxid`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, taxonomic_group, name, ncbi_taxid, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(REPLACE_SQL, taxonomic_group, name, ncbi_taxid, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `genomes` SET `id`='{0}', `taxonomic_group`='{1}', `name`='{2}', `ncbi_taxid`='{3}', `add_time`='{4}' WHERE `id` = '{5}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, taxonomic_group, name, ncbi_taxid, MySqlScript.ToMySqlDateTimeString(add_time), id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As genomes
                         Return DirectCast(MyClass.MemberwiseClone, genomes)
                     End Function
End Class


End Namespace
