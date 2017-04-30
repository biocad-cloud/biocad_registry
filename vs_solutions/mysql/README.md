# MySQL development docs
Mysql database field attributes notes:

> AI: Auto Increment; B: Binary; NN: Not Null; PK: Primary Key; UQ: Unique; UN: Unsigned; ZF: Zero Fill

## class_ko00001_orthology


|field|type|attributes|description|
|-----|----|----------|-----------|
|Orthology|Int64 (11)|``NN``||
|KEGG|VarChar (45)|||
|name|VarChar (45)|||
|function|VarChar (45)|||
|level_A|VarChar (45)|||
|level_B|VarChar (45)|||
|level_C|VarChar (45)||KEGG pathway|

```SQL
CREATE TABLE `class_ko00001_orthology` (
  `Orthology` int(11) NOT NULL,
  `KEGG` varchar(45) DEFAULT NULL,
  `name` varchar(45) DEFAULT NULL,
  `function` varchar(45) DEFAULT NULL,
  `level_A` varchar(45) DEFAULT NULL,
  `level_B` varchar(45) DEFAULT NULL,
  `level_C` varchar(45) DEFAULT NULL COMMENT 'KEGG pathway',
  PRIMARY KEY (`Orthology`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
```



## class_ko00001_pathway


|field|type|attributes|description|
|-----|----|----------|-----------|
|pathway|Int64 (11)|``NN``||
|KEGG|VarChar (45)|||
|level_A|VarChar (45)|||
|level_B|VarChar (45)|||
|name|VarChar (45)|||

```SQL
CREATE TABLE `class_ko00001_pathway` (
  `pathway` int(11) NOT NULL,
  `KEGG` varchar(45) DEFAULT NULL,
  `level_A` varchar(45) DEFAULT NULL,
  `level_B` varchar(45) DEFAULT NULL,
  `name` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`pathway`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
```



## class_orthology_genes


|field|type|attributes|description|
|-----|----|----------|-----------|
|uid|Int64 (11)|``NN``||
|orthology|VarChar (45)|``NN``||
|locus_tag|VarChar (45)|``NN``||
|organism|VarChar (45)|``NN``||

```SQL
CREATE TABLE `class_orthology_genes` (
  `uid` int(11) NOT NULL,
  `orthology` varchar(45) NOT NULL,
  `locus_tag` varchar(45) NOT NULL,
  `organism` varchar(45) NOT NULL,
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
```



## data_compounds


|field|type|attributes|description|
|-----|----|----------|-----------|
|uid|Int64 (11)|``NN``||
|KEGG|VarChar (45)|||
|names|VarChar (45)|||
|formula|VarChar (45)|||
|mass|VarChar (45)|||
|mol_weight|VarChar (45)|||

```SQL
CREATE TABLE `data_compounds` (
  `uid` int(11) NOT NULL,
  `KEGG` varchar(45) DEFAULT NULL,
  `names` varchar(45) DEFAULT NULL,
  `formula` varchar(45) DEFAULT NULL,
  `mass` varchar(45) DEFAULT NULL,
  `mol_weight` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
```



## data_enzyme


|field|type|attributes|description|
|-----|----|----------|-----------|
|uid|Int64 (11)|``NN``||
|EC|VarChar (45)|||
|name|VarChar (45)|||
|sysname|VarChar (45)|||
|Reaction(KEGG)_uid|VarChar (45)|||
|Reaction(KEGG)|VarChar (45)|||
|Reaction(IUBMB)|VarChar (45)|||
|Substrate|VarChar (45)|||
|Product|VarChar (45)|||
|Comment|VarChar (45)|||

```SQL
CREATE TABLE `data_enzyme` (
  `uid` int(11) NOT NULL,
  `EC` varchar(45) DEFAULT NULL,
  `name` varchar(45) DEFAULT NULL,
  `sysname` varchar(45) DEFAULT NULL,
  `Reaction(KEGG)_uid` varchar(45) DEFAULT NULL,
  `Reaction(KEGG)` varchar(45) DEFAULT NULL,
  `Reaction(IUBMB)` varchar(45) DEFAULT NULL,
  `Substrate` varchar(45) DEFAULT NULL,
  `Product` varchar(45) DEFAULT NULL,
  `Comment` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`uid`),
  UNIQUE KEY `uid_UNIQUE` (`uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
