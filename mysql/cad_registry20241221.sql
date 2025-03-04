CREATE DATABASE  IF NOT EXISTS `cad_registry` /*!40100 DEFAULT CHARACTER SET utf8mb3 COLLATE utf8mb3_bin */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `cad_registry`;
-- MySQL dump 10.13  Distrib 8.0.36, for Win64 (x86_64)
--
-- Host: 192.168.3.233    Database: cad_registry
-- ------------------------------------------------------
-- Server version	9.0.1

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `complex`
--

DROP TABLE IF EXISTS `complex`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `complex` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `molecule_id` int unsigned NOT NULL COMMENT 'the complex molecule id',
  `component_id` int unsigned NOT NULL COMMENT 'the component molecule id',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext COLLATE utf8mb3_bin,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `molecule_info_idx` (`molecule_id`),
  KEY `component_info_idx` (`component_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='the complex component composition graph data';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `db_xrefs`
--

DROP TABLE IF EXISTS `db_xrefs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `db_xrefs` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `obj_id` int unsigned NOT NULL,
  `db_key` int unsigned NOT NULL,
  `xref` varchar(255) COLLATE utf8mb3_bin NOT NULL,
  `type` int unsigned NOT NULL COMMENT 'the target type of obj_id that point to, could be molecules, reactions, pathways',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `unique_dblink` (`obj_id`,`db_key`,`xref`,`type`),
  KEY `molecule_id_idx` (`obj_id`),
  KEY `db_name_idx` (`db_key`),
  KEY `object_type_idx` (`type`),
  KEY `find_xrefs` (`type`,`xref`),
  KEY `dbid_index` (`xref`)
) ENGINE=InnoDB AUTO_INCREMENT=31057711 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `genomics`
--

DROP TABLE IF EXISTS `genomics`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `genomics` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `ncbi_taxid` int unsigned NOT NULL COMMENT 'the ncbi taxonomy id',
  `db_xref` varchar(45) COLLATE utf8mb3_bin DEFAULT NULL COMMENT 'ncbi genbank accession id of current genomics sequence',
  `def` mediumtext COLLATE utf8mb3_bin NOT NULL,
  `nt` longtext CHARACTER SET utf8mb3 COLLATE utf8mb3_bin NOT NULL COMMENT 'sequence data',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `comment` longtext COLLATE utf8mb3_bin,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `taxonomy_index` (`ncbi_taxid`),
  KEY `accession_index` (`db_xref`) /*!80000 INVISIBLE */,
  FULLTEXT KEY `search_text` (`def`,`comment`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='genome nucleotide sequence data';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `kinetic_law`
--

DROP TABLE IF EXISTS `kinetic_law`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `kinetic_law` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `db_xref` varchar(64) COLLATE utf8mb3_bin NOT NULL COMMENT 'the external reference id of current kinetics lambda model',
  `params` json NOT NULL COMMENT 'parameter set of the current kinetics lambda epxression, in json string format',
  `lambda` mediumtext CHARACTER SET utf8mb3 COLLATE utf8mb3_bin NOT NULL COMMENT 'the lambda expression of the kinetics',
  `temperature` double NOT NULL DEFAULT '37' COMMENT 'temperature of the enzyme catalytic kinetics',
  `pH` double unsigned NOT NULL DEFAULT '7.5' COMMENT 'pH of the enzyme catalytic kinetics',
  `buffer` mediumtext COLLATE utf8mb3_bin,
  `substrate_id` int unsigned NOT NULL COMMENT 'id reference to the metabolite molecule',
  `uniprot` varchar(45) CHARACTER SET utf8mb3 COLLATE utf8mb3_bin DEFAULT NULL COMMENT 'the uniprot id of the current enzyme model, the kinetics parameter is associated with a specific molecule instance. could be missing',
  `ec_number` varchar(128) COLLATE utf8mb3_bin NOT NULL DEFAULT '-' COMMENT 'the enzyme ec_number reference id of the molecule function record',
  `json_str` json NOT NULL COMMENT 'the raw json string data of current kinetics data',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext COLLATE utf8mb3_bin COMMENT 'description note text about current enzyme kinetics lambda model',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `regulation_id_idx` (`ec_number`),
  KEY `xrefs_index` (`db_xref`),
  KEY `ph_filter` (`pH`),
  KEY `temperature_filter` (`temperature`),
  KEY `uniprot_index` (`uniprot`)
) ENGINE=InnoDB AUTO_INCREMENT=35204 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='the enzymatic catalytic kinetics lambda model';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `molecule`
--

