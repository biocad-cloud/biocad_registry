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

Generate time: 8/21/2024 9:49:34 PM<br />
By: ``mysqli.vb`` reflector tool ([https://github.com/xieguigang/mysqli.vb](https://github.com/xieguigang/mysqli.vb))

<div style="page-break-after: always;"></div>

***

## complex

the complex component composition graph data

|field|type|attributes|description|
|-----|----|----------|-----------|
|id|int (11)|``AI``, ``NN``, ``PK``, ``UN``||
|molecule_id|int (11)|``NN``, ``UN``|the complex molecule id|
|component_id|int (11)|``NN``, ``UN``|the component molecule id|
|add_time|datetime|``NN``||
|note|text|||


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `complex` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `molecule_id` INT UNSIGNED NOT NULL COMMENT 'the complex molecule id',
  `component_id` INT UNSIGNED NOT NULL COMMENT 'the component molecule id',
  `add_time` DATETIME NOT NULL DEFAULT now(),
  `note` LONGTEXT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
COMMENT = 'the complex component composition graph data';

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
|type|int (11)|``NN``, ``UN``|the target type of obj_id that point to, could be molecules, reactions, pathways|
|add_time|datetime|``NN``||


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `db_xrefs` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `obj_id` INT UNSIGNED NOT NULL,
  `db_key` INT UNSIGNED NOT NULL,
  `xref` VARCHAR(255) NOT NULL,
  `type` INT UNSIGNED NOT NULL COMMENT 'the target type of obj_id that point to, could be molecules, reactions, pathways',
  `add_time` DATETIME NOT NULL DEFAULT now(),
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

```


<div style="page-break-after: always;"></div>

***

## kinetic_law



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|int (11)|``AI``, ``NN``, ``PK``, ``UN``||
|db_xref|varchar (64)|``NN``||
|lambda|varchar (1024)|``NN``||
|params|varchar (1024)|``NN``||
|temperature|double|``NN``||
|pH|double|``NN``, ``UN``||
|uniprot|varchar (45)|``NN``||
|function_id|int (11)|``NN``, ``UN``||
|add_time|datetime|``NN``||
|note|text|||

<div style="page-break-after: always;"></div>


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `kinetic_law` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `db_xref` VARCHAR(64) NOT NULL,
  `lambda` VARCHAR(1024) NOT NULL,
  `params` VARCHAR(1024) NOT NULL,
  `temperature` DOUBLE NOT NULL DEFAULT 37,
  `pH` DOUBLE UNSIGNED NOT NULL DEFAULT 7.5,
  `uniprot` VARCHAR(45) NOT NULL,
  `function_id` INT UNSIGNED NOT NULL,
  `add_time` DATETIME NOT NULL DEFAULT now(),
  `note` LONGTEXT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

```


<div style="page-break-after: always;"></div>

***

## molecule

The molecular entity object inside a cell

|field|type|attributes|description|
|-----|----|----------|-----------|
|id|int (11)|``AI``, ``NN``, ``PK``, ``UN``||
|xref_id|varchar (32)|``NN``|the source external reference id of current molecule, for a gene should be a locus_tag, and protein should be a main uniprot accession id and for metabolite should be pubchem cid or chebi id|
|name|varchar (512)|``NN``|the name of the molecule|
|mass|double|``NN``|the molecular exact mass|
|type|int (11)|``NN``, ``UN``|the molecule type, the id of the vocabulary term, value could be nucl(DNA), RNA, prot, metabolite, complex|
|formula|varchar (128)|``NN``|metabolite formula or nucl/prot sequence|
|parent|int (11)|``NN``, ``UN``|the parent metabolite id, example as RNA is a parent of polypeptide, and gene is a parent of RNA sequence.|
|add_time|datetime|``NN``|add time of current molecular entity|
|note|text||description text about current entity object|


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `molecule` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `xref_id` VARCHAR(32) NOT NULL COMMENT 'the source external reference id of current molecule, for a gene should be a locus_tag, and protein should be a main uniprot accession id and for metabolite should be pubchem cid or chebi id',
  `name` VARCHAR(512) NOT NULL COMMENT 'the name of the molecule',
  `mass` DOUBLE NOT NULL COMMENT 'the molecular exact mass',
  `type` INT UNSIGNED NOT NULL COMMENT 'the molecule type, the id of the vocabulary term, value could be nucl(DNA), RNA, prot, metabolite, complex',
  `formula` VARCHAR(128) NOT NULL COMMENT 'metabolite formula or nucl/prot sequence',
  `parent` INT UNSIGNED NOT NULL COMMENT 'the parent metabolite id, example as RNA is a parent of polypeptide, and gene is a parent of RNA sequence.',
  `add_time` DATETIME NOT NULL DEFAULT now() COMMENT 'add time of current molecular entity',
  `note` LONGTEXT NULL COMMENT 'description text about current entity object',
  PRIMARY KEY (`id`))
ENGINE = InnoDB
COMMENT = 'The molecular entity object inside a cell';

```


<div style="page-break-after: always;"></div>

***

## molecule_function



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|int (11)|``AI``, ``NN``, ``PK``, ``UN``||
|molecule_id|int (11)|``NN``, ``UN``|the molecule id, usually be the protein molecule id|
|regulation_term|int (11)|``NN``, ``UN``|the id of the term in regulation graph table|
|add_time|datetime|``NN``||
|note|text|||


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `molecule_function` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `molecule_id` INT UNSIGNED NOT NULL COMMENT 'the molecule id, usually be the protein molecule id',
  `regulation_term` INT UNSIGNED NOT NULL COMMENT 'the id of the term in regulation graph table',
  `add_time` DATETIME NOT NULL DEFAULT now(),
  `note` LONGTEXT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

```


<div style="page-break-after: always;"></div>

***

## pathway



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|int (11)|``AI``, ``NN``, ``PK``, ``UN``||
|xref_id|varchar (45)||the external reference id of the pathway|
|name|varchar (1024)|``NN``|the pathway name|
|add_time|datetime|``NN``||
|note|text|||


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `pathway` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `xref_id` VARCHAR(45) NULL COMMENT 'the external reference id of the pathway',
  `name` VARCHAR(1024) NOT NULL COMMENT 'the pathway name',
  `add_time` DATETIME NOT NULL DEFAULT now(),
  `note` LONGTEXT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

```


<div style="page-break-after: always;"></div>

***

## pathway_graph



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|int (11)|``AI``, ``NN``, ``PK``, ``UN``||
|pathway_id|int (11)|``NN``, ``UN``||
|entity_id|int (11)|``NN``, ``UN``||
|add_time|datetime|``NN``||
|note|text|||


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `pathway_graph` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `pathway_id` INT UNSIGNED NOT NULL,
  `entity_id` INT UNSIGNED NOT NULL,
  `add_time` DATETIME NOT NULL DEFAULT now(),
  `note` LONGTEXT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

```


<div style="page-break-after: always;"></div>

***

## reaction

the definition of the biological reaction process

|field|type|attributes|description|
|-----|----|----------|-----------|
|id|int (11)|``AI``, ``NN``, ``PK``, ``UN``||
|db_xref|varchar (64)|``NN``||
|name|varchar (1024)|``NN``||
|equation|text|``NN``||
|add_time|datetime|``NN``||
|note|text|||


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `reaction` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `db_xref` VARCHAR(64) NOT NULL,
  `name` VARCHAR(1024) NOT NULL,
  `equation` MEDIUMTEXT NOT NULL,
  `add_time` DATETIME NOT NULL DEFAULT now(),
  `note` LONGTEXT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
COMMENT = 'the definition of the biological reaction process';

```


<div style="page-break-after: always;"></div>

***

## reaction_graph

the relationship between the reaction model and molecule objects

|field|type|attributes|description|
|-----|----|----------|-----------|
|id|int (11)|``AI``, ``NN``, ``PK``, ``UN``||
|reaction|int (11)|``NN``, ``UN``|the id of the reaction|
|molecule_id|int (11)|``NN``, ``UN``|the id of the molecule entity|
|db_xref|varchar (255)|``NN``|the reaction source definition of the molecule to reference the molecule entity inside biocad registry, used for link the molecule entity with current relationship record |
|role|int (11)|``NN``, ``UN``|the vocabulary term of the molecule role inside this reaction model, usually be the substrate or product|
|factor|double|``NN``||
|add_time|datetime|``NN``||
|note|text|||


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `reaction_graph` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `reaction` INT UNSIGNED NOT NULL COMMENT 'the id of the reaction',
  `molecule_id` INT UNSIGNED NOT NULL COMMENT 'the id of the molecule entity',
  `db_xref` VARCHAR(255) NOT NULL COMMENT 'the reaction source definition of the molecule to reference the molecule entity inside biocad registry, used for link the molecule entity with current relationship record ',
  `role` INT UNSIGNED NOT NULL COMMENT 'the vocabulary term of the molecule role inside this reaction model, usually be the substrate or product',
  `factor` DOUBLE NOT NULL DEFAULT 1,
  `add_time` DATETIME NOT NULL DEFAULT now(),
  `note` LONGTEXT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
COMMENT = 'the relationship between the reaction model and molecule objects';

```


<div style="page-break-after: always;"></div>

***

## regulation_graph



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|int (11)|``AI``, ``NN``, ``PK``, ``UN``||
|term|varchar (64)|``NN``|usually be the EC number|
|role|int (11)|``NN``, ``UN``||
|reaction_id|int (11)|``NN``, ``UN``||
|add_time|datetime|``NN``||
|note|text|||


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `regulation_graph` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `term` VARCHAR(64) NOT NULL COMMENT 'usually be the EC number',
  `role` INT UNSIGNED NOT NULL,
  `reaction_id` INT UNSIGNED NOT NULL,
  `add_time` DATETIME NOT NULL DEFAULT now(),
  `note` LONGTEXT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

```


<div style="page-break-after: always;"></div>

***

## sequence_graph

the sequence composition data

|field|type|attributes|description|
|-----|----|----------|-----------|
|id|int (11)|``AI``, ``NN``, ``PK``, ``UN``||
|molecule_id|int (11)|``NN``, ``UN``|the molecule reference id inside biocad registry system|
|sequence|text|``NN``|the sequence data, for metabolite should be the SMILES structure string data|
|hashcode|varchar (32)|``NN``|md5 hashcode of the sequence data string|
|seq_graph|text|``NN``|base64 encoded double vector of the embedding result|
|embedding|text|``NN``|the embedding keys, for dna, always ATGC, for RNA always AUGC, for popypeptide always 20 Amino acid, for small metabolite, is the atom groups that parsed from the smiles graph |
|add_time|datetime|``NN``||


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `sequence_graph` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `molecule_id` INT UNSIGNED NOT NULL COMMENT 'the molecule reference id inside biocad registry system',
  `sequence` LONGTEXT NOT NULL COMMENT 'the sequence data, for metabolite should be the SMILES structure string data',
  `hashcode` CHAR(32) NOT NULL COMMENT 'md5 hashcode of the sequence data string',
  `seq_graph` LONGTEXT NOT NULL COMMENT 'base64 encoded double vector of the embedding result',
  `embedding` LONGTEXT NOT NULL COMMENT 'the embedding keys, for dna, always ATGC, for RNA always AUGC, for popypeptide always 20 Amino acid, for small metabolite, is the atom groups that parsed from the smiles graph ',
  `add_time` DATETIME NOT NULL DEFAULT now(),
  PRIMARY KEY (`id`))
ENGINE = InnoDB
COMMENT = 'the sequence composition data';

```


<div style="page-break-after: always;"></div>

***

## subcellular_compartments

defines the subcellular compartments

|field|type|attributes|description|
|-----|----|----------|-----------|
|id|int (11)|``AI``, ``NN``, ``PK``, ``UN``||
|compartment_name|varchar (1024)|``NN``||
|topology|varchar (1024)|||
|add_time|datetime|``NN``||
|note|text|||


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `subcellular_compartments` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `compartment_name` VARCHAR(1024) NOT NULL,
  `topology` VARCHAR(1024) NULL,
  `add_time` DATETIME NOT NULL DEFAULT now(),
  `note` LONGTEXT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
COMMENT = 'defines the subcellular compartments';

```


<div style="page-break-after: always;"></div>

***

## subcellular_location

associates the subcellular_compartments and the molecule objects

|field|type|attributes|description|
|-----|----|----------|-----------|
|id|int (11)|``AI``, ``NN``, ``PK``, ``UN``||
|compartment_id|int (11)|``NN``, ``UN``||
|obj_id|int (11)|``NN``, ``UN``||
|entity|int (11)|``NN``, ``UN``|the vocabulary type id of the entity object, could be molecule, reaction or pathways|
|add_time|datetime|``NN``||
|note|text|||


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `subcellular_location` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `compartment_id` INT UNSIGNED NOT NULL,
  `obj_id` INT UNSIGNED NOT NULL,
  `entity` INT UNSIGNED NOT NULL COMMENT 'the vocabulary type id of the entity object, could be molecule, reaction or pathways',
  `add_time` DATETIME NOT NULL DEFAULT now(),
  `note` LONGTEXT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
COMMENT = 'associates the subcellular_compartments and the molecule objects';

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
CREATE TABLE IF NOT EXISTS `vocabulary` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `category` VARCHAR(32) NOT NULL,
  `term` VARCHAR(255) NOT NULL,
  `color` VARCHAR(8) NOT NULL DEFAULT '#000000' COMMENT 'the html color code, example as #ffffff',
  `add_time` DATETIME NOT NULL DEFAULT now(),
  `note` LONGTEXT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

```


<div style="page-break-after: always;"></div>




