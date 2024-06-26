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
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("reaction_graph", Database:="cad_registry", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `reaction_graph` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `reaction_id` INT UNSIGNED NOT NULL,
  `molecule_id` INT UNSIGNED NOT NULL,
  `name` VARCHAR(255) NOT NULL COMMENT 'the name text of the molecule',
  `stoichiometric` FLOAT NOT NULL DEFAULT 1,
  `type` TINYINT(1) NOT NULL DEFAULT 1 COMMENT '1 - left, substrate; 2 - right, product; 3 - middle, enzyme',
  PRIMARY KEY (`id`))
ENGINE = InnoDB;
")>
Public Class reaction_graph: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("reaction_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="reaction_id")> Public Property reaction_id As UInteger
    <DatabaseField("molecule_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="molecule_id")> Public Property molecule_id As UInteger
''' <summary>
''' the name text of the molecule
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("name"), NotNull, DataType(MySqlDbType.VarChar, "255"), Column(Name:="name")> Public Property name As String
    <DatabaseField("stoichiometric"), NotNull, DataType(MySqlDbType.Double), Column(Name:="stoichiometric")> Public Property stoichiometric As Double
''' <summary>
''' 1 - left, substrate; 2 - right, product; 3 - middle, enzyme
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("type"), NotNull, DataType(MySqlDbType.Boolean, "1"), Column(Name:="type")> Public Property type As Boolean
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `reaction_graph` (`reaction_id`, `molecule_id`, `name`, `stoichiometric`, `type`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `reaction_graph` (`id`, `reaction_id`, `molecule_id`, `name`, `stoichiometric`, `type`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `reaction_graph` (`reaction_id`, `molecule_id`, `name`, `stoichiometric`, `type`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `reaction_graph` (`id`, `reaction_id`, `molecule_id`, `name`, `stoichiometric`, `type`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `reaction_graph` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `reaction_graph` SET `id`='{0}', `reaction_id`='{1}', `molecule_id`='{2}', `name`='{3}', `stoichiometric`='{4}', `type`='{5}' WHERE `id` = '{6}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `reaction_graph` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `reaction_graph` (`id`, `reaction_id`, `molecule_id`, `name`, `stoichiometric`, `type`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, reaction_id, molecule_id, name, stoichiometric, type)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `reaction_graph` (`id`, `reaction_id`, `molecule_id`, `name`, `stoichiometric`, `type`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, reaction_id, molecule_id, name, stoichiometric, type)
        Else
        Return String.Format(INSERT_SQL, reaction_id, molecule_id, name, stoichiometric, type)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{reaction_id}', '{molecule_id}', '{name}', '{stoichiometric}', '{type}')"
        Else
            Return $"('{reaction_id}', '{molecule_id}', '{name}', '{stoichiometric}', '{type}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `reaction_graph` (`id`, `reaction_id`, `molecule_id`, `name`, `stoichiometric`, `type`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, reaction_id, molecule_id, name, stoichiometric, type)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `reaction_graph` (`id`, `reaction_id`, `molecule_id`, `name`, `stoichiometric`, `type`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, reaction_id, molecule_id, name, stoichiometric, type)
        Else
        Return String.Format(REPLACE_SQL, reaction_id, molecule_id, name, stoichiometric, type)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `reaction_graph` SET `id`='{0}', `reaction_id`='{1}', `molecule_id`='{2}', `name`='{3}', `stoichiometric`='{4}', `type`='{5}' WHERE `id` = '{6}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, reaction_id, molecule_id, name, stoichiometric, type, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As reaction_graph
                         Return DirectCast(MyClass.MemberwiseClone, reaction_graph)
                     End Function
End Class


End Namespace
