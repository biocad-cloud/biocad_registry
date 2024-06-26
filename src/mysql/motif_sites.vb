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
''' motif site sequence and genomics context (organism specific data)
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("motif_sites", Database:="cad_registry", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `motif_sites` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `type` VARCHAR(3) NOT NULL DEFAULT 'TF' COMMENT 'RNA/TF',
  `gene_id` VARCHAR(45) NULL,
  `gene_name` VARCHAR(45) NULL,
  `loci` INT NOT NULL DEFAULT 0 COMMENT 'the upstream location of the motif site, value could be negative',
  `score` DOUBLE NOT NULL DEFAULT 0.0,
  `regulator` INT UNSIGNED NOT NULL,
  `genome_id` INT UNSIGNED NOT NULL,
  `motif_id` INT UNSIGNED NOT NULL DEFAULT 0 COMMENT 'link to the organism non-specific motif PWM model data',
  `len` INT UNSIGNED NOT NULL DEFAULT 0,
  `hashcode` VARCHAR(32) NOT NULL,
  `family` VARCHAR(45) NULL,
  `seq` VARCHAR(8192) NOT NULL COMMENT 'the motif site sequence',
  PRIMARY KEY (`id`))
ENGINE = InnoDB
COMMENT = 'motif site sequence and genomics context (organism specific data)';
")>
Public Class motif_sites: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
''' <summary>
''' RNA/TF
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("type"), NotNull, DataType(MySqlDbType.VarChar, "3"), Column(Name:="type")> Public Property type As String
    <DatabaseField("gene_id"), DataType(MySqlDbType.VarChar, "45"), Column(Name:="gene_id")> Public Property gene_id As String
    <DatabaseField("gene_name"), DataType(MySqlDbType.VarChar, "45"), Column(Name:="gene_name")> Public Property gene_name As String
''' <summary>
''' the upstream location of the motif site, value could be negative
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("loci"), NotNull, DataType(MySqlDbType.Int32, "11"), Column(Name:="loci")> Public Property loci As Integer
    <DatabaseField("score"), NotNull, DataType(MySqlDbType.Double), Column(Name:="score")> Public Property score As Double
    <DatabaseField("regulator"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="regulator")> Public Property regulator As UInteger
    <DatabaseField("genome_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="genome_id")> Public Property genome_id As UInteger
''' <summary>
''' link to the organism non-specific motif PWM model data
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("motif_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="motif_id")> Public Property motif_id As UInteger
    <DatabaseField("len"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="len")> Public Property len As UInteger
    <DatabaseField("hashcode"), NotNull, DataType(MySqlDbType.VarChar, "32"), Column(Name:="hashcode")> Public Property hashcode As String
    <DatabaseField("family"), DataType(MySqlDbType.VarChar, "45"), Column(Name:="family")> Public Property family As String
''' <summary>
''' the motif site sequence
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("seq"), NotNull, DataType(MySqlDbType.VarChar, "8192"), Column(Name:="seq")> Public Property seq As String
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `motif_sites` (`type`, `gene_id`, `gene_name`, `loci`, `score`, `regulator`, `genome_id`, `motif_id`, `len`, `hashcode`, `family`, `seq`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `motif_sites` (`id`, `type`, `gene_id`, `gene_name`, `loci`, `score`, `regulator`, `genome_id`, `motif_id`, `len`, `hashcode`, `family`, `seq`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `motif_sites` (`type`, `gene_id`, `gene_name`, `loci`, `score`, `regulator`, `genome_id`, `motif_id`, `len`, `hashcode`, `family`, `seq`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `motif_sites` (`id`, `type`, `gene_id`, `gene_name`, `loci`, `score`, `regulator`, `genome_id`, `motif_id`, `len`, `hashcode`, `family`, `seq`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `motif_sites` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `motif_sites` SET `id`='{0}', `type`='{1}', `gene_id`='{2}', `gene_name`='{3}', `loci`='{4}', `score`='{5}', `regulator`='{6}', `genome_id`='{7}', `motif_id`='{8}', `len`='{9}', `hashcode`='{10}', `family`='{11}', `seq`='{12}' WHERE `id` = '{13}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `motif_sites` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `motif_sites` (`id`, `type`, `gene_id`, `gene_name`, `loci`, `score`, `regulator`, `genome_id`, `motif_id`, `len`, `hashcode`, `family`, `seq`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, type, gene_id, gene_name, loci, score, regulator, genome_id, motif_id, len, hashcode, family, seq)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `motif_sites` (`id`, `type`, `gene_id`, `gene_name`, `loci`, `score`, `regulator`, `genome_id`, `motif_id`, `len`, `hashcode`, `family`, `seq`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, type, gene_id, gene_name, loci, score, regulator, genome_id, motif_id, len, hashcode, family, seq)
        Else
        Return String.Format(INSERT_SQL, type, gene_id, gene_name, loci, score, regulator, genome_id, motif_id, len, hashcode, family, seq)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{type}', '{gene_id}', '{gene_name}', '{loci}', '{score}', '{regulator}', '{genome_id}', '{motif_id}', '{len}', '{hashcode}', '{family}', '{seq}')"
        Else
            Return $"('{type}', '{gene_id}', '{gene_name}', '{loci}', '{score}', '{regulator}', '{genome_id}', '{motif_id}', '{len}', '{hashcode}', '{family}', '{seq}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `motif_sites` (`id`, `type`, `gene_id`, `gene_name`, `loci`, `score`, `regulator`, `genome_id`, `motif_id`, `len`, `hashcode`, `family`, `seq`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, type, gene_id, gene_name, loci, score, regulator, genome_id, motif_id, len, hashcode, family, seq)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `motif_sites` (`id`, `type`, `gene_id`, `gene_name`, `loci`, `score`, `regulator`, `genome_id`, `motif_id`, `len`, `hashcode`, `family`, `seq`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, type, gene_id, gene_name, loci, score, regulator, genome_id, motif_id, len, hashcode, family, seq)
        Else
        Return String.Format(REPLACE_SQL, type, gene_id, gene_name, loci, score, regulator, genome_id, motif_id, len, hashcode, family, seq)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `motif_sites` SET `id`='{0}', `type`='{1}', `gene_id`='{2}', `gene_name`='{3}', `loci`='{4}', `score`='{5}', `regulator`='{6}', `genome_id`='{7}', `motif_id`='{8}', `len`='{9}', `hashcode`='{10}', `family`='{11}', `seq`='{12}' WHERE `id` = '{13}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, type, gene_id, gene_name, loci, score, regulator, genome_id, motif_id, len, hashcode, family, seq, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As motif_sites
                         Return DirectCast(MyClass.MemberwiseClone, motif_sites)
                     End Function
End Class


End Namespace
