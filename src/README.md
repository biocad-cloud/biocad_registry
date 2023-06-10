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

Generate time: 6/10/2023 11:43:02 AM<br />
By: ``mysqli.vb`` reflector tool ([https://github.com/xieguigang/mysqli.vb](https://github.com/xieguigang/mysqli.vb))

<div style="page-break-after: always;"></div>

***

## dblinks



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|Int64 (11)|``AI``, ``NN``, ``PK``||
|db_src|Int64 (11)|``NN``|the external database|
|xref_id|VarChar (255)|``NN``|the external database id of current entity|
|entity_id|Int64 (11)|``NN``|the biocad internal registry id of the entity object reference to|
|entity_type|Int64 (11)|``NN``|the data type of the current entity object<br /><br />1: molecules<br />2: reactions<br />3: gene ontology term<br />4: compartment term|
|add_time|DateTime ()|``NN``||


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `dblinks` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `db_src` INT UNSIGNED NOT NULL COMMENT 'the external database',
  `xref_id` VARCHAR(255) NOT NULL COMMENT 'the external database id of current entity',
  `entity_id` INT UNSIGNED NOT NULL COMMENT 'the biocad internal registry id of the entity object reference to',
  `entity_type` INT UNSIGNED NOT NULL COMMENT 'the data type of the current entity object\n\n1: molecules\n2: reactions\n3: gene ontology term\n4: compartment term',
  `add_time` DATETIME NOT NULL DEFAULT now(),
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

```


<div style="page-break-after: always;"></div>

***

## family



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|Int64 (11)|``AI``, ``NN``, ``PK``||
|name|VarChar (255)|``NN``||
|parent_id|Int64 (11)|``NN``||
|n_childs|Int64 (11)|``NN``||
|add_time|DateTime ()|``NN``||
|description|VarChar (8192)|||


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `family` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(255) NOT NULL,
  `parent_id` INT UNSIGNED NOT NULL,
  `n_childs` INT UNSIGNED NOT NULL DEFAULT 0,
  `add_time` DATETIME NOT NULL DEFAULT now(),
  `description` VARCHAR(8192) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

```


<div style="page-break-after: always;"></div>

***

## gene_ontology



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|Int64 (11)|``NN``, ``PK``|The go term id|
|name|VarChar (255)|``NN``||
|namespace|Boolean (1)|``NN``|enum value from https://github.com/SMRUCC/GCModeller/blob/5f709e21e88d4b10ca9ef3f2c2c83585405caa2b/src/GCModeller/data/GO_gene-ontology/GeneOntology/Ontologies.vb#L74<br /><br />1: cellular_component<br />2: biological_process<br />3: molecular_function|
|def|VarChar (8192)|``NN``||
|is_obsolete|Boolean (1)|``NN``||


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `gene_ontology` (
  `id` INT UNSIGNED NOT NULL COMMENT 'The go term id',
  `name` VARCHAR(255) NOT NULL,
  `namespace` TINYINT(1) NOT NULL DEFAULT 1 COMMENT 'enum value from https://github.com/SMRUCC/GCModeller/blob/5f709e21e88d4b10ca9ef3f2c2c83585405caa2b/src/GCModeller/data/GO_gene-ontology/GeneOntology/Ontologies.vb#L74\n\n1: cellular_component\n2: biological_process\n3: molecular_function',
  `def` VARCHAR(8192) NOT NULL,
  `is_obsolete` TINYINT NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

```


<div style="page-break-after: always;"></div>

***

## genomes



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|Int64 (11)|``AI``, ``NN``, ``PK``||
|taxonomic_group|Int64 (11)|``NN``||
|name|VarChar (255)|``NN``||
|ncbi_taxid|Int64 (11)|``NN``|default 0 means unknown genome ( has not been assigned in ncbi taxonomy database?)|
|add_time|DateTime ()|``NN``||


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `genomes` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `taxonomic_group` INT UNSIGNED NOT NULL,
  `name` VARCHAR(255) NOT NULL,
  `ncbi_taxid` INT UNSIGNED NOT NULL DEFAULT 0 COMMENT 'default 0 means unknown genome ( has not been assigned in ncbi taxonomy database?)',
  `add_time` DATETIME NOT NULL DEFAULT now(),
  PRIMARY KEY (`id`))
ENGINE = InnoDB
COMMENT = 'A subset of the ncbi taxonomy tree';

```


<div style="page-break-after: always;"></div>

***

## go_dag



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|Int64 (11)|``AI``, ``NN``, ``PK``||
|term_id|Int64 (11)|``NN``|current go term id|
|go_term|Int64 (11)|``NN``|another go term id|
|relationship|Int64 (11)|``NN``|1: is_a, example as term_id is_a go_term|


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `go_dag` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `term_id` INT UNSIGNED NOT NULL COMMENT 'current go term id',
  `go_term` INT UNSIGNED NOT NULL COMMENT 'another go term id',
  `relationship` INT UNSIGNED NOT NULL COMMENT '1: is_a, example as term_id is_a go_term',
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

```


<div style="page-break-after: always;"></div>

***

## model_graph



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|Int64 (11)|``AI``, ``NN``, ``PK``||
|pathway_id|Int64 (11)|``NN``||
|reaction_id|Int64 (11)|``NN``||
|add_time|DateTime ()|``NN``||


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `model_graph` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `pathway_id` INT UNSIGNED NOT NULL,
  `reaction_id` INT UNSIGNED NOT NULL,
  `add_time` DATETIME NOT NULL DEFAULT now(),
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

```


<div style="page-break-after: always;"></div>

***

## molecules



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|Int64 (11)|``AI``, ``NN``, ``PK``||
|molecule_id|VarChar (64)|``NN``||
|type|Int64 (11)|``NN``|1: gene(dna), rna, polypeptide(protein)<br />2: metabolite<br />3: protein-complex<br /><br />|
|name|VarChar (64)|``NN``||
|seq_num|Int64 (11)|``NN``||
|synonym_num|Int64 (11)|``NN``||
|ncbi_taxid|Int64 (11)|``NN``|the genome source of current molecule|
|category_id|Int64 (11)|``NN``||
|description|VarChar (8192)|||
|add_time|DateTime ()|``NN``||

<div style="page-break-after: always;"></div>


#### SQL Declare

```SQL
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

```


<div style="page-break-after: always;"></div>

***

## motif



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|Int64 (11)|``AI``, ``NN``, ``PK``||
|family|VarChar (45)|``NN``|the motif family name|
|family_id|Int64 (11)|``NN``||
|type|VarChar (3)|``NN``|RNA/TF|
|width|Int64 (11)|``NN``|the motif PWM width|
|variant|VarChar (4096)|``NN``|family may contains multiple version of the PWM variant matrix data, this data field is the sequence string representative of the PWM matrix object|
|checmsum|VarChar (32)||check sum of the pwm binary pack data|
|pwm|VarChar (8192)|``NN``|pwm data bson encoded in gzip compression base64 string|
|add_time|DateTime ()|``NN``||
|note|VarChar (4096)|||

<div style="page-break-after: always;"></div>


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `motif` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `family` VARCHAR(45) NOT NULL COMMENT 'the motif family name',
  `family_id` INT UNSIGNED NOT NULL,
  `type` VARCHAR(3) NOT NULL DEFAULT 'TF' COMMENT 'RNA/TF',
  `width` INT UNSIGNED NOT NULL COMMENT 'the motif PWM width',
  `variant` VARCHAR(4096) NOT NULL COMMENT 'family may contains multiple version of the PWM variant matrix data, this data field is the sequence string representative of the PWM matrix object',
  `checmsum` VARCHAR(32) NULL COMMENT 'check sum of the pwm binary pack data',
  `pwm` VARCHAR(8192) NOT NULL COMMENT 'pwm data bson encoded in gzip compression base64 string',
  `add_time` DATETIME NOT NULL DEFAULT now(),
  `note` VARCHAR(4096) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
COMMENT = 'organism non-specific data';

```


<div style="page-break-after: always;"></div>

***

## motif_sites



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|Int64 (11)|``AI``, ``NN``, ``PK``||
|type|VarChar (3)|``NN``|RNA/TF|
|gene_id|VarChar (45)|||
|gene_name|VarChar (45)|||
|loci|Int64 (11)|``NN``|the upstream location of the motif site, value could be negative|
|score|Double ()|``NN``||
|regulator|Int64 (11)|``NN``||
|genome_id|Int64 (11)|``NN``||
|motif_id|Int64 (11)|``NN``|link to the organism non-specific motif PWM model data|
|len|Int64 (11)|``NN``||
|hashcode|VarChar (32)|``NN``||
|family|VarChar (45)|||
|seq|VarChar (8192)|``NN``|the motif site sequence|

<div style="page-break-after: always;"></div>


#### SQL Declare

```SQL
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

```


<div style="page-break-after: always;"></div>

***

## ncbi_taxonomy_tree



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|Int64 (11)|``NN``, ``PK``|The ncbi taxonomy id|
|name|VarChar (1024)|``NN``||
|parent|Int64 (11)|``NN``||
|rank|Int64 (11)|``NN``|the vocabulary term id in the biocad registry|
|n_childs|Int64 (11)|``NN``||


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `ncbi_taxonomy_tree` (
  `id` INT UNSIGNED NOT NULL COMMENT 'The ncbi taxonomy id',
  `name` VARCHAR(1024) NOT NULL,
  `parent` INT UNSIGNED NOT NULL,
  `rank` INT UNSIGNED NOT NULL COMMENT 'the vocabulary term id in the biocad registry',
  `n_childs` INT UNSIGNED NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

```


<div style="page-break-after: always;"></div>

***

## operon_graph



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|Int64 (11)|``AI``, ``NN``, ``PK``||
|operon_id|Int64 (11)|``NN``||
|gene_id|Int64 (11)|``NN``||
|genome_id|Int64 (11)|``NN``||


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `operon_graph` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `operon_id` INT UNSIGNED NOT NULL,
  `gene_id` INT UNSIGNED NOT NULL,
  `genome_id` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

```


<div style="page-break-after: always;"></div>

***

## operon_group



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|Int64 (11)|``AI``, ``NN``, ``PK``||
|operon|VarChar (255)|``NN``|the operon name|
|genome_id|Int64 (11)|``NN``||
|add_time|DateTime ()|``NN``||


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `operon_group` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `operon` VARCHAR(255) NOT NULL COMMENT 'the operon name',
  `genome_id` INT UNSIGNED NOT NULL,
  `add_time` DATETIME NOT NULL DEFAULT now(),
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

```


<div style="page-break-after: always;"></div>

***

## pathway



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|Int64 (11)|``AI``, ``NN``, ``PK``||
|name|VarChar (1024)|``NN``||
|add_time|DateTime ()|``NN``||
|source|Int64 (11)|``NN``|the vocabulary id of the database source name|
|note|Text ()|||


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `pathway` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(1024) NOT NULL,
  `add_time` DATETIME NOT NULL DEFAULT now(),
  `source` INT UNSIGNED NOT NULL COMMENT 'the vocabulary id of the database source name',
  `note` MEDIUMTEXT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
COMMENT = 'define the biological pathway data';

```


<div style="page-break-after: always;"></div>

***

## reaction_graph



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|Int64 (11)|``AI``, ``NN``, ``PK``||
|reaction_id|Int64 (11)|``NN``||
|molecule_id|Int64 (11)|``NN``||
|name|VarChar (255)|``NN``|the name text of the molecule|
|stoichiometric|Double ()|``NN``||
|type|Boolean (1)|``NN``|1 - left, substrate; 2 - right, product; 3 - middle, enzyme|


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `reaction_graph` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `reaction_id` INT UNSIGNED NOT NULL,
  `molecule_id` INT UNSIGNED NOT NULL,
  `name` VARCHAR(255) NOT NULL COMMENT 'the name text of the molecule',
  `stoichiometric` FLOAT NOT NULL DEFAULT 1,
  `type` TINYINT(1) NOT NULL DEFAULT 1 COMMENT '1 - left, substrate; 2 - right, product; 3 - middle, enzyme',
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

```


<div style="page-break-after: always;"></div>

***

## reaction_node



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|Int64 (11)|``AI``, ``NN``, ``PK``||
|name|VarChar (255)|``NN``||
|n_left|Int64 (11)|``NN``||
|n_right|Int64 (11)|``NN``||
|n_regulator|Int64 (11)||count for enzyme or something of which associated with this reaction link|
|add_time|DateTime ()|``NN``||


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `reaction_node` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(255) NOT NULL,
  `n_left` INT NOT NULL DEFAULT 0,
  `n_right` INT NOT NULL DEFAULT 0,
  `n_regulator` INT NULL DEFAULT 0 COMMENT 'count for enzyme or something of which associated with this reaction link',
  `add_time` DATETIME NOT NULL DEFAULT now(),
  PRIMARY KEY (`id`))
ENGINE = InnoDB
COMMENT = 'define the biochemical reaction data';

```


<div style="page-break-after: always;"></div>

***

## regulator



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|Int64 (11)|``AI``, ``NN``, ``PK``||
|gene_id|VarChar (64)|``NN``||
|type|VarChar (3)|``NN``|TF/RNA|
|mol_id|Int64 (11)|``NN``||
|family_id|Int64 (11)|``NN``||
|family|VarChar (45)|``NN``||
|genome_id|Int64 (11)|``NN``||


#### SQL Declare

```SQL
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

```


<div style="page-break-after: always;"></div>

***

## seq_archive



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|Int64 (11)|``AI``, ``NN``, ``PK``||
|seq_id|VarChar (64)|``NN``||
|mol_id|Int64 (11)|``NN``||
|mol_type|Int64 (11)|``NN``|1: dna<br />2: rna<br />3: protein<br /><br />https://github.com/SMRUCC/GCModeller/blob/918cb50e86f2159856edaa0531cec227fc7f6e97/src/GCModeller/core/Bio.Assembly/SequenceModel/Abstract.vb#L70|
|len|Int64 (11)|``NN``||
|hashcode|VarChar (32)|``NN``|md5 hashcode of the sequence string|
|seq|Text ()|``NN``||


#### SQL Declare

```SQL
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

```


<div style="page-break-after: always;"></div>

***

## seq_graph



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|Int64 (11)|``NN``, ``PK``|the id of the seq_archive, so this id is not an auto-increment index|
|title|VarChar (4096)|``NN``||
|add_time|DateTime ()|``NN``||
|hashcode|VarChar (32)|``NN``|md5 of the graph_base64 string|
|graph_base64|Text ()|``NN``|base64 encoded string based on the zlib compresion of seq graph values|


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `seq_graph` (
  `id` INT UNSIGNED NOT NULL COMMENT 'the id of the seq_archive, so this id is not an auto-increment index',
  `title` VARCHAR(4096) NOT NULL,
  `add_time` DATETIME NOT NULL DEFAULT now(),
  `hashcode` VARCHAR(32) NOT NULL COMMENT 'md5 of the graph_base64 string',
  `graph_base64` LONGTEXT NOT NULL COMMENT 'base64 encoded string based on the zlib compresion of seq graph values',
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

```


<div style="page-break-after: always;"></div>

***

## subcellular_compartments



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|Int64 (11)|``AI``, ``NN``, ``PK``||
|name|VarChar (2048)|``NN``||
|add_time|DateTime ()|||


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `subcellular_compartments` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(2048) NOT NULL,
  `add_time` DATETIME NULL DEFAULT now(),
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

```


<div style="page-break-after: always;"></div>

***

## subcellular_locations



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|Int64 (11)|``AI``, ``NN``, ``PK``||
|biological_process|Int64 (11)|``NN``||
|compartment|Int64 (11)|``NN``||


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `subcellular_locations` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `biological_process` INT UNSIGNED NOT NULL,
  `compartment` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

```


<div style="page-break-after: always;"></div>

***

## synonym



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|Int64 (11)|``AI``, ``NN``, ``PK``||
|term_id|Int64 (11)|``NN``|A internal reference id to the biocad object|
|synonym|VarChar (255)|``NN``||
|type|Int64 (11)|``NN``|the data type of current term object, could be:<br /><br />1. molecules object<br />2. gene ontology term|
|add_time|DateTime ()|``NN``||


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `synonym` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `term_id` INT UNSIGNED NOT NULL COMMENT 'A internal reference id to the biocad object',
  `synonym` VARCHAR(255) NOT NULL,
  `type` INT UNSIGNED NOT NULL COMMENT 'the data type of current term object, could be:\n\n1. molecules object\n2. gene ontology term',
  `add_time` DATETIME NOT NULL DEFAULT now(),
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

```


<div style="page-break-after: always;"></div>

***

## taxonomic



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|Int64 (11)|``NN``, ``PK``|assign external tax id, so this id index field is not auto-incremental|
|name|VarChar (255)|``NN``||
|n_tax|Int64 (11)|``NN``||
|add_time|DateTime ()|``NN``||
|note|VarChar (4096)|||


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `taxonomic` (
  `id` INT UNSIGNED NOT NULL COMMENT 'assign external tax id, so this id index field is not auto-incremental',
  `name` VARCHAR(255) NOT NULL,
  `n_tax` INT UNSIGNED NOT NULL DEFAULT 0,
  `add_time` DATETIME NOT NULL DEFAULT now(),
  `note` VARCHAR(4096) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
COMMENT = 'Taxonomic group';

```


<div style="page-break-after: always;"></div>

***

## vocabulary



|field|type|attributes|description|
|-----|----|----------|-----------|
|id|Int64 (11)|``AI``, ``NN``, ``PK``||
|term|VarChar (255)|``NN``||
|hashcode|VarChar (32)|``NN``||
|parent_id|Int64 (11)|``NN``|the id of current vocabulary term that could be classify to, the id of another vocabulary term|
|add_time|DateTime ()|``NN``||
|description|VarChar (8192)|||


#### SQL Declare

```SQL
CREATE TABLE IF NOT EXISTS `vocabulary` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `term` VARCHAR(255) NOT NULL,
  `hashcode` VARCHAR(32) NOT NULL DEFAULT 'NA',
  `parent_id` INT UNSIGNED NOT NULL DEFAULT 0 COMMENT 'the id of current vocabulary term that could be classify to, the id of another vocabulary term',
  `add_time` DATETIME NOT NULL DEFAULT now(),
  `description` VARCHAR(8192) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

```


<div style="page-break-after: always;"></div>




