require(biocad_registry);

imports "taxonomy_kit" from "metagenomics_kit";
imports "data_imports" from "biocad_registry";

let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");
let taxonomy_tree = taxonomy_kit::Ncbi.taxonomy_tree("D:\\datapool\\taxdump");

data_imports::imports_taxonomy(biocad_registry, taxonomy_tree);