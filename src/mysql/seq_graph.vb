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
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("seq_graph", Database:="cad_registry", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `seq_graph` (
  `id` INT UNSIGNED NOT NULL COMMENT 'the id of the seq_archive, so this id is not an auto-increment index',
  `title` VARCHAR(4096) NOT NULL,
  `add_time` DATETIME NOT NULL DEFAULT now(),
  `hashcode` VARCHAR(32) NOT NULL COMMENT 'md5 of the graph_base64 string',
  `graph_base64` LONGTEXT NOT NULL COMMENT 'base64 encoded string based on the zlib compresion of seq graph values',
  PRIMARY KEY (`id`))
ENGINE = InnoDB;
")>
Public Class seq_graph: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
''' <summary>
''' the id of the seq_archive, so this id is not an auto-increment index
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("id"), PrimaryKey, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("title"), NotNull, DataType(MySqlDbType.VarChar, "4096"), Column(Name:="title")> Public Property title As String
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
''' <summary>
''' md5 of the graph_base64 string
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("hashcode"), NotNull, DataType(MySqlDbType.VarChar, "32"), Column(Name:="hashcode")> Public Property hashcode As String
''' <summary>
''' base64 encoded string based on the zlib compresion of seq graph values
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("graph_base64"), NotNull, DataType(MySqlDbType.Text), Column(Name:="graph_base64")> Public Property graph_base64 As String
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `seq_graph` (`id`, `title`, `add_time`, `hashcode`, `graph_base64`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `seq_graph` (`id`, `title`, `add_time`, `hashcode`, `graph_base64`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `seq_graph` (`id`, `title`, `add_time`, `hashcode`, `graph_base64`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `seq_graph` (`id`, `title`, `add_time`, `hashcode`, `graph_base64`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `seq_graph` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `seq_graph` SET `id`='{0}', `title`='{1}', `add_time`='{2}', `hashcode`='{3}', `graph_base64`='{4}' WHERE `id` = '{5}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `seq_graph` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `seq_graph` (`id`, `title`, `add_time`, `hashcode`, `graph_base64`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, id, title, MySqlScript.ToMySqlDateTimeString(add_time), hashcode, graph_base64)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `seq_graph` (`id`, `title`, `add_time`, `hashcode`, `graph_base64`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, title, MySqlScript.ToMySqlDateTimeString(add_time), hashcode, graph_base64)
        Else
        Return String.Format(INSERT_SQL, id, title, MySqlScript.ToMySqlDateTimeString(add_time), hashcode, graph_base64)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{title}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{hashcode}', '{graph_base64}')"
        Else
            Return $"('{id}', '{title}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{hashcode}', '{graph_base64}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `seq_graph` (`id`, `title`, `add_time`, `hashcode`, `graph_base64`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, id, title, MySqlScript.ToMySqlDateTimeString(add_time), hashcode, graph_base64)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `seq_graph` (`id`, `title`, `add_time`, `hashcode`, `graph_base64`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, title, MySqlScript.ToMySqlDateTimeString(add_time), hashcode, graph_base64)
        Else
        Return String.Format(REPLACE_SQL, id, title, MySqlScript.ToMySqlDateTimeString(add_time), hashcode, graph_base64)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `seq_graph` SET `id`='{0}', `title`='{1}', `add_time`='{2}', `hashcode`='{3}', `graph_base64`='{4}' WHERE `id` = '{5}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, title, MySqlScript.ToMySqlDateTimeString(add_time), hashcode, graph_base64, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As seq_graph
                         Return DirectCast(MyClass.MemberwiseClone, seq_graph)
                     End Function
End Class


End Namespace
