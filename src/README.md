# MySql Development Docs #

MySql database field attributes notes in this development document:

> + **AI**: Auto Increment;
> + **B**:  Binary;
> + **G**:  Generated
> + **NN**: Not Null;
> + **PK**: Primary Key;
> + **UQ**: Unique;
> + **UN**: Unsigned;
> + **ZF**: Zero Fill

Generate time: 8/2/2024 10:43:53 PM<br />
By: ``mysqli.vb`` reflector tool ([https://github.com/xieguigang/mysqli.vb](https://github.com/xieguigang/mysqli.vb))

<div style="page-break-after: always;"></div>

***

## complex



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|int (11)|``AI``, ``NN``, ``PK``, ``UN``||
|molecule_id|int (11)|``NN``, ``UN``|the complex molecule id|
|component_id|int (11)|``NN``, ``UN``|the component molecule id|
|add_time|datetime|``NN``||
|note|text|||


#### SQL Declare

```SQL
CREATE TABLE `complex` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `molecule_id` int unsigned NOT NULL COMMENT 'the complex molecule id',
  `component_id` int unsigned NOT NULL COMMENT 'the component molecule id',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext COLLATE utf8mb3_bin,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;
```


<div style="page-break-after: always;"></div>

***

## db_xrefs



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|int (11)|``AI``, ``NN``, ``PK``, ``UN``||
|obj_id|int (11)|``NN``, ``UN``||
|db_key|int (11)|``NN``, ``UN``||
|xref|varchar (255)|``NN``||
|type|int (11)|``NN``, ``UN``||
|add_time|datetime|``NN``||


#### SQL Declare

```SQL
CREATE TABLE `db_xrefs` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `obj_id` int unsigned NOT NULL,
  `db_key` int unsigned NOT NULL,
  `xref` varchar(255) COLLATE utf8mb3_bin NOT NULL,
  `type` int unsigned NOT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;
```


<div style="page-break-after: always;"></div>

***

## molecule



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|int (11)|``AI``, ``NN``, ``PK``, ``UN``||
|name|varchar (1024)|``NN``|the name of the molecule|
|mass|double|``NN``|the molecular exact mass|
|type|int (11)|``NN``, ``UN``|the molecule type, the id of the vocabulary term, value could be nucl(DNA), RNA, prot, metabolite, complex|
|formula|varchar (255)|``NN``|metabolite formula or nucl/prot sequence|
|parent|int (11)|``NN``, ``UN``|the parent metabolite id, example as RNA is a parent of polypeptide, and gene is a parent of RNA sequence.|
|add_time|datetime|``NN``|add time of current molecular entity|
|note|text||description text about current entity object|


#### SQL Declare

```SQL
CREATE TABLE `molecule` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(1024) COLLATE utf8mb3_bin NOT NULL COMMENT 'the name of the molecule',
  `mass` double NOT NULL COMMENT 'the molecular exact mass',
  `type` int unsigned NOT NULL COMMENT 'the molecule type, the id of the vocabulary term, value could be nucl(DNA), RNA, prot, metabolite, complex',
  `formula` varchar(255) COLLATE utf8mb3_bin NOT NULL COMMENT 'metabolite formula or nucl/prot sequence',
  `parent` int unsigned NOT NULL COMMENT 'the parent metabolite id, example as RNA is a parent of polypeptide, and gene is a parent of RNA sequence.',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'add time of current molecular entity',
  `note` longtext COLLATE utf8mb3_bin COMMENT 'description text about current entity object',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='The molecular entity object';
```


<div style="page-break-after: always;"></div>

***

## sequence_graph



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|int (11)|``AI``, ``NN``, ``PK``, ``UN``||
|molecule_id|int (11)|``NN``, ``UN``||
|sequence|text|``NN``|the sequence data|
|seq_graph|text|``NN``|base64 encoded double vector of the embedding result|
|embedding|text|``NN``|the embedding keys, for dna, always ATGC, for RNA always AUGC, for popypeptide always 20 Amino acid, for small metabolite, is the atom groups that parsed from the smiles graph |
|add_time|datetime|``NN``||


#### SQL Declare

```SQL
CREATE TABLE `sequence_graph` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `molecule_id` int unsigned NOT NULL,
  `sequence` longtext COLLATE utf8mb3_bin NOT NULL COMMENT 'the sequence data',
  `seq_graph` longtext COLLATE utf8mb3_bin NOT NULL COMMENT 'base64 encoded double vector of the embedding result',
  `embedding` longtext COLLATE utf8mb3_bin NOT NULL COMMENT 'the embedding keys, for dna, always ATGC, for RNA always AUGC, for popypeptide always 20 Amino acid, for small metabolite, is the atom groups that parsed from the smiles graph ',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;
```


<div style="page-break-after: always;"></div>

***

## subcellular_compartments



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|int (11)|``AI``, ``NN``, ``PK``, ``UN``||
|compartment_name|varchar (1024)|``NN``||
|add_time|datetime|``NN``||
|note|text|||


#### SQL Declare

```SQL
CREATE TABLE `subcellular_compartments` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `compartment_name` varchar(1024) COLLATE utf8mb3_bin NOT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext COLLATE utf8mb3_bin,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `compartment_name_UNIQUE` (`compartment_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='defines the subcellular compartments';
```


<div style="page-break-after: always;"></div>

***

## subcellular_location



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|int (11)|``AI``, ``NN``, ``PK``, ``UN``||
|compartment_id|int (11)|``NN``, ``UN``||
|obj_id|int (11)|``NN``, ``UN``||
|add_time|datetime|``NN``||
|note|text|||


#### SQL Declare

```SQL
CREATE TABLE `subcellular_location` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `compartment_id` int unsigned NOT NULL,
  `obj_id` int unsigned NOT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext COLLATE utf8mb3_bin,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='associates the subcellular_compartments and the molecule objects';
```


<div style="page-break-after: always;"></div>

***

## vocabulary



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|int (11)|``AI``, ``NN``, ``PK``, ``UN``||
|category|varchar (32)|``NN``||
|term|varchar (255)|``NN``||
|color|varchar (8)|``NN``|the html color code, example as #ffffff|
|add_time|datetime|``NN``||
|note|text|||


#### SQL Declare

```SQL
CREATE TABLE `vocabulary` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `category` varchar(32) COLLATE utf8mb3_bin NOT NULL,
  `term` varchar(255) COLLATE utf8mb3_bin NOT NULL,
  `color` varchar(8) COLLATE utf8mb3_bin NOT NULL DEFAULT '#000000' COMMENT 'the html color code, example as #ffffff',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext COLLATE utf8mb3_bin,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;
```


<div style="page-break-after: always;"></div>




