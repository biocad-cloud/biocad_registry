require(GCModeller);

setwd(@dir);
options(http.cache_dir = "E:/UniProt/genbank");

let genome_list = read.csv("F:/bioCAD/mysql/genomes.csv", row.names = 1)$name;
let download_genbank = function(genome) {
    for(tax in taxonomy_search(genome)) {
        let assm_list = reference_genome(ncbi_taxid = tax) |> first();
        let id = assm_list$assembly$assembly_accession;

        str(tax);
        str(assm_list);
        str(id);

        if (!is.null(id)) {
            fetch_genbank(accession_id = id);
        }

        sleep(3);
    }

    invisible(NULL);
}

print(genome_list);

for(name in genome_list) {
    try({
        download_genbank(name);
    });

    invisible(NULL);
}
