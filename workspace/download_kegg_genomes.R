require(biocad_registry);

imports "dbget" from "kegg_kit";

options(dbget.cache = @dir);

let org = dbget::show_organism("hsa");

str(as.list(org));