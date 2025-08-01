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
''' DROP TABLE IF EXISTS `molecule`;
''' /*!40101 SET @saved_cs_client     = @@character_set_client */;
''' /*!50503 SET character_set_client = utf8mb4 */;
''' CREATE TABLE `molecule` (
'''   `id` int unsigned NOT NULL AUTO_INCREMENT,
'''   `xref_id` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'the source external reference id of current molecule, for a gene should be a locus_tag, and protein should be a main uniprot accession id and for metabolite should be pubchem cid or chebi id(255 chars for long metacyc id)',
'''   `name` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'the name of the molecule',
'''   `mass` double NOT NULL COMMENT 'the molecular exact mass',
'''   `type` int unsigned NOT NULL COMMENT 'the molecule type, the id of the vocabulary term, value could be nucl(DNA), RNA, prot, metabolite, complex, dna_motif_sequence, protein_motif_sequence',
'''   `formula` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'metabolite formula or nucl/prot sequence',
'''   `parent` int unsigned NOT NULL COMMENT 'the parent metabolite id, example as RNA is a parent of polypeptide, and gene is a parent of RNA sequence.',
'''   `tax_id` int unsigned DEFAULT NULL COMMENT 'the ncbi taxonomy id of current molecule that belongs to, apply for the protein/gene. this relationship data is extract from the genbank/uniprot annotation source directly',
'''   `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'add time of current molecular entity',
'''   `note` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci COMMENT 'description text about current entity object',
'''   PRIMARY KEY (`id`),
'''   UNIQUE KEY `id_UNIQUE` (`id`),
'''   UNIQUE KEY `unique_reference` (`xref_id`),
'''   KEY `data_type_idx` (`type`),
'''   KEY `parent_molecule_idx` (`parent`),
'''   KEY `name_index` (`name`),
'''   KEY `xref_index` (`xref_id`),
'''   KEY `mass_filter` (`mass`),
'''   KEY `find_index` (`xref_id`,`type`),
'''   KEY `taxonomy_info_idx` (`tax_id`),
'''   FULLTEXT KEY `search_text` (`name`,`note`)
''' ) ENGINE=InnoDB AUTO_INCREMENT=76720536 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='The molecular entity object inside a cell';
''' /*!40101 SET character_set_client = @saved_cs_client */;
''' 
''' --
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("molecule", Database:="cad_registry", SchemaSQL:="
CREATE TABLE `molecule` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `xref_id` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'the source external reference id of current molecule, for a gene should be a locus_tag, and protein should be a main uniprot accession id and for metabolite should be pubchem cid or chebi id(255 chars for long metacyc id)',
  `name` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'the name of the molecule',
  `mass` double NOT NULL COMMENT 'the molecular exact mass',
  `type` int unsigned NOT NULL COMMENT 'the molecule type, the id of the vocabulary term, value could be nucl(DNA), RNA, prot, metabolite, complex, dna_motif_sequence, protein_motif_sequence',
  `formula` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'metabolite formula or nucl/prot sequence',
  `parent` int unsigned NOT NULL COMMENT 'the parent metabolite id, example as RNA is a parent of polypeptide, and gene is a parent of RNA sequence.',
  `tax_id` int unsigned DEFAULT NULL COMMENT 'the ncbi taxonomy id of current molecule that belongs to, apply for the protein/gene. this relationship data is extract from the genbank/uniprot annotation source directly',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'add time of current molecular entity',
  `note` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci COMMENT 'description text about current entity object',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `unique_reference` (`xref_id`),
  KEY `data_type_idx` (`type`),
  KEY `parent_molecule_idx` (`parent`),
  KEY `name_index` (`name`),
  KEY `xref_index` (`xref_id`),
  KEY `mass_filter` (`mass`),
  KEY `find_index` (`xref_id`,`type`),
  KEY `taxonomy_info_idx` (`tax_id`),
  FULLTEXT KEY `search_text` (`name`,`note`)
) ENGINE=InnoDB AUTO_INCREMENT=76720536 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='The molecular entity object inside a cell';")>
Public Class molecule: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
''' <summary>
''' the source external reference id of current molecule, for a gene should be a locus_tag, and protein should be a main uniprot accession id and for metabolite should be pubchem cid or chebi id(255 chars for long metacyc id)
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("xref_id"), NotNull, DataType(MySqlDbType.VarChar, "255"), Column(Name:="xref_id")> Public Property xref_id As String
''' <summary>
''' the name of the molecule
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("name"), NotNull, DataType(MySqlDbType.VarChar, "512"), Column(Name:="name")> Public Property name As String
''' <summary>
''' the molecular exact mass
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("mass"), NotNull, DataType(MySqlDbType.Double), Column(Name:="mass")> Public Property mass As Double
''' <summary>
''' the molecule type, the id of the vocabulary term, value could be nucl(DNA), RNA, prot, metabolite, complex, dna_motif_sequence, protein_motif_sequence
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("type"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="type")> Public Property type As UInteger
''' <summary>
''' metabolite formula or nucl/prot sequence
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("formula"), NotNull, DataType(MySqlDbType.VarChar, "128"), Column(Name:="formula")> Public Property formula As String
''' <summary>
''' the parent metabolite id, example as RNA is a parent of polypeptide, and gene is a parent of RNA sequence.
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("parent"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="parent")> Public Property parent As UInteger
''' <summary>
''' the ncbi taxonomy id of current molecule that belongs to, apply for the protein/gene. this relationship data is extract from the genbank/uniprot annotation source directly
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("tax_id"), DataType(MySqlDbType.UInt32, "11"), Column(Name:="tax_id")> Public Property tax_id As UInteger
''' <summary>
''' add time of current molecular entity
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
''' <summary>
''' description text about current entity object
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("note"), DataType(MySqlDbType.Text), Column(Name:="note")> Public Property note As String
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `molecule` (`xref_id`, `name`, `mass`, `type`, `formula`, `parent`, `tax_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `molecule` (`id`, `xref_id`, `name`, `mass`, `type`, `formula`, `parent`, `tax_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `molecule` (`xref_id`, `name`, `mass`, `type`, `formula`, `parent`, `tax_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `molecule` (`id`, `xref_id`, `name`, `mass`, `type`, `formula`, `parent`, `tax_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `molecule` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `molecule` SET `id`='{0}', `xref_id`='{1}', `name`='{2}', `mass`='{3}', `type`='{4}', `formula`='{5}', `parent`='{6}', `tax_id`='{7}', `add_time`='{8}', `note`='{9}' WHERE `id` = '{10}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `molecule` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `molecule` (`id`, `xref_id`, `name`, `mass`, `type`, `formula`, `parent`, `tax_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, xref_id, name, mass, type, formula, parent, tax_id, MySqlScript.ToMySqlDateTimeString(add_time), note)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `molecule` (`id`, `xref_id`, `name`, `mass`, `type`, `formula`, `parent`, `tax_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, xref_id, name, mass, type, formula, parent, tax_id, MySqlScript.ToMySqlDateTimeString(add_time), note)
        Else
        Return String.Format(INSERT_SQL, xref_id, name, mass, type, formula, parent, tax_id, MySqlScript.ToMySqlDateTimeString(add_time), note)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{xref_id}', '{name}', '{mass}', '{type}', '{formula}', '{parent}', '{tax_id}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{note}')"
        Else
            Return $"('{xref_id}', '{name}', '{mass}', '{type}', '{formula}', '{parent}', '{tax_id}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{note}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `molecule` (`id`, `xref_id`, `name`, `mass`, `type`, `formula`, `parent`, `tax_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, xref_id, name, mass, type, formula, parent, tax_id, MySqlScript.ToMySqlDateTimeString(add_time), note)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `molecule` (`id`, `xref_id`, `name`, `mass`, `type`, `formula`, `parent`, `tax_id`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, xref_id, name, mass, type, formula, parent, tax_id, MySqlScript.ToMySqlDateTimeString(add_time), note)
        Else
        Return String.Format(REPLACE_SQL, xref_id, name, mass, type, formula, parent, tax_id, MySqlScript.ToMySqlDateTimeString(add_time), note)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `molecule` SET `id`='{0}', `xref_id`='{1}', `name`='{2}', `mass`='{3}', `type`='{4}', `formula`='{5}', `parent`='{6}', `tax_id`='{7}', `add_time`='{8}', `note`='{9}' WHERE `id` = '{10}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, xref_id, name, mass, type, formula, parent, tax_id, MySqlScript.ToMySqlDateTimeString(add_time), note, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As molecule
                         Return DirectCast(MyClass.MemberwiseClone, molecule)
                     End Function
End Class


End Namespace