```



## data_modules


|field|type|attributes|description|
|-----|----|----------|-----------|
|uid|Int64 (11)|``NN``||
|KEGG|VarChar (45)|||
|name|VarChar (45)|||
|definition|VarChar (45)|||
|map|VarChar (45)||image -> gzip -> base64 string|

```SQL
CREATE TABLE `data_modules` (
  `uid` int(11) NOT NULL,
  `KEGG` varchar(45) DEFAULT NULL,
  `name` varchar(45) DEFAULT NULL,
  `definition` varchar(45) DEFAULT NULL,
  `map` varchar(45) DEFAULT NULL COMMENT 'image -> gzip -> base64 string',
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
```



## data_orthology


|field|type|attributes|description|
|-----|----|----------|-----------|
|uid|Int64 (11)|``NN``||
|KEGG|VarChar (45)|||
|name|VarChar (45)|||
|definition|VarChar (45)|||

```SQL
CREATE TABLE `data_orthology` (
  `uid` int(11) NOT NULL,
  `KEGG` varchar(45) DEFAULT NULL,
  `name` varchar(45) DEFAULT NULL,
  `definition` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`uid`),
  UNIQUE KEY `uid_UNIQUE` (`uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
```



## data_pathway
参考代谢途径的定义

|field|type|attributes|description|
|-----|----|----------|-----------|
|uid|Int64 (11)|``AI``, ``NN``||
|KO|VarChar (45)|``NN``||
|description|VarChar (45)|||
|name|VarChar (45)|||
|map|VarChar (45)||image -> gzip -> base64 string|

```SQL
CREATE TABLE `data_pathway` (
  `uid` int(11) NOT NULL AUTO_INCREMENT,
  `KO` varchar(45) NOT NULL,
  `description` varchar(45) DEFAULT NULL,
  `name` varchar(45) DEFAULT NULL,
  `map` varchar(45) DEFAULT NULL COMMENT 'image -> gzip -> base64 string',
  PRIMARY KEY (`uid`),
  UNIQUE KEY `uid_UNIQUE` (`uid`),
  UNIQUE KEY `KO_UNIQUE` (`KO`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='参考代谢途径的定义';
```



## data_reactions


|field|type|attributes|description|
|-----|----|----------|-----------|
|uid|Int64 (11)|``NN``||
|KEGG|VarChar (45)|||
|name|VarChar (45)|||
|definition|VarChar (45)|||
|comment|VarChar (45)|||

```SQL
CREATE TABLE `data_reactions` (
  `uid` int(11) NOT NULL,
  `KEGG` varchar(45) DEFAULT NULL,
  `name` varchar(45) DEFAULT NULL,
  `definition` varchar(45) DEFAULT NULL,
  `comment` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
```



## data_references


|field|type|attributes|description|
|-----|----|----------|-----------|
|uid|Int64 (11)|``AI``, ``NN``||
|pmid|Int64 (11)|``NN``||
|journal|VarChar (45)|||
|title|VarChar (45)|``NN``||
|authors|VarChar (45)|||

```SQL
CREATE TABLE `data_references` (
  `uid` int(11) NOT NULL AUTO_INCREMENT,
  `pmid` int(11) NOT NULL,
  `journal` varchar(45) DEFAULT NULL,
  `title` varchar(45) NOT NULL,
  `authors` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
```



## link_enzymes


|field|type|attributes|description|
|-----|----|----------|-----------|
|enzyme|Int64 (11)|``NN``||
|EC|VarChar (45)|||
|database|VarChar (45)|||
|ID|VarChar (45)|||

```SQL
CREATE TABLE `link_enzymes` (
  `enzyme` int(11) NOT NULL,
  `EC` varchar(45) DEFAULT NULL,
  `database` varchar(45) DEFAULT NULL,
  `ID` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`enzyme`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
```



## xref_module_reactions


|field|type|attributes|description|
|-----|----|----------|-----------|
|module|Int64 (11)|``NN``||
|reaction|VarChar (45)|||
|KEGG|VarChar (45)|||

```SQL
CREATE TABLE `xref_module_reactions` (
  `module` int(11) NOT NULL,
  `reaction` varchar(45) DEFAULT NULL,
  `KEGG` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`module`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
```



## xref_pathway_compounds


|field|type|attributes|description|
|-----|----|----------|-----------|
|pathway|Int64 (11)|``NN``||
|compound|Int64 (11)|``NN``||
|KEGG|VarChar (45)||KEGG compound id|
|name|VarChar (45)|||

```SQL
CREATE TABLE `xref_pathway_compounds` (
  `pathway` int(11) NOT NULL,
  `compound` int(11) NOT NULL,
  `KEGG` varchar(45) DEFAULT NULL COMMENT 'KEGG compound id',
  `name` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`pathway`,`compound`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
```



## xref_pathway_genes


|field|type|attributes|description|
|-----|----|----------|-----------|
|pathway|Int64 (11)|``NN``||
|gene|Int64 (11)|``NN``||
|gene_KO|VarChar (45)|||
|locus_tag|VarChar (45)|||
|gene_name|VarChar (45)|||

```SQL
CREATE TABLE `xref_pathway_genes` (
  `pathway` int(11) NOT NULL,
  `gene` int(11) NOT NULL,
  `gene_KO` varchar(45) DEFAULT NULL,
  `locus_tag` varchar(45) DEFAULT NULL,
  `gene_name` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`pathway`,`gene`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
```



## xref_pathway_modules


|field|type|attributes|description|
|-----|----|----------|-----------|
|pathway|Int64 (11)|``NN``||
|module|Int64 (11)|``NN``||
|KO|VarChar (45)|||
|name|VarChar (45)|||

```SQL
CREATE TABLE `xref_pathway_modules` (
  `pathway` int(11) NOT NULL,
  `module` int(11) NOT NULL,
  `KO` varchar(45) DEFAULT NULL,
  `name` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`pathway`,`module`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
```



## xref_pathway_references


|field|type|attributes|description|
|-----|----|----------|-----------|
|pathway|Int64 (11)|``NN``||
|reference|Int64 (11)|``NN``||
|title|VarChar (45)|||

```SQL
CREATE TABLE `xref_pathway_references` (
  `pathway` int(11) NOT NULL,
  `reference` int(11) NOT NULL,
  `title` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`pathway`,`reference`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
```



