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
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("regulator", Database:="cad_registry", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `regulator` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `gene_id` VARCHAR(64) NOT NULL,
  `type` VARCHAR(3) NOT NULL COMMENT 'TF/RNA',
  `mol_id` INT UNSIGNED NOT NULL,
  `family_id` INT UNSIGNED NOT NULL,
  `family` VARCHAR(45) NOT NULL,
  `genome_id` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;
")>
Public Class regulator: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("gene_id"), NotNull, DataType(MySqlDbType.VarChar, "64"), Column(Name:="gene_id")> Public Property gene_id As String
''' <summary>
''' TF/RNA
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("type"), NotNull, DataType(MySqlDbType.VarChar, "3"), Column(Name:="type")> Public Property type As String
    <DatabaseField("mol_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="mol_id")> Public Property mol_id As UInteger
    <DatabaseField("family_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="family_id")> Public Property family_id As UInteger
    <DatabaseField("family"), NotNull, DataType(MySqlDbType.VarChar, "45"), Column(Name:="family")> Public Property family As String
    <DatabaseField("genome_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="genome_id")> Public Property genome_id As UInteger
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `regulator` (`gene_id`, `type`, `mol_id`, `family_id`, `family`, `genome_id`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `regulator` (`id`, `gene_id`, `type`, `mol_id`, `family_id`, `family`, `genome_id`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `regulator` (`gene_id`, `type`, `mol_id`, `family_id`, `family`, `genome_id`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `regulator` (`id`, `gene_id`, `type`, `mol_id`, `family_id`, `family`, `genome_id`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `regulator` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `regulator` SET `id`='{0}', `gene_id`='{1}', `type`='{2}', `mol_id`='{3}', `family_id`='{4}', `family`='{5}', `genome_id`='{6}' WHERE `id` = '{7}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `regulator` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `regulator` (`id`, `gene_id`, `type`, `mol_id`, `family_id`, `family`, `genome_id`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, gene_id, type, mol_id, family_id, family, genome_id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `regulator` (`id`, `gene_id`, `type`, `mol_id`, `family_id`, `family`, `genome_id`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, gene_id, type, mol_id, family_id, family, genome_id)
        Else
        Return String.Format(INSERT_SQL, gene_id, type, mol_id, family_id, family, genome_id)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{gene_id}', '{type}', '{mol_id}', '{family_id}', '{family}', '{genome_id}')"
        Else
            Return $"('{gene_id}', '{type}', '{mol_id}', '{family_id}', '{family}', '{genome_id}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `regulator` (`id`, `gene_id`, `type`, `mol_id`, `family_id`, `family`, `genome_id`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, gene_id, type, mol_id, family_id, family, genome_id)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `regulator` (`id`, `gene_id`, `type`, `mol_id`, `family_id`, `family`, `genome_id`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, gene_id, type, mol_id, family_id, family, genome_id)
        Else
        Return String.Format(REPLACE_SQL, gene_id, type, mol_id, family_id, family, genome_id)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `regulator` SET `id`='{0}', `gene_id`='{1}', `type`='{2}', `mol_id`='{3}', `family_id`='{4}', `family`='{5}', `genome_id`='{6}' WHERE `id` = '{7}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, gene_id, type, mol_id, family_id, family, genome_id, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As regulator
                         Return DirectCast(MyClass.MemberwiseClone, regulator)
                     End Function
End Class


End Namespace
