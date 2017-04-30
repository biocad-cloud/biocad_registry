# MySQL development docs
Mysql database field attributes notes:

> AI: Auto Increment; B: Binary; NN: Not Null; PK: Primary Key; UQ: Unique; UN: Unsigned; ZF: Zero Fill

## class_ko00001_orthology
KEGG的基因同源分类

|field|type|attributes|description|
|-----|----|----------|-----------|
|Orthology|Int64 (11)|``NN``|``data_orthology``基因同源数据表之中的唯一数字编号|
|KEGG|VarChar (45)||当前的这个基因同源的KO编号|
|name|VarChar (45)||基因名|
|function|VarChar (45)||功能描述|
|level_A|VarChar (45)||代谢途径大分类|
|level_B|VarChar (45)||代谢途径小分类|
|level_C|VarChar (45)||KEGG pathway.当前的这个参考基因同源所处的代谢途径|

```SQL
CREATE TABLE `class_ko00001_orthology` (
  `Orthology` int(11) NOT NULL COMMENT '``data_orthology``基因同源数据表之中的唯一数字编号',
  `KEGG` varchar(45) DEFAULT NULL COMMENT '当前的这个基因同源的KO编号',
  `name` varchar(45) DEFAULT NULL COMMENT '基因名',
  `function` varchar(45) DEFAULT NULL COMMENT '功能描述',
  `level_A` varchar(45) DEFAULT NULL COMMENT '代谢途径大分类',
  `level_B` varchar(45) DEFAULT NULL COMMENT '代谢途径小分类',
  `level_C` varchar(45) DEFAULT NULL COMMENT 'KEGG pathway.当前的这个参考基因同源所处的代谢途径',
  PRIMARY KEY (`Orthology`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='KEGG的基因同源分类';
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
这个数据表描述了uniprot之中的基因蛋白数据之间的基因同源关系

|field|type|attributes|description|
|-----|----|----------|-----------|
|uid|Int64 (11)|``NN``||
|orthology|Int64 (11)|``NN``|直系同源表的数字编号|
|locus_tag|VarChar (45)|``NN``|基因号|
|geneName|VarChar (45)||基因名，因为有些基因还是没有名称的，所以在这里可以为空|
|organism|VarChar (45)|``NN``|KEGG物种简写编号|

```SQL
CREATE TABLE `class_orthology_genes` (
  `uid` int(11) NOT NULL,
  `orthology` int(11) NOT NULL COMMENT '直系同源表的数字编号',
  `locus_tag` varchar(45) NOT NULL COMMENT '基因号',
  `geneName` varchar(45) DEFAULT NULL COMMENT '基因名，因为有些基因还是没有名称的，所以在这里可以为空',
  `organism` varchar(45) NOT NULL COMMENT 'KEGG物种简写编号',
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='这个数据表描述了uniprot之中的基因蛋白数据之间的基因同源关系';
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
酶

|field|type|attributes|description|
|-----|----|----------|-----------|
|uid|Int64 (11)|``NN``||
|EC|VarChar (45)||EC编号|
|name|VarChar (45)||酶名称|
|sysname|VarChar (45)||生物酶的系统名称|
|Reaction(KEGG)_uid|VarChar (45)||``data_reactions``表之中的数字编号|
|Reaction(KEGG)|VarChar (45)||KEGG之中所能够被催化的生物过程的编号|
|Reaction(IUBMB)|VarChar (45)|||
|Substrate|VarChar (45)|||
|Product|VarChar (45)|||
|Comment|VarChar (45)|||

```SQL
CREATE TABLE `data_enzyme` (
  `uid` int(11) NOT NULL,
  `EC` varchar(45) DEFAULT NULL COMMENT 'EC编号',
  `name` varchar(45) DEFAULT NULL COMMENT '酶名称',
  `sysname` varchar(45) DEFAULT NULL COMMENT '生物酶的系统名称',
  `Reaction(KEGG)_uid` varchar(45) DEFAULT NULL COMMENT '``data_reactions``表之中的数字编号',
  `Reaction(KEGG)` varchar(45) DEFAULT NULL COMMENT 'KEGG之中所能够被催化的生物过程的编号',
  `Reaction(IUBMB)` varchar(45) DEFAULT NULL,
  `Substrate` varchar(45) DEFAULT NULL,
  `Product` varchar(45) DEFAULT NULL,
  `Comment` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`uid`),
  UNIQUE KEY `uid_UNIQUE` (`uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='酶';
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
KEGG基因直系同源数据

|field|type|attributes|description|
|-----|----|----------|-----------|
|uid|Int64 (11)|``NN``||
|KEGG|VarChar (45)||KO编号|
|name|VarChar (45)|||
|definition|VarChar (45)|||

```SQL
CREATE TABLE `data_orthology` (
  `uid` int(11) NOT NULL,
  `KEGG` varchar(45) DEFAULT NULL COMMENT 'KO编号',
  `name` varchar(45) DEFAULT NULL,
  `definition` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`uid`),
  UNIQUE KEY `uid_UNIQUE` (`uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='KEGG基因直系同源数据';
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
代谢途径和所属于该代谢途径对象的基因之间的关系表

|field|type|attributes|description|
|-----|----|----------|-----------|
|pathway|Int64 (11)|``NN``||
|gene|Int64 (11)|``NN``||
|gene_KO|VarChar (45)||目标基因的KO分类编号|
|locus_tag|VarChar (45)||基因号|
|gene_name|VarChar (45)|||

```SQL
CREATE TABLE `xref_pathway_genes` (
  `pathway` int(11) NOT NULL,
  `gene` int(11) NOT NULL,
  `gene_KO` varchar(45) DEFAULT NULL COMMENT '目标基因的KO分类编号',
  `locus_tag` varchar(45) DEFAULT NULL COMMENT '基因号',
  `gene_name` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`pathway`,`gene`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='代谢途径和所属于该代谢途径对象的基因之间的关系表';
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
代谢途径的参考文献

|field|type|attributes|description|
|-----|----|----------|-----------|
|pathway|Int64 (11)|``NN``||
|reference|Int64 (11)|``NN``||
|title|VarChar (45)||文献的标题|

```SQL
CREATE TABLE `xref_pathway_references` (
  `pathway` int(11) NOT NULL,
  `reference` int(11) NOT NULL,
  `title` varchar(45) DEFAULT NULL COMMENT '文献的标题',
  PRIMARY KEY (`pathway`,`reference`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='代谢途径的参考文献';
```



