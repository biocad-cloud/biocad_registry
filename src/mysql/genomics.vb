REM  Oracle.LinuxCompatibility.MySQL.CodeSolution.VisualBasic.CodeGenerator
REM  MYSQL Schema Mapper
REM      for Microsoft VisualBasic.NET 1.0.0.0

REM  Dump @1/4/2025 12:06:52 PM


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
''' DROP TABLE IF EXISTS `genomics`;
''' /*!40101 SET @saved_cs_client     = @@character_set_client */;
''' /*!50503 SET character_set_client = utf8mb4 */;
''' CREATE TABLE `genomics` (
'''   `id` int unsigned NOT NULL AUTO_INCREMENT,
'''   `ncbi_taxid` int unsigned NOT NULL COMMENT 'the ncbi taxonomy id',
'''   `db_xref` varchar(45) COLLATE utf8mb3_bin DEFAULT NULL COMMENT 'ncbi genbank accession id of current genomics sequence',
'''   `def` mediumtext COLLATE utf8mb3_bin NOT NULL,
'''   `nt` longtext CHARACTER SET utf8mb3 COLLATE utf8mb3_bin NOT NULL COMMENT 'sequence data',
'''   `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
'''   `comment` longtext COLLATE utf8mb3_bin,
'''   PRIMARY KEY (`id`),
'''   UNIQUE KEY `id_UNIQUE` (`id`),
'''   KEY `taxonomy_index` (`ncbi_taxid`),
'''   KEY `accession_index` (`db_xref`) /*!80000 INVISIBLE */,
'''   FULLTEXT KEY `search_text` (`def`,`comment`)
''' ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='genome nucleotide sequence data';
''' /*!40101 SET character_set_client = @saved_cs_client */;
''' 
''' --
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("genomics", Database:="cad_registry", SchemaSQL:="
CREATE TABLE `genomics` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `ncbi_taxid` int unsigned NOT NULL COMMENT 'the ncbi taxonomy id',
  `db_xref` varchar(45) COLLATE utf8mb3_bin DEFAULT NULL COMMENT 'ncbi genbank accession id of current genomics sequence',
  `def` mediumtext COLLATE utf8mb3_bin NOT NULL,
  `nt` longtext CHARACTER SET utf8mb3 COLLATE utf8mb3_bin NOT NULL COMMENT 'sequence data',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `comment` longtext COLLATE utf8mb3_bin,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `taxonomy_index` (`ncbi_taxid`),
  KEY `accession_index` (`db_xref`) /*!80000 INVISIBLE */,
  FULLTEXT KEY `search_text` (`def`,`comment`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='genome nucleotide sequence data';")>
Public Class genomics: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
''' <summary>
''' the ncbi taxonomy id
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("ncbi_taxid"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="ncbi_taxid")> Public Property ncbi_taxid As UInteger
''' <summary>
''' ncbi genbank accession id of current genomics sequence
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("db_xref"), DataType(MySqlDbType.VarChar, "45"), Column(Name:="db_xref")> Public Property db_xref As String
    <DatabaseField("def"), NotNull, DataType(MySqlDbType.Text), Column(Name:="def")> Public Property def As String
''' <summary>
''' sequence data
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
        <SQL>INSERT INTO `genomics` (`ncbi_taxid`, `db_xref`, `def`, `nt`, `add_time`, `comment`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `genomics` (`id`, `ncbi_taxid`, `db_xref`, `def`, `nt`, `add_time`, `comment`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `genomics` (`ncbi_taxid`, `db_xref`, `def`, `nt`, `add_time`, `comment`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `genomics` (`id`, `ncbi_taxid`, `db_xref`, `def`, `nt`, `add_time`, `comment`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `genomics` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `genomics` SET `id`='{0}', `ncbi_taxid`='{1}', `db_xref`='{2}', `def`='{3}', `nt`='{4}', `add_time`='{5}', `comment`='{6}' WHERE `id` = '{7}';</SQL>

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
''' INSERT INTO `genomics` (`id`, `ncbi_taxid`, `db_xref`, `def`, `nt`, `add_time`, `comment`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, ncbi_taxid, db_xref, def, nt, MySqlScript.ToMySqlDateTimeString(add_time), comment)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `genomics` (`id`, `ncbi_taxid`, `db_xref`, `def`, `nt`, `add_time`, `comment`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, ncbi_taxid, db_xref, def, nt, MySqlScript.ToMySqlDateTimeString(add_time), comment)
        Else
        Return String.Format(INSERT_SQL, ncbi_taxid, db_xref, def, nt, MySqlScript.ToMySqlDateTimeString(add_time), comment)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{ncbi_taxid}', '{db_xref}', '{def}', '{nt}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{comment}')"
        Else
            Return $"('{ncbi_taxid}', '{db_xref}', '{def}', '{nt}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{comment}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `genomics` (`id`, `ncbi_taxid`, `db_xref`, `def`, `nt`, `add_time`, `comment`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, ncbi_taxid, db_xref, def, nt, MySqlScript.ToMySqlDateTimeString(add_time), comment)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `genomics` (`id`, `ncbi_taxid`, `db_xref`, `def`, `nt`, `add_time`, `comment`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, ncbi_taxid, db_xref, def, nt, MySqlScript.ToMySqlDateTimeString(add_time), comment)
        Else
        Return String.Format(REPLACE_SQL, ncbi_taxid, db_xref, def, nt, MySqlScript.ToMySqlDateTimeString(add_time), comment)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `genomics` SET `id`='{0}', `ncbi_taxid`='{1}', `db_xref`='{2}', `def`='{3}', `nt`='{4}', `add_time`='{5}', `comment`='{6}' WHERE `id` = '{7}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, ncbi_taxid, db_xref, def, nt, MySqlScript.ToMySqlDateTimeString(add_time), comment, id)
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
