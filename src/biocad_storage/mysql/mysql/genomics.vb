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
''' DROP TABLE IF EXISTS `genomics`;
''' /*!40101 SET @saved_cs_client     = @@character_set_client */;
''' /*!50503 SET character_set_client = utf8mb4 */;
''' CREATE TABLE `genomics` (
'''   `id` int unsigned NOT NULL AUTO_INCREMENT,
'''   `db_xref` varchar(45) COLLATE utf8mb3_bin DEFAULT NULL COMMENT 'ncbi genbank accession id of current genomics sequence',
'''   `ncbi_taxid` int unsigned NOT NULL COMMENT 'the ncbi taxonomy id',
'''   `biom_string` varchar(4096) COLLATE utf8mb3_bin DEFAULT 'NA',
'''   `def` mediumtext CHARACTER SET utf8mb3 COLLATE utf8mb3_bin NOT NULL COMMENT 'definition and description about this genome',
'''   `length` int unsigned NOT NULL DEFAULT '0',
'''   `checksum` char(32) COLLATE utf8mb3_bin DEFAULT NULL,
'''   `nt` longtext CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL COMMENT 'DNA sequence data',
'''   `fingerprint` longtext COLLATE utf8mb3_bin COMMENT 'base64 encoded of the 8192 bytes morgan fingerprint of this genome nt sequence',
'''   `embedding` tinytext COLLATE utf8mb3_bin COMMENT 'base64 encoded of the 9 dimension size umap embedding of the morgan fingerprint of this genome sequence data',
'''   `comment` longtext CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci,
'''   `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
'''   PRIMARY KEY (`id`),
'''   UNIQUE KEY `id_UNIQUE` (`id`),
'''   UNIQUE KEY `unique_genome` (`db_xref`,`ncbi_taxid`),
'''   KEY `taxonomy_index` (`ncbi_taxid`),
'''   KEY `accession_index` (`db_xref`),
'''   KEY `sort_time` (`add_time` DESC),
'''   FULLTEXT KEY `biom_index` (`biom_string`),
'''   FULLTEXT KEY `def_index` (`def`),
'''   FULLTEXT KEY `comment_text_index` (`comment`)
''' ) ENGINE=InnoDB AUTO_INCREMENT=414091 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='genome nucleotide sequence data pool, the genomics dna sequence is another kind of marcro molecule.';
''' /*!40101 SET character_set_client = @saved_cs_client */;
''' 
''' --
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("genomics", Database:="cad_registry", SchemaSQL:="
CREATE TABLE `genomics` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `db_xref` varchar(45) COLLATE utf8mb3_bin DEFAULT NULL COMMENT 'ncbi genbank accession id of current genomics sequence',
  `ncbi_taxid` int unsigned NOT NULL COMMENT 'the ncbi taxonomy id',
  `biom_string` varchar(4096) COLLATE utf8mb3_bin DEFAULT 'NA',
  `def` mediumtext CHARACTER SET utf8mb3 COLLATE utf8mb3_bin NOT NULL COMMENT 'definition and description about this genome',
  `length` int unsigned NOT NULL DEFAULT '0',
  `checksum` char(32) COLLATE utf8mb3_bin DEFAULT NULL,
  `nt` longtext CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL COMMENT 'DNA sequence data',
  `fingerprint` longtext COLLATE utf8mb3_bin COMMENT 'base64 encoded of the 8192 bytes morgan fingerprint of this genome nt sequence',
  `embedding` tinytext COLLATE utf8mb3_bin COMMENT 'base64 encoded of the 9 dimension size umap embedding of the morgan fingerprint of this genome sequence data',
  `comment` longtext CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `unique_genome` (`db_xref`,`ncbi_taxid`),
  KEY `taxonomy_index` (`ncbi_taxid`),
  KEY `accession_index` (`db_xref`),
  KEY `sort_time` (`add_time` DESC),
  FULLTEXT KEY `biom_index` (`biom_string`),
  FULLTEXT KEY `def_index` (`def`),
  FULLTEXT KEY `comment_text_index` (`comment`)
) ENGINE=InnoDB AUTO_INCREMENT=414091 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='genome nucleotide sequence data pool, the genomics dna sequence is another kind of marcro molecule.';")>
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
    <DatabaseField("length"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="length")> Public Property length As UInteger
    <DatabaseField("checksum"), DataType(MySqlDbType.VarChar, "32"), Column(Name:="checksum")> Public Property checksum As String
''' <summary>
''' DNA sequence data
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("nt"), NotNull, DataType(MySqlDbType.Text), Column(Name:="nt")> Public Property nt As String
''' <summary>
''' base64 encoded of the 8192 bytes morgan fingerprint of this genome nt sequence
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("fingerprint"), DataType(MySqlDbType.Text), Column(Name:="fingerprint")> Public Property fingerprint As String
''' <summary>
''' base64 encoded of the 9 dimension size umap embedding of the morgan fingerprint of this genome sequence data
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("embedding"), DataType(MySqlDbType.Text), Column(Name:="embedding")> Public Property embedding As String
    <DatabaseField("comment"), DataType(MySqlDbType.Text), Column(Name:="comment")> Public Property comment As String
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `genomics` (`db_xref`, `ncbi_taxid`, `biom_string`, `def`, `length`, `checksum`, `nt`, `fingerprint`, `embedding`, `comment`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `genomics` (`id`, `db_xref`, `ncbi_taxid`, `biom_string`, `def`, `length`, `checksum`, `nt`, `fingerprint`, `embedding`, `comment`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `genomics` (`db_xref`, `ncbi_taxid`, `biom_string`, `def`, `length`, `checksum`, `nt`, `fingerprint`, `embedding`, `comment`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `genomics` (`id`, `db_xref`, `ncbi_taxid`, `biom_string`, `def`, `length`, `checksum`, `nt`, `fingerprint`, `embedding`, `comment`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `genomics` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `genomics` SET `id`='{0}', `db_xref`='{1}', `ncbi_taxid`='{2}', `biom_string`='{3}', `def`='{4}', `length`='{5}', `checksum`='{6}', `nt`='{7}', `fingerprint`='{8}', `embedding`='{9}', `comment`='{10}', `add_time`='{11}' WHERE `id` = '{12}';</SQL>

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
''' INSERT INTO `genomics` (`id`, `db_xref`, `ncbi_taxid`, `biom_string`, `def`, `length`, `checksum`, `nt`, `fingerprint`, `embedding`, `comment`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, db_xref, ncbi_taxid, biom_string, def, length, checksum, nt, fingerprint, embedding, comment, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `genomics` (`id`, `db_xref`, `ncbi_taxid`, `biom_string`, `def`, `length`, `checksum`, `nt`, `fingerprint`, `embedding`, `comment`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, db_xref, ncbi_taxid, biom_string, def, length, checksum, nt, fingerprint, embedding, comment, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(INSERT_SQL, db_xref, ncbi_taxid, biom_string, def, length, checksum, nt, fingerprint, embedding, comment, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{db_xref}', '{ncbi_taxid}', '{biom_string}', '{def}', '{length}', '{checksum}', '{nt}', '{fingerprint}', '{embedding}', '{comment}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        Else
            Return $"('{db_xref}', '{ncbi_taxid}', '{biom_string}', '{def}', '{length}', '{checksum}', '{nt}', '{fingerprint}', '{embedding}', '{comment}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `genomics` (`id`, `db_xref`, `ncbi_taxid`, `biom_string`, `def`, `length`, `checksum`, `nt`, `fingerprint`, `embedding`, `comment`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, db_xref, ncbi_taxid, biom_string, def, length, checksum, nt, fingerprint, embedding, comment, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `genomics` (`id`, `db_xref`, `ncbi_taxid`, `biom_string`, `def`, `length`, `checksum`, `nt`, `fingerprint`, `embedding`, `comment`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, db_xref, ncbi_taxid, biom_string, def, length, checksum, nt, fingerprint, embedding, comment, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(REPLACE_SQL, db_xref, ncbi_taxid, biom_string, def, length, checksum, nt, fingerprint, embedding, comment, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `genomics` SET `id`='{0}', `db_xref`='{1}', `ncbi_taxid`='{2}', `biom_string`='{3}', `def`='{4}', `length`='{5}', `checksum`='{6}', `nt`='{7}', `fingerprint`='{8}', `embedding`='{9}', `comment`='{10}', `add_time`='{11}' WHERE `id` = '{12}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, db_xref, ncbi_taxid, biom_string, def, length, checksum, nt, fingerprint, embedding, comment, MySqlScript.ToMySqlDateTimeString(add_time), id)
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
