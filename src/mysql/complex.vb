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
    <Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("complex", Database:="cad_registry", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `complex` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `molecule_id` INT UNSIGNED NOT NULL COMMENT 'the molecules id of current complex entity',
  `component_id` INT UNSIGNED NOT NULL COMMENT 'One part of the component of current complex, the molecules id',
  `n_components` INT UNSIGNED NOT NULL DEFAULT 0 COMMENT 'the number of the component parts of current complex',
  `name` VARCHAR(2048) NOT NULL COMMENT 'the name of current complex',
  PRIMARY KEY (`id`))
ENGINE = InnoDB;
")>
    Public Class complex : Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
        <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNULL, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
        ''' <summary>
        ''' the molecules id of current complex entity
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DatabaseField("molecule_id"), NotNULL, DataType(MySqlDbType.UInt32, "11"), Column(Name:="molecule_id")> Public Property molecule_id As UInteger
        ''' <summary>
        ''' One part of the component of current complex, the molecules id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DatabaseField("component_id"), NotNULL, DataType(MySqlDbType.UInt32, "11"), Column(Name:="component_id")> Public Property component_id As UInteger
        ''' <summary>
        ''' the number of the component parts of current complex
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DatabaseField("n_components"), NotNULL, DataType(MySqlDbType.UInt32, "11"), Column(Name:="n_components")> Public Property n_components As UInteger
        ''' <summary>
        ''' the name of current complex
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DatabaseField("name"), NotNULL, DataType(MySqlDbType.VarChar, "2048"), Column(Name:="name")> Public Property name As String
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
        Friend Shared ReadOnly INSERT_SQL$ =
        <SQL>INSERT INTO `complex` (`molecule_id`, `component_id`, `n_components`, `name`) VALUES ('{0}', '{1}', '{2}', '{3}');</SQL>

        Friend Shared ReadOnly INSERT_AI_SQL$ =
        <SQL>INSERT INTO `complex` (`id`, `molecule_id`, `component_id`, `n_components`, `name`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

        Friend Shared ReadOnly REPLACE_SQL$ =
        <SQL>REPLACE INTO `complex` (`molecule_id`, `component_id`, `n_components`, `name`) VALUES ('{0}', '{1}', '{2}', '{3}');</SQL>

        Friend Shared ReadOnly REPLACE_AI_SQL$ =
        <SQL>REPLACE INTO `complex` (`id`, `molecule_id`, `component_id`, `n_components`, `name`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

        Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `complex` WHERE `id` = '{0}';</SQL>

        Friend Shared ReadOnly UPDATE_SQL$ =
        <SQL>UPDATE `complex` SET `id`='{0}', `molecule_id`='{1}', `component_id`='{2}', `n_components`='{3}', `name`='{4}' WHERE `id` = '{5}';</SQL>

#End Region

        ''' <summary>
        ''' ```SQL
        ''' DELETE FROM `complex` WHERE `id` = '{0}';
        ''' ```
        ''' </summary>
        Public Overrides Function GetDeleteSQL() As String
            Return String.Format(DELETE_SQL, id)
        End Function

        ''' <summary>
        ''' ```SQL
        ''' INSERT INTO `complex` (`id`, `molecule_id`, `component_id`, `n_components`, `name`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
        ''' ```
        ''' </summary>
        Public Overrides Function GetInsertSQL() As String
            Return String.Format(INSERT_SQL, molecule_id, component_id, n_components, name)
        End Function

        ''' <summary>
        ''' ```SQL
        ''' INSERT INTO `complex` (`id`, `molecule_id`, `component_id`, `n_components`, `name`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
        ''' ```
        ''' </summary>
        Public Overrides Function GetInsertSQL(AI As Boolean) As String
            If AI Then
                Return String.Format(INSERT_AI_SQL, id, molecule_id, component_id, n_components, name)
            Else
                Return String.Format(INSERT_SQL, molecule_id, component_id, n_components, name)
            End If
        End Function

        ''' <summary>
        ''' <see cref="GetInsertSQL"/>
        ''' </summary>
        Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
            If AI Then
                Return $"('{id}', '{molecule_id}', '{component_id}', '{n_components}', '{name}')"
            Else
                Return $"('{molecule_id}', '{component_id}', '{n_components}', '{name}')"
            End If
        End Function


        ''' <summary>
        ''' ```SQL
        ''' REPLACE INTO `complex` (`id`, `molecule_id`, `component_id`, `n_components`, `name`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
        ''' ```
        ''' </summary>
        Public Overrides Function GetReplaceSQL() As String
            Return String.Format(REPLACE_SQL, molecule_id, component_id, n_components, name)
        End Function

        ''' <summary>
        ''' ```SQL
        ''' REPLACE INTO `complex` (`id`, `molecule_id`, `component_id`, `n_components`, `name`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
        ''' ```
        ''' </summary>
        Public Overrides Function GetReplaceSQL(AI As Boolean) As String
            If AI Then
                Return String.Format(REPLACE_AI_SQL, id, molecule_id, component_id, n_components, name)
            Else
                Return String.Format(REPLACE_SQL, molecule_id, component_id, n_components, name)
            End If
        End Function

        ''' <summary>
        ''' ```SQL
        ''' UPDATE `complex` SET `id`='{0}', `molecule_id`='{1}', `component_id`='{2}', `n_components`='{3}', `name`='{4}' WHERE `id` = '{5}';
        ''' ```
        ''' </summary>
        Public Overrides Function GetUpdateSQL() As String
            Return String.Format(UPDATE_SQL, id, molecule_id, component_id, n_components, name, id)
        End Function
#End Region

        ''' <summary>
        ''' Memberwise clone of current table Object.
        ''' </summary>
        Public Function Clone() As complex
            Return DirectCast(MyClass.MemberwiseClone, complex)
        End Function
    End Class


End Namespace
