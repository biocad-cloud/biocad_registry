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
''' DROP TABLE IF EXISTS `sequence_graph`;
''' /*!40101 SET @saved_cs_client     = @@character_set_client */;
''' /*!50503 SET character_set_client = utf8mb4 */;
''' CREATE TABLE `sequence_graph` (
'''   `id` int unsigned NOT NULL AUTO_INCREMENT,
'''   `molecule_id` int unsigned NOT NULL COMMENT 'the molecule reference id inside biocad registry system',
'''   `sequence` longtext COLLATE utf8mb3_bin NOT NULL COMMENT 'the molecule sequence data, for metabolite should be the SMILES structure string data, gene should be the nucleotide sequence data and protein should be the polypeptide sequence.',
'''   `hashcode` char(32) CHARACTER SET utf8mb3 COLLATE utf8mb3_bin NOT NULL COMMENT 'md5 hashcode of the sequence data string',
'''   `morgan` longtext CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci COMMENT 'base64 encoded morgan fingerprint of the k-mer(k=3) graph of the sequence, or the morgan fingerprint of the metabolite molecular structure data',
'''   `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
'''   PRIMARY KEY (`id`),
'''   UNIQUE KEY `id_UNIQUE` (`id`),
'''   UNIQUE KEY `unique_mol_data` (`molecule_id`,`hashcode`),
'''   KEY `molecules_idx` (`molecule_id`),
'''   KEY `search_sequence` (`hashcode`),
'''   KEY `index_mols_seqs` (`molecule_id`,`hashcode`)
''' ) ENGINE=InnoDB AUTO_INCREMENT=162529106 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='the sequence composition data of the molecule';
''' /*!40101 SET character_set_client = @saved_cs_client */;
''' 
''' --
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("sequence_graph", Database:="cad_registry", SchemaSQL:="
CREATE TABLE `sequence_graph` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `molecule_id` int unsigned NOT NULL COMMENT 'the molecule reference id inside biocad registry system',
  `sequence` longtext COLLATE utf8mb3_bin NOT NULL COMMENT 'the molecule sequence data, for metabolite should be the SMILES structure string data, gene should be the nucleotide sequence data and protein should be the polypeptide sequence.',
  `hashcode` char(32) CHARACTER SET utf8mb3 COLLATE utf8mb3_bin NOT NULL COMMENT 'md5 hashcode of the sequence data string',
  `morgan` longtext CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci COMMENT 'base64 encoded morgan fingerprint of the k-mer(k=3) graph of the sequence, or the morgan fingerprint of the metabolite molecular structure data',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `unique_mol_data` (`molecule_id`,`hashcode`),
  KEY `molecules_idx` (`molecule_id`),
  KEY `search_sequence` (`hashcode`),
  KEY `index_mols_seqs` (`molecule_id`,`hashcode`)
) ENGINE=InnoDB AUTO_INCREMENT=162529106 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='the sequence composition data of the molecule';")>
Public Class sequence_graph: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
''' <summary>
''' the molecule reference id inside biocad registry system
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("molecule_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="molecule_id")> Public Property molecule_id As UInteger
''' <summary>
''' the molecule sequence data, for metabolite should be the SMILES structure string data, gene should be the nucleotide sequence data and protein should be the polypeptide sequence.
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("sequence"), NotNull, DataType(MySqlDbType.Text), Column(Name:="sequence")> Public Property sequence As String
''' <summary>
''' md5 hashcode of the sequence data string
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("hashcode"), NotNull, DataType(MySqlDbType.VarChar, "32"), Column(Name:="hashcode")> Public Property hashcode As String
''' <summary>
''' base64 encoded morgan fingerprint of the k-mer(k=3) graph of the sequence, or the morgan fingerprint of the metabolite molecular structure data
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("morgan"), DataType(MySqlDbType.Text), Column(Name:="morgan")> Public Property morgan As String
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `sequence_graph` (`molecule_id`, `sequence`, `hashcode`, `morgan`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `sequence_graph` (`id`, `molecule_id`, `sequence`, `hashcode`, `morgan`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `sequence_graph` (`molecule_id`, `sequence`, `hashcode`, `morgan`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `sequence_graph` (`id`, `molecule_id`, `sequence`, `hashcode`, `morgan`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `sequence_graph` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `sequence_graph` SET `id`='{0}', `molecule_id`='{1}', `sequence`='{2}', `hashcode`='{3}', `morgan`='{4}', `add_time`='{5}' WHERE `id` = '{6}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `sequence_graph` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `sequence_graph` (`id`, `molecule_id`, `sequence`, `hashcode`, `morgan`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, molecule_id, sequence, hashcode, morgan, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `sequence_graph` (`id`, `molecule_id`, `sequence`, `hashcode`, `morgan`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, molecule_id, sequence, hashcode, morgan, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(INSERT_SQL, molecule_id, sequence, hashcode, morgan, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{molecule_id}', '{sequence}', '{hashcode}', '{morgan}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        Else
            Return $"('{molecule_id}', '{sequence}', '{hashcode}', '{morgan}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `sequence_graph` (`id`, `molecule_id`, `sequence`, `hashcode`, `morgan`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, molecule_id, sequence, hashcode, morgan, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `sequence_graph` (`id`, `molecule_id`, `sequence`, `hashcode`, `morgan`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, molecule_id, sequence, hashcode, morgan, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(REPLACE_SQL, molecule_id, sequence, hashcode, morgan, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `sequence_graph` SET `id`='{0}', `molecule_id`='{1}', `sequence`='{2}', `hashcode`='{3}', `morgan`='{4}', `add_time`='{5}' WHERE `id` = '{6}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, molecule_id, sequence, hashcode, morgan, MySqlScript.ToMySqlDateTimeString(add_time), id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As sequence_graph
                         Return DirectCast(MyClass.MemberwiseClone, sequence_graph)
                     End Function
End Class


End Namespace
