require(biocad_registry);

imports "taxonomy_kit" from "metagenomics_kit";
imports "data_imports" from "biocad_registry";

let biocad_registry = open_registry("root", 123456, host ="127.0.0.1");
let taxonomy_tree = taxonomy_kit::Ncbi.taxonomy_tree("D:\\datapool\\taxdump");

data_imports::imports_taxonomy(biocad_registry, taxonomy_tree);