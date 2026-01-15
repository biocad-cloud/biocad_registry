require(biocad_registry);

imports "setup" from "biocad_registry";
imports "taxonomy_kit" from "metagenomics_kit";

let registry = open_registry("xieguigang", 123456, host ="192.168.3.15");
let ncbi_tax = Ncbi.taxonomy_tree("U:\metagenomics_LLMs\taxdmp_2025-12-01");

setup::setup_taxonomy(registry,ncbi_tax);