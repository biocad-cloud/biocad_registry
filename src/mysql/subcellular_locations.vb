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
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("subcellular_locations", Database:="cad_registry", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `subcellular_locations` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `biological_process` INT UNSIGNED NOT NULL,
  `compartment` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;
")>
Public Class subcellular_locations: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("biological_process"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="biological_process")> Public Property biological_process As UInteger
    <DatabaseField("compartment"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="compartment")> Public Property compartment As UInteger
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `subcellular_locations` (`biological_process`, `compartment`) VALUES ('{0}', '{1}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `subcellular_locations` (`id`, `biological_process`, `compartment`) VALUES ('{0}', '{1}', '{2}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `subcellular_locations` (`biological_process`, `compartment`) VALUES ('{0}', '{1}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `subcellular_locations` (`id`, `biological_process`, `compartment`) VALUES ('{0}', '{1}', '{2}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `subcellular_locations` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `subcellular_locations` SET `id`='{0}', `biological_process`='{1}', `compartment`='{2}' WHERE `id` = '{3}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `subcellular_locations` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `subcellular_locations` (`id`, `biological_process`, `compartment`) VALUES ('{0}', '{1}', '{2}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, biological_process, compartment)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `subcellular_locations` (`id`, `biological_process`, `compartment`) VALUES ('{0}', '{1}', '{2}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, biological_process, compartment)
        Else
        Return String.Format(INSERT_SQL, biological_process, compartment)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{biological_process}', '{compartment}')"
        Else
            Return $"('{biological_process}', '{compartment}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `subcellular_locations` (`id`, `biological_process`, `compartment`) VALUES ('{0}', '{1}', '{2}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, biological_process, compartment)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `subcellular_locations` (`id`, `biological_process`, `compartment`) VALUES ('{0}', '{1}', '{2}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, biological_process, compartment)
        Else
        Return String.Format(REPLACE_SQL, biological_process, compartment)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `subcellular_locations` SET `id`='{0}', `biological_process`='{1}', `compartment`='{2}' WHERE `id` = '{3}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, biological_process, compartment, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As subcellular_locations
                         Return DirectCast(MyClass.MemberwiseClone, subcellular_locations)
                     End Function
End Class


End Namespace
