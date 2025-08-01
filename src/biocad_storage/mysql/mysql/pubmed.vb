REM  Oracle.LinuxCompatibility.MySQL.CodeSolution.VisualBasic.CodeGenerator
REM  MYSQL Schema Mapper
REM      for Microsoft VisualBasic.NET 1.0.0.0

REM  Dump @7/26/2025 9:16:16 PM


Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports MySqlScript = Oracle.LinuxCompatibility.MySQL.Scripting.Extensions

Namespace biocad_registryModel

''' <summary>
''' ```SQL
''' 
''' --
''' 
''' DROP TABLE IF EXISTS `pubmed`;
''' /*!40101 SET @saved_cs_client     = @@character_set_client */;
''' /*!50503 SET character_set_client = utf8mb4 */;
''' CREATE TABLE `pubmed` (
'''   `id` int unsigned NOT NULL COMMENT 'the pubmed id of the article',
'''   `authors` mediumtext COLLATE utf8mb3_bin,
'''   `title` mediumtext COLLATE utf8mb3_bin,
'''   `journal` varchar(255) COLLATE utf8mb3_bin DEFAULT NULL,
'''   `year` int unsigned NOT NULL DEFAULT '1900',
'''   `citation` varchar(255) COLLATE utf8mb3_bin NOT NULL DEFAULT '-',
'''   `doi` varchar(100) COLLATE utf8mb3_bin NOT NULL DEFAULT '-',
'''   `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
'''   `affil` longtext COLLATE utf8mb3_bin,
'''   `abstract` longtext COLLATE utf8mb3_bin,
'''   PRIMARY KEY (`id`),
'''   UNIQUE KEY `id_UNIQUE` (`id`),
'''   KEY `index_journal` (`journal`) /*!80000 INVISIBLE */,
'''   KEY `sort_year` (`year` DESC) /*!80000 INVISIBLE */,
'''   KEY `index_doi` (`doi`) /*!80000 INVISIBLE */,
'''   KEY `sort_time` (`add_time`),
'''   FULLTEXT KEY `search_article_txt` (`title`,`abstract`) /*!80000 INVISIBLE */
''' ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;
''' /*!40101 SET character_set_client = @saved_cs_client */;
''' 
''' --
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("pubmed", Database:="cad_registry", SchemaSQL:="
CREATE TABLE `pubmed` (
  `id` int unsigned NOT NULL COMMENT 'the pubmed id of the article',
  `authors` mediumtext COLLATE utf8mb3_bin,
  `title` mediumtext COLLATE utf8mb3_bin,
  `journal` varchar(255) COLLATE utf8mb3_bin DEFAULT NULL,
  `year` int unsigned NOT NULL DEFAULT '1900',
  `citation` varchar(255) COLLATE utf8mb3_bin NOT NULL DEFAULT '-',
  `doi` varchar(100) COLLATE utf8mb3_bin NOT NULL DEFAULT '-',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `affil` longtext COLLATE utf8mb3_bin,
  `abstract` longtext COLLATE utf8mb3_bin,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `index_journal` (`journal`) /*!80000 INVISIBLE */,
  KEY `sort_year` (`year` DESC) /*!80000 INVISIBLE */,
  KEY `index_doi` (`doi`) /*!80000 INVISIBLE */,
  KEY `sort_time` (`add_time`),
  FULLTEXT KEY `search_article_txt` (`title`,`abstract`) /*!80000 INVISIBLE */
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;")>
Public Class pubmed: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
''' <summary>
''' the pubmed id of the article
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("id"), PrimaryKey, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("authors"), DataType(MySqlDbType.Text), Column(Name:="authors")> Public Property authors As String
    <DatabaseField("title"), DataType(MySqlDbType.Text), Column(Name:="title")> Public Property title As String
    <DatabaseField("journal"), DataType(MySqlDbType.VarChar, "255"), Column(Name:="journal")> Public Property journal As String
    <DatabaseField("year"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="year")> Public Property year As UInteger
    <DatabaseField("citation"), NotNull, DataType(MySqlDbType.VarChar, "255"), Column(Name:="citation")> Public Property citation As String
    <DatabaseField("doi"), NotNull, DataType(MySqlDbType.VarChar, "100"), Column(Name:="doi")> Public Property doi As String
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
    <DatabaseField("affil"), DataType(MySqlDbType.Text), Column(Name:="affil")> Public Property affil As String
    <DatabaseField("abstract"), DataType(MySqlDbType.Text), Column(Name:="abstract")> Public Property abstract As String
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `pubmed` (`id`, `authors`, `title`, `journal`, `year`, `citation`, `doi`, `add_time`, `affil`, `abstract`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `pubmed` (`id`, `authors`, `title`, `journal`, `year`, `citation`, `doi`, `add_time`, `affil`, `abstract`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `pubmed` (`id`, `authors`, `title`, `journal`, `year`, `citation`, `doi`, `add_time`, `affil`, `abstract`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `pubmed` (`id`, `authors`, `title`, `journal`, `year`, `citation`, `doi`, `add_time`, `affil`, `abstract`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `pubmed` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `pubmed` SET `id`='{0}', `authors`='{1}', `title`='{2}', `journal`='{3}', `year`='{4}', `citation`='{5}', `doi`='{6}', `add_time`='{7}', `affil`='{8}', `abstract`='{9}' WHERE `id` = '{10}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `pubmed` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `pubmed` (`id`, `authors`, `title`, `journal`, `year`, `citation`, `doi`, `add_time`, `affil`, `abstract`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, id, authors, title, journal, year, citation, doi, MySqlScript.ToMySqlDateTimeString(add_time), affil, abstract)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `pubmed` (`id`, `authors`, `title`, `journal`, `year`, `citation`, `doi`, `add_time`, `affil`, `abstract`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, authors, title, journal, year, citation, doi, MySqlScript.ToMySqlDateTimeString(add_time), affil, abstract)
        Else
        Return String.Format(INSERT_SQL, id, authors, title, journal, year, citation, doi, MySqlScript.ToMySqlDateTimeString(add_time), affil, abstract)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{authors}', '{title}', '{journal}', '{year}', '{citation}', '{doi}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{affil}', '{abstract}')"
        Else
            Return $"('{id}', '{authors}', '{title}', '{journal}', '{year}', '{citation}', '{doi}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{affil}', '{abstract}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `pubmed` (`id`, `authors`, `title`, `journal`, `year`, `citation`, `doi`, `add_time`, `affil`, `abstract`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, id, authors, title, journal, year, citation, doi, MySqlScript.ToMySqlDateTimeString(add_time), affil, abstract)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `pubmed` (`id`, `authors`, `title`, `journal`, `year`, `citation`, `doi`, `add_time`, `affil`, `abstract`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, authors, title, journal, year, citation, doi, MySqlScript.ToMySqlDateTimeString(add_time), affil, abstract)
        Else
        Return String.Format(REPLACE_SQL, id, authors, title, journal, year, citation, doi, MySqlScript.ToMySqlDateTimeString(add_time), affil, abstract)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `pubmed` SET `id`='{0}', `authors`='{1}', `title`='{2}', `journal`='{3}', `year`='{4}', `citation`='{5}', `doi`='{6}', `add_time`='{7}', `affil`='{8}', `abstract`='{9}' WHERE `id` = '{10}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, authors, title, journal, year, citation, doi, MySqlScript.ToMySqlDateTimeString(add_time), affil, abstract, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As pubmed
                         Return DirectCast(MyClass.MemberwiseClone, pubmed)
                     End Function
End Class


End Namespace