DROP TABLE IF EXISTS `molecule`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `molecule` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `xref_id` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'the source external reference id of current molecule, for a gene should be a locus_tag, and protein should be a main uniprot accession id and for metabolite should be pubchem cid or chebi id(255 chars for long metacyc id)',
  `name` varchar(512) COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'the name of the molecule',
  `mass` double NOT NULL COMMENT 'the molecular exact mass',
  `type` int unsigned NOT NULL COMMENT 'the molecule type, the id of the vocabulary term, value could be nucl(DNA), RNA, prot, metabolite, complex',
  `formula` varchar(128) COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'metabolite formula or nucl/prot sequence',
  `parent` int unsigned NOT NULL COMMENT 'the parent metabolite id, example as RNA is a parent of polypeptide, and gene is a parent of RNA sequence.',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'add time of current molecular entity',
  `note` longtext COLLATE utf8mb4_unicode_ci COMMENT 'description text about current entity object',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`) /*!80000 INVISIBLE */,
  KEY `data_type_idx` (`type`),
  KEY `parent_molecule_idx` (`parent`),
  KEY `name_index` (`name`),
  KEY `xref_index` (`xref_id`),
  KEY `mass_filter` (`mass`),
  KEY `find_index` (`xref_id`,`type`)
) ENGINE=InnoDB AUTO_INCREMENT=1052704 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='The molecular entity object inside a cell';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `molecule_function`
--

DROP TABLE IF EXISTS `molecule_function`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `molecule_function` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `molecule_id` int unsigned NOT NULL COMMENT 'the molecule id, usually be the protein molecule id',
  `regulation_term` int unsigned NOT NULL COMMENT 'the id of the term in regulation graph table',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext COLLATE utf8mb3_bin,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `molecule_entity_idx` (`molecule_id`),
  KEY `regulation_info_idx` (`regulation_term`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `molecule_tags`
--

DROP TABLE IF EXISTS `molecule_tags`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `molecule_tags` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `tag_id` int unsigned NOT NULL COMMENT 'id reference to the vocabulary table',
  `molecule_id` int unsigned NOT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `description` longtext COLLATE utf8mb3_bin,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `check_label` (`tag_id`,`molecule_id`),
  KEY `filter_tag` (`tag_id`),
  KEY `filter_obj` (`molecule_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='keywords, tags, labels of the molecule entity';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ncbi_taxonomy`
--

DROP TABLE IF EXISTS `ncbi_taxonomy`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ncbi_taxonomy` (
  `id` int unsigned NOT NULL COMMENT 'ncbi tax id',
  `taxname` varchar(255) COLLATE utf8mb3_bin NOT NULL,
  `childs` json DEFAULT NULL COMMENT 'tax id of the direct child nodes, json array',
  `parent_id` int unsigned DEFAULT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `description` longtext COLLATE utf8mb3_bin,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `match_name` (`taxname`),
  KEY `find_parent` (`parent_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='the ncbi taxonomy information';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `odor`
--

DROP TABLE IF EXISTS `odor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `odor` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `molecule_id` int unsigned NOT NULL,
  `category` int unsigned NOT NULL,
  `odor` varchar(255) COLLATE utf8mb3_bin DEFAULT NULL,
  `hashcode` varchar(32) COLLATE utf8mb3_bin NOT NULL,
  `value` double NOT NULL DEFAULT '0',
  `unit` int unsigned NOT NULL,
  `text` tinytext COLLATE utf8mb3_bin NOT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `odor_term_index` (`odor`),
  KEY `class_index` (`category`),
  KEY `mol_index` (`molecule_id`),
  KEY `check_odor` (`molecule_id`,`category`,`odor`),
  FULLTEXT KEY `search_text` (`text`)
) ENGINE=InnoDB AUTO_INCREMENT=69396 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='odor information about the metabolite molecules';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pathway`
--

DROP TABLE IF EXISTS `pathway`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pathway` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `xref_id` varchar(45) COLLATE utf8mb3_bin DEFAULT NULL COMMENT 'the external reference id of the pathway',
  `name` varchar(1024) COLLATE utf8mb3_bin NOT NULL COMMENT 'the pathway name',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext COLLATE utf8mb3_bin,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pathway_graph`
--

DROP TABLE IF EXISTS `pathway_graph`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pathway_graph` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `pathway_id` int unsigned NOT NULL,
  `entity_id` int unsigned NOT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext COLLATE utf8mb3_bin,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `pathway_info_idx` (`pathway_id`),
  KEY `molecule_data_idx` (`entity_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `reaction`
--

DROP TABLE IF EXISTS `reaction`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reaction` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `db_xref` varchar(64) COLLATE utf8mb3_bin NOT NULL COMMENT 'the reference id of this reaction model from the external source database',
  `name` varchar(1024) COLLATE utf8mb3_bin NOT NULL COMMENT 'name of current reaction model',
  `equation` mediumtext COLLATE utf8mb3_bin NOT NULL COMMENT 'reaction equation of current reaction model',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext COLLATE utf8mb3_bin,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `db_xref_UNIQUE` (`db_xref`)
) ENGINE=InnoDB AUTO_INCREMENT=65720 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='the definition of the biological reaction process';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `reaction_graph`
--

DROP TABLE IF EXISTS `reaction_graph`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reaction_graph` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `reaction` int unsigned NOT NULL COMMENT 'the id of the reaction',
  `molecule_id` int unsigned NOT NULL COMMENT 'the id of the molecule entity',
  `db_xref` varchar(255) COLLATE utf8mb3_bin NOT NULL COMMENT 'the reaction source definition of the molecule to reference the molecule entity inside biocad registry, used for link the molecule entity with current relationship record ',
  `role` int unsigned NOT NULL COMMENT 'the vocabulary term of the molecule role inside this reaction model, usually be the substrate or product',
  `factor` double NOT NULL DEFAULT '1',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext COLLATE utf8mb3_bin,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`) /*!80000 INVISIBLE */,
  KEY `reaction_info_idx` (`reaction`),
  KEY `molecule_data_idx` (`molecule_id`),
  KEY `role_term_idx` (`role`),
  KEY `check_duplicated` (`reaction`,`db_xref`,`role`)
) ENGINE=InnoDB AUTO_INCREMENT=227925 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='the relationship between the reaction model and molecule objects';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `regulation_graph`
--

