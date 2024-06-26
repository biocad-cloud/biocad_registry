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
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("operon_graph", Database:="cad_registry", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `operon_graph` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `operon_id` INT UNSIGNED NOT NULL,
  `gene_id` INT UNSIGNED NOT NULL,
  `genome_id` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;
")>
Public Class operon_graph: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("operon_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="operon_id")> Public Property operon_id As UInteger
    <DatabaseField("gene_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="gene_id")> Public Property gene_id As UInteger
    <DatabaseField("genome_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="genome_id")> Public Property genome_id As UInteger
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `operon_graph` (`operon_id`, `gene_id`, `genome_id`) VALUES ('{0}', '{1}', '{2}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `operon_graph` (`id`, `operon_id`, `gene_id`, `genome_id`) VALUES ('{0}', '{1}', '{2}', '{3}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `operon_graph` (`operon_id`, `gene_id`, `genome_id`) VALUES ('{0}', '{1}', '{2}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `operon_graph` (`id`, `operon_id`, `gene_id`, `genome_id`) VALUES ('{0}', '{1}', '{2}', '{3}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `operon_graph` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `operon_graph` SET `id`='{0}', `operon_id`='{1}', `gene_id`='{2}', `genome_id`='{3}' WHERE `id` = '{4}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `operon_graph` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `operon_graph` (`id`, `operon_id`, `gene_id`, `genome_id`) VALUES ('{0}', '{1}', '{2}', '{3}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, operon_id, gene_id, genome_id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `operon_graph` (`id`, `operon_id`, `gene_id`, `genome_id`) VALUES ('{0}', '{1}', '{2}', '{3}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, operon_id, gene_id, genome_id)
        Else
        Return String.Format(INSERT_SQL, operon_id, gene_id, genome_id)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{operon_id}', '{gene_id}', '{genome_id}')"
        Else
            Return $"('{operon_id}', '{gene_id}', '{genome_id}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `operon_graph` (`id`, `operon_id`, `gene_id`, `genome_id`) VALUES ('{0}', '{1}', '{2}', '{3}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, operon_id, gene_id, genome_id)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `operon_graph` (`id`, `operon_id`, `gene_id`, `genome_id`) VALUES ('{0}', '{1}', '{2}', '{3}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, operon_id, gene_id, genome_id)
        Else
        Return String.Format(REPLACE_SQL, operon_id, gene_id, genome_id)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `operon_graph` SET `id`='{0}', `operon_id`='{1}', `gene_id`='{2}', `genome_id`='{3}' WHERE `id` = '{4}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, operon_id, gene_id, genome_id, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As operon_graph
                         Return DirectCast(MyClass.MemberwiseClone, operon_graph)
                     End Function
End Class


End Namespace
