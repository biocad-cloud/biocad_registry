require(biocad_registry);

imports "dbget" from "kegg_kit";
imports "kegg_api" from "kegg_kit";

# tools for download kegg bacterial genomes
options(dbget.cache = @dir);

let genomes = kegg_api::listing("organism");

genomes <- lapply(genomes, str -> strsplit(str, "\t"));
genomes <- data.frame(
    row.names = names(genomes),
    kegg_code = genomes@{1},
    organism = genomes@{2},
    kingdom = sapply(genomes@{3},str -> strsplit(str,";")[1]),
    lineage = genomes@{3}
);

print(genomes);

let org = dbget::show_organism("hsa");

str(as.list(org));