DROP TABLE IF EXISTS `regulation_graph`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `regulation_graph` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `term` varchar(64) COLLATE utf8mb3_bin NOT NULL COMMENT 'usually be the EC number',
  `role` int unsigned NOT NULL,
  `reaction_id` int unsigned NOT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext COLLATE utf8mb3_bin,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `role_term_idx` (`role`),
  KEY `reaction_process_idx` (`reaction_id`)
) ENGINE=InnoDB AUTO_INCREMENT=36533 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='the enzyme associates with the reactions';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `sequence_graph`
--

DROP TABLE IF EXISTS `sequence_graph`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sequence_graph` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `molecule_id` int unsigned NOT NULL COMMENT 'the molecule reference id inside biocad registry system',
  `sequence` longtext COLLATE utf8mb3_bin NOT NULL COMMENT 'the sequence data, for metabolite should be the SMILES structure string data',
  `hashcode` char(32) COLLATE utf8mb3_bin NOT NULL COMMENT 'md5 hashcode of the sequence data string',
  `seq_graph` longtext COLLATE utf8mb3_bin NOT NULL COMMENT 'base64 encoded double vector of the embedding result',
  `embedding` longtext COLLATE utf8mb3_bin NOT NULL COMMENT 'the embedding keys, for dna, always ATGC, for RNA always AUGC, for popypeptide always 20 Amino acid, for small metabolite, is the atom groups that parsed from the smiles graph ',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `molecules_idx` (`molecule_id`),
  KEY `search_sequence` (`hashcode`)
) ENGINE=InnoDB AUTO_INCREMENT=1034407 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='the sequence composition data';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `subcellular_compartments`
--

DROP TABLE IF EXISTS `subcellular_compartments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `subcellular_compartments` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `compartment_name` varchar(1024) COLLATE utf8mb3_bin NOT NULL,
  `topology` varchar(1024) COLLATE utf8mb3_bin DEFAULT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext COLLATE utf8mb3_bin,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `compartment_name_UNIQUE` (`compartment_name`)
) ENGINE=InnoDB AUTO_INCREMENT=99 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='defines the subcellular compartments';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `subcellular_location`
--

DROP TABLE IF EXISTS `subcellular_location`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `subcellular_location` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `compartment_id` int unsigned NOT NULL,
  `obj_id` int unsigned NOT NULL,
  `entity` int unsigned NOT NULL COMMENT 'the vocabulary type id of the entity object, could be molecule, reaction or pathways',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext COLLATE utf8mb3_bin,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `unique_reference` (`compartment_id`,`obj_id`,`entity`) /*!80000 INVISIBLE */,
  KEY `subcellular_compartments_idx` (`compartment_id`),
  KEY `molecule_obj_idx` (`obj_id`),
  KEY `link_entity_type_idx` (`entity`)
) ENGINE=InnoDB AUTO_INCREMENT=507106 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin COMMENT='associates the subcellular_compartments and the molecule objects';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `vocabulary`
--

DROP TABLE IF EXISTS `vocabulary`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `vocabulary` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `category` varchar(32) COLLATE utf8mb3_bin NOT NULL,
  `term` varchar(255) COLLATE utf8mb3_bin NOT NULL,
  `color` varchar(8) COLLATE utf8mb3_bin NOT NULL DEFAULT '#000000' COMMENT 'the html color code, example as #ffffff',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `note` longtext COLLATE utf8mb3_bin,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `search_term` (`category`,`term`)
) ENGINE=InnoDB AUTO_INCREMENT=163 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping events for database 'cad_registry'
--

--
-- Dumping routines for database 'cad_registry'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-01-05  1:20:05
