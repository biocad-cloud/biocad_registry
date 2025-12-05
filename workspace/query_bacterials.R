require(GCModeller);

imports "NCBI" from "annotationKit";

let genomes = read.csv("D:\datapool\bacterial.csv", row.names = 1, check.names = FALSE);

print(genomes);

let db = genbank_assemblyDb("D:\datapool\assembly_summary_genbank.txt");

let q = [];
let index = [];

for(let name in tqdm(genomes$organism)) {
    let result = db |> NCBI::query(name, cutoff = 0.9);

    index = append(index, result |> attr("index"));
    q = append(q, result);    
}

q = unlist(q);
index = unlist(index);

print(as.data.frame(index));

write.csv(q, file = relative_work("kegg_genomes.csv"));
write.csv(index, file = relative_work("kegg_query_index.csv"));