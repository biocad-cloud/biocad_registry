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
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("go_dag", Database:="cad_registry", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `go_dag` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `term_id` INT UNSIGNED NOT NULL COMMENT 'current go term id',
  `go_term` INT UNSIGNED NOT NULL COMMENT 'another go term id',
  `relationship` INT UNSIGNED NOT NULL COMMENT '1: is_a, example as term_id is_a go_term',
  PRIMARY KEY (`id`))
ENGINE = InnoDB;
")>
Public Class go_dag: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
''' <summary>
''' current go term id
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("term_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="term_id")> Public Property term_id As UInteger
''' <summary>
''' another go term id
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("go_term"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="go_term")> Public Property go_term As UInteger
''' <summary>
''' 1: is_a, example as term_id is_a go_term
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("relationship"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="relationship")> Public Property relationship As UInteger
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `go_dag` (`term_id`, `go_term`, `relationship`) VALUES ('{0}', '{1}', '{2}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `go_dag` (`id`, `term_id`, `go_term`, `relationship`) VALUES ('{0}', '{1}', '{2}', '{3}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `go_dag` (`term_id`, `go_term`, `relationship`) VALUES ('{0}', '{1}', '{2}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `go_dag` (`id`, `term_id`, `go_term`, `relationship`) VALUES ('{0}', '{1}', '{2}', '{3}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `go_dag` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `go_dag` SET `id`='{0}', `term_id`='{1}', `go_term`='{2}', `relationship`='{3}' WHERE `id` = '{4}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `go_dag` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `go_dag` (`id`, `term_id`, `go_term`, `relationship`) VALUES ('{0}', '{1}', '{2}', '{3}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, term_id, go_term, relationship)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `go_dag` (`id`, `term_id`, `go_term`, `relationship`) VALUES ('{0}', '{1}', '{2}', '{3}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, term_id, go_term, relationship)
        Else
        Return String.Format(INSERT_SQL, term_id, go_term, relationship)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{term_id}', '{go_term}', '{relationship}')"
        Else
            Return $"('{term_id}', '{go_term}', '{relationship}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `go_dag` (`id`, `term_id`, `go_term`, `relationship`) VALUES ('{0}', '{1}', '{2}', '{3}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, term_id, go_term, relationship)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `go_dag` (`id`, `term_id`, `go_term`, `relationship`) VALUES ('{0}', '{1}', '{2}', '{3}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, term_id, go_term, relationship)
        Else
        Return String.Format(REPLACE_SQL, term_id, go_term, relationship)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `go_dag` SET `id`='{0}', `term_id`='{1}', `go_term`='{2}', `relationship`='{3}' WHERE `id` = '{4}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, term_id, go_term, relationship, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As go_dag
                         Return DirectCast(MyClass.MemberwiseClone, go_dag)
                     End Function
End Class


End Namespace
