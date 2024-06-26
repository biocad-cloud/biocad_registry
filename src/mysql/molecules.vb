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
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("molecules", Database:="cad_registry", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `molecules` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `molecule_id` VARCHAR(64) NOT NULL,
  `type` INT NOT NULL DEFAULT 1 COMMENT '1: gene(dna), rna, polypeptide(protein)\n2: metabolite\n3: protein-complex\n\n',
  `name` VARCHAR(64) NOT NULL,
  `seq_num` INT NOT NULL DEFAULT 0,
  `synonym_num` INT UNSIGNED NOT NULL DEFAULT 0,
  `ncbi_taxid` INT UNSIGNED NOT NULL DEFAULT 0 COMMENT 'the genome source of current molecule',
  `category_id` INT UNSIGNED NOT NULL DEFAULT 0,
  `description` VARCHAR(8192) NULL,
  `add_time` DATETIME NOT NULL DEFAULT now(),
  PRIMARY KEY (`id`))
ENGINE = InnoDB;
")>
Public Class molecules: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("molecule_id"), NotNull, DataType(MySqlDbType.VarChar, "64"), Column(Name:="molecule_id")> Public Property molecule_id As String
''' <summary>
''' 1: gene(dna), rna, polypeptide(protein)\n2: metabolite\n3: protein-complex\n\n
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("type"), NotNull, DataType(MySqlDbType.Int32, "11"), Column(Name:="type")> Public Property type As Integer
    <DatabaseField("name"), NotNull, DataType(MySqlDbType.VarChar, "64"), Column(Name:="name")> Public Property name As String
    <DatabaseField("seq_num"), NotNull, DataType(MySqlDbType.Int32, "11"), Column(Name:="seq_num")> Public Property seq_num As Integer
    <DatabaseField("synonym_num"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="synonym_num")> Public Property synonym_num As UInteger
''' <summary>
''' the genome source of current molecule
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("ncbi_taxid"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="ncbi_taxid")> Public Property ncbi_taxid As UInteger
    <DatabaseField("category_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="category_id")> Public Property category_id As UInteger
    <DatabaseField("description"), DataType(MySqlDbType.VarChar, "8192"), Column(Name:="description")> Public Property description As String
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `molecules` (`molecule_id`, `type`, `name`, `seq_num`, `synonym_num`, `ncbi_taxid`, `category_id`, `description`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `molecules` (`id`, `molecule_id`, `type`, `name`, `seq_num`, `synonym_num`, `ncbi_taxid`, `category_id`, `description`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `molecules` (`molecule_id`, `type`, `name`, `seq_num`, `synonym_num`, `ncbi_taxid`, `category_id`, `description`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `molecules` (`id`, `molecule_id`, `type`, `name`, `seq_num`, `synonym_num`, `ncbi_taxid`, `category_id`, `description`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `molecules` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `molecules` SET `id`='{0}', `molecule_id`='{1}', `type`='{2}', `name`='{3}', `seq_num`='{4}', `synonym_num`='{5}', `ncbi_taxid`='{6}', `category_id`='{7}', `description`='{8}', `add_time`='{9}' WHERE `id` = '{10}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `molecules` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `molecules` (`id`, `molecule_id`, `type`, `name`, `seq_num`, `synonym_num`, `ncbi_taxid`, `category_id`, `description`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, molecule_id, type, name, seq_num, synonym_num, ncbi_taxid, category_id, description, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `molecules` (`id`, `molecule_id`, `type`, `name`, `seq_num`, `synonym_num`, `ncbi_taxid`, `category_id`, `description`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, molecule_id, type, name, seq_num, synonym_num, ncbi_taxid, category_id, description, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(INSERT_SQL, molecule_id, type, name, seq_num, synonym_num, ncbi_taxid, category_id, description, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{molecule_id}', '{type}', '{name}', '{seq_num}', '{synonym_num}', '{ncbi_taxid}', '{category_id}', '{description}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        Else
            Return $"('{molecule_id}', '{type}', '{name}', '{seq_num}', '{synonym_num}', '{ncbi_taxid}', '{category_id}', '{description}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `molecules` (`id`, `molecule_id`, `type`, `name`, `seq_num`, `synonym_num`, `ncbi_taxid`, `category_id`, `description`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, molecule_id, type, name, seq_num, synonym_num, ncbi_taxid, category_id, description, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `molecules` (`id`, `molecule_id`, `type`, `name`, `seq_num`, `synonym_num`, `ncbi_taxid`, `category_id`, `description`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, molecule_id, type, name, seq_num, synonym_num, ncbi_taxid, category_id, description, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(REPLACE_SQL, molecule_id, type, name, seq_num, synonym_num, ncbi_taxid, category_id, description, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `molecules` SET `id`='{0}', `molecule_id`='{1}', `type`='{2}', `name`='{3}', `seq_num`='{4}', `synonym_num`='{5}', `ncbi_taxid`='{6}', `category_id`='{7}', `description`='{8}', `add_time`='{9}' WHERE `id` = '{10}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, molecule_id, type, name, seq_num, synonym_num, ncbi_taxid, category_id, description, MySqlScript.ToMySqlDateTimeString(add_time), id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As molecules
                         Return DirectCast(MyClass.MemberwiseClone, molecules)
                     End Function
End Class


End Namespace
