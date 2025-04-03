require(biocad_registry);

imports "dbget" from "kegg_kit";
imports "kegg_api" from "kegg_kit";

# tools for download kegg bacterial genomes
options(dbget.cache = "D:\datapool");

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
print(unique(genomes$kingdom));

let bacterial = genomes[genomes$kingdom == "Prokaryotes", ];

for(let entry in as.list(bacterial,byrow=TRUE)) {
    try({
        let org = dbget::show_organism(entry$kegg_code);
        # str(as.list(org));
    });
}

