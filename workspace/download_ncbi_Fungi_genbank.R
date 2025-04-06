require(biocad_registry);

imports "dbget" from "kegg_kit";
imports "kegg_api" from "kegg_kit";

# tools for download kegg bacterial genomes genbank annotation data
options(dbget.cache = "D:\datapool");

let genomes = kegg_api::listing("organism");

genomes <- lapply(genomes, str -> strsplit(str, "\t"));
genomes <- data.frame(
    row.names = names(genomes),
    kegg_code = genomes@{1},
    organism = genomes@{2},
    kingdom = sapply(genomes@{3},str -> strsplit(str,";")[1]),
    class = sapply(genomes@{3},str -> strsplit(str,";")[2]),
    lineage = genomes@{3}
);

print(genomes);
print(unique(genomes$kingdom));

let Fungi = genomes[genomes$kingdom == "Eukaryotes", ];
Fungi = Fungi[Fungi$class == "Fungi", ];
let repo_dir= file.path(getOption("dbget.cache"),"ncbi_genbank");

for(let entry in as.list(Fungi,byrow=TRUE)) {
    try({
        let org = dbget::show_organism(entry$kegg_code);
        let kegg_code = as.list(org)$code;
        let source = as.list(org)$DataSource;

        source = source@text |> which(url -> (instr(url,"ncbi.nlm.nih.gov") > 1) && (instr(url, "assembly") > 1));
        source = last(strsplit(source, "/"));
        source = first(strsplit(source,".",fixed=TRUE));

        str(source);   

        if (isTRUE(nchar(source) > 0)) {
            get_genbank(asm_id = source, repo_dir = file.path(repo_dir, `${kegg_code}-${source}/`));
        }
    });
    
    # stop();
    invisible(NULL);
}

