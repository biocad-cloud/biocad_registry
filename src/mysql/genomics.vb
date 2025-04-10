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
''' genome nucleotide sequence data pool, the genomics dna sequence is another kind of marcro molecule.
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("genomics", Database:="cad_registry", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `genomics` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `db_xref` VARCHAR(45) NULL DEFAULT NULL COMMENT 'ncbi genbank accession id of current genomics sequence',
  `ncbi_taxid` INT UNSIGNED NOT NULL COMMENT 'the ncbi taxonomy id',
  `biom_string` VARCHAR(4096) NULL DEFAULT 'NA',
  `def` MEDIUMTEXT COLLATE 'utf8mb3_bin' NOT NULL COMMENT 'definition and description about this genome',
  `nt` LONGTEXT CHARACTER SET 'utf8mb3' NOT NULL COMMENT 'DNA sequence data',
  `add_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `comment` LONGTEXT COLLATE 'utf8mb3_bin' NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE,
  INDEX `taxonomy_index` (`ncbi_taxid` ASC) VISIBLE,
  INDEX `accession_index` (`db_xref` ASC) INVISIBLE,
  FULLTEXT INDEX `search_text` (`def`, `comment`) VISIBLE)
ENGINE = InnoDB
COMMENT = 'genome nucleotide sequence data pool, the genomics dna sequence is another kind of marcro molecule.';
")>
Public Class genomics: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
''' <summary>
''' ncbi genbank accession id of current genomics sequence
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("db_xref"), DataType(MySqlDbType.VarChar, "45"), Column(Name:="db_xref")> Public Property db_xref As String
''' <summary>
''' the ncbi taxonomy id
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("ncbi_taxid"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="ncbi_taxid")> Public Property ncbi_taxid As UInteger
    <DatabaseField("biom_string"), DataType(MySqlDbType.VarChar, "4096"), Column(Name:="biom_string")> Public Property biom_string As String
''' <summary>
''' definition and description about this genome
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("def"), NotNull, DataType(MySqlDbType.Text), Column(Name:="def")> Public Property def As String
''' <summary>
''' DNA sequence data
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("nt"), NotNull, DataType(MySqlDbType.Text), Column(Name:="nt")> Public Property nt As String
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
    <DatabaseField("comment"), DataType(MySqlDbType.Text), Column(Name:="comment")> Public Property comment As String
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `genomics` (`db_xref`, `ncbi_taxid`, `biom_string`, `def`, `nt`, `add_time`, `comment`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `genomics` (`id`, `db_xref`, `ncbi_taxid`, `biom_string`, `def`, `nt`, `add_time`, `comment`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `genomics` (`db_xref`, `ncbi_taxid`, `biom_string`, `def`, `nt`, `add_time`, `comment`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `genomics` (`id`, `db_xref`, `ncbi_taxid`, `biom_string`, `def`, `nt`, `add_time`, `comment`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `genomics` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `genomics` SET `id`='{0}', `db_xref`='{1}', `ncbi_taxid`='{2}', `biom_string`='{3}', `def`='{4}', `nt`='{5}', `add_time`='{6}', `comment`='{7}' WHERE `id` = '{8}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `genomics` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `genomics` (`id`, `db_xref`, `ncbi_taxid`, `biom_string`, `def`, `nt`, `add_time`, `comment`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, db_xref, ncbi_taxid, biom_string, def, nt, MySqlScript.ToMySqlDateTimeString(add_time), comment)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `genomics` (`id`, `db_xref`, `ncbi_taxid`, `biom_string`, `def`, `nt`, `add_time`, `comment`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, db_xref, ncbi_taxid, biom_string, def, nt, MySqlScript.ToMySqlDateTimeString(add_time), comment)
        Else
        Return String.Format(INSERT_SQL, db_xref, ncbi_taxid, biom_string, def, nt, MySqlScript.ToMySqlDateTimeString(add_time), comment)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{db_xref}', '{ncbi_taxid}', '{biom_string}', '{def}', '{nt}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{comment}')"
        Else
            Return $"('{db_xref}', '{ncbi_taxid}', '{biom_string}', '{def}', '{nt}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{comment}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `genomics` (`id`, `db_xref`, `ncbi_taxid`, `biom_string`, `def`, `nt`, `add_time`, `comment`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, db_xref, ncbi_taxid, biom_string, def, nt, MySqlScript.ToMySqlDateTimeString(add_time), comment)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `genomics` (`id`, `db_xref`, `ncbi_taxid`, `biom_string`, `def`, `nt`, `add_time`, `comment`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, db_xref, ncbi_taxid, biom_string, def, nt, MySqlScript.ToMySqlDateTimeString(add_time), comment)
        Else
        Return String.Format(REPLACE_SQL, db_xref, ncbi_taxid, biom_string, def, nt, MySqlScript.ToMySqlDateTimeString(add_time), comment)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `genomics` SET `id`='{0}', `db_xref`='{1}', `ncbi_taxid`='{2}', `biom_string`='{3}', `def`='{4}', `nt`='{5}', `add_time`='{6}', `comment`='{7}' WHERE `id` = '{8}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, db_xref, ncbi_taxid, biom_string, def, nt, MySqlScript.ToMySqlDateTimeString(add_time), comment, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As genomics
                         Return DirectCast(MyClass.MemberwiseClone, genomics)
                     End Function
End Class


End Namespace
