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
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("gene_ontology", Database:="cad_registry", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `gene_ontology` (
  `id` INT UNSIGNED NOT NULL COMMENT 'The go term id',
  `name` VARCHAR(255) NOT NULL,
  `namespace` TINYINT(1) NOT NULL DEFAULT 1 COMMENT 'enum value from https://github.com/SMRUCC/GCModeller/blob/5f709e21e88d4b10ca9ef3f2c2c83585405caa2b/src/GCModeller/data/GO_gene-ontology/GeneOntology/Ontologies.vb#L74\n\n1: cellular_component\n2: biological_process\n3: molecular_function',
  `def` VARCHAR(8192) NOT NULL,
  `is_obsolete` TINYINT NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;
")>
Public Class gene_ontology: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
''' <summary>
''' The go term id
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("id"), PrimaryKey, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("name"), NotNull, DataType(MySqlDbType.VarChar, "255"), Column(Name:="name")> Public Property name As String
''' <summary>
''' enum value from https://github.com/SMRUCC/GCModeller/blob/5f709e21e88d4b10ca9ef3f2c2c83585405caa2b/src/GCModeller/data/GO_gene-ontology/GeneOntology/Ontologies.vb#L74\n\n1: cellular_component\n2: biological_process\n3: molecular_function
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("namespace"), NotNull, DataType(MySqlDbType.Boolean, "1"), Column(Name:="namespace")> Public Property [namespace] As Boolean
    <DatabaseField("def"), NotNull, DataType(MySqlDbType.VarChar, "8192"), Column(Name:="def")> Public Property def As String
    <DatabaseField("is_obsolete"), NotNull, DataType(MySqlDbType.Boolean, "1"), Column(Name:="is_obsolete")> Public Property is_obsolete As Boolean
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `gene_ontology` (`id`, `name`, `namespace`, `def`, `is_obsolete`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `gene_ontology` (`id`, `name`, `namespace`, `def`, `is_obsolete`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `gene_ontology` (`id`, `name`, `namespace`, `def`, `is_obsolete`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `gene_ontology` (`id`, `name`, `namespace`, `def`, `is_obsolete`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `gene_ontology` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `gene_ontology` SET `id`='{0}', `name`='{1}', `namespace`='{2}', `def`='{3}', `is_obsolete`='{4}' WHERE `id` = '{5}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `gene_ontology` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `gene_ontology` (`id`, `name`, `namespace`, `def`, `is_obsolete`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, id, name, [namespace], def, is_obsolete)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `gene_ontology` (`id`, `name`, `namespace`, `def`, `is_obsolete`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, name, [namespace], def, is_obsolete)
        Else
        Return String.Format(INSERT_SQL, id, name, [namespace], def, is_obsolete)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{name}', '{[namespace]}', '{def}', '{is_obsolete}')"
        Else
            Return $"('{id}', '{name}', '{[namespace]}', '{def}', '{is_obsolete}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `gene_ontology` (`id`, `name`, `namespace`, `def`, `is_obsolete`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, id, name, [namespace], def, is_obsolete)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `gene_ontology` (`id`, `name`, `namespace`, `def`, `is_obsolete`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, name, [namespace], def, is_obsolete)
        Else
        Return String.Format(REPLACE_SQL, id, name, [namespace], def, is_obsolete)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `gene_ontology` SET `id`='{0}', `name`='{1}', `namespace`='{2}', `def`='{3}', `is_obsolete`='{4}' WHERE `id` = '{5}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, name, [namespace], def, is_obsolete, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As gene_ontology
                         Return DirectCast(MyClass.MemberwiseClone, gene_ontology)
                     End Function
End Class


End Namespace
