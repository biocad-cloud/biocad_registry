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
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("seq_archive", Database:="cad_registry", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `seq_archive` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `seq_id` VARCHAR(64) NOT NULL,
  `mol_id` INT UNSIGNED NOT NULL,
  `mol_type` INT NOT NULL COMMENT '1: dna\n2: rna\n3: protein\n\nhttps://github.com/SMRUCC/GCModeller/blob/918cb50e86f2159856edaa0531cec227fc7f6e97/src/GCModeller/core/Bio.Assembly/SequenceModel/Abstract.vb#L70',
  `len` INT UNSIGNED NOT NULL DEFAULT 0,
  `hashcode` CHAR(32) NOT NULL COMMENT 'md5 hashcode of the sequence string',
  `seq` LONGTEXT NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;
")>
Public Class seq_archive: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("seq_id"), NotNull, DataType(MySqlDbType.VarChar, "64"), Column(Name:="seq_id")> Public Property seq_id As String
    <DatabaseField("mol_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="mol_id")> Public Property mol_id As UInteger
''' <summary>
''' 1: dna\n2: rna\n3: protein\n\nhttps://github.com/SMRUCC/GCModeller/blob/918cb50e86f2159856edaa0531cec227fc7f6e97/src/GCModeller/core/Bio.Assembly/SequenceModel/Abstract.vb#L70
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("mol_type"), NotNull, DataType(MySqlDbType.Int32, "11"), Column(Name:="mol_type")> Public Property mol_type As Integer
    <DatabaseField("len"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="len")> Public Property len As UInteger
''' <summary>
''' md5 hashcode of the sequence string
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("hashcode"), NotNull, DataType(MySqlDbType.VarChar, "32"), Column(Name:="hashcode")> Public Property hashcode As String
    <DatabaseField("seq"), NotNull, DataType(MySqlDbType.Text), Column(Name:="seq")> Public Property seq As String
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `seq_archive` (`seq_id`, `mol_id`, `mol_type`, `len`, `hashcode`, `seq`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `seq_archive` (`id`, `seq_id`, `mol_id`, `mol_type`, `len`, `hashcode`, `seq`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `seq_archive` (`seq_id`, `mol_id`, `mol_type`, `len`, `hashcode`, `seq`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `seq_archive` (`id`, `seq_id`, `mol_id`, `mol_type`, `len`, `hashcode`, `seq`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `seq_archive` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `seq_archive` SET `id`='{0}', `seq_id`='{1}', `mol_id`='{2}', `mol_type`='{3}', `len`='{4}', `hashcode`='{5}', `seq`='{6}' WHERE `id` = '{7}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `seq_archive` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `seq_archive` (`id`, `seq_id`, `mol_id`, `mol_type`, `len`, `hashcode`, `seq`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, seq_id, mol_id, mol_type, len, hashcode, seq)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `seq_archive` (`id`, `seq_id`, `mol_id`, `mol_type`, `len`, `hashcode`, `seq`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, seq_id, mol_id, mol_type, len, hashcode, seq)
        Else
        Return String.Format(INSERT_SQL, seq_id, mol_id, mol_type, len, hashcode, seq)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{seq_id}', '{mol_id}', '{mol_type}', '{len}', '{hashcode}', '{seq}')"
        Else
            Return $"('{seq_id}', '{mol_id}', '{mol_type}', '{len}', '{hashcode}', '{seq}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `seq_archive` (`id`, `seq_id`, `mol_id`, `mol_type`, `len`, `hashcode`, `seq`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, seq_id, mol_id, mol_type, len, hashcode, seq)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `seq_archive` (`id`, `seq_id`, `mol_id`, `mol_type`, `len`, `hashcode`, `seq`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, seq_id, mol_id, mol_type, len, hashcode, seq)
        Else
        Return String.Format(REPLACE_SQL, seq_id, mol_id, mol_type, len, hashcode, seq)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `seq_archive` SET `id`='{0}', `seq_id`='{1}', `mol_id`='{2}', `mol_type`='{3}', `len`='{4}', `hashcode`='{5}', `seq`='{6}' WHERE `id` = '{7}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, seq_id, mol_id, mol_type, len, hashcode, seq, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As seq_archive
                         Return DirectCast(MyClass.MemberwiseClone, seq_archive)
                     End Function
End Class


End Namespace
