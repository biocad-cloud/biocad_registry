require(biocad_registry);

imports "dbget" from "kegg_kit";

let org = dbget::show_organism("xcc");

str(org);