require(GCModeller);

imports "NCBI" from "annotationKit";

let repo = list.files("F:\ecoli\regprecise", pattern = "*.xml",recursive = FALSE);
let genome_names = basename(repo);

print(genome_names);

let db = genbank_assemblyDb("D:\datapool\assembly_summary_genbank.txt");
let q = [];
let index = [];

for(let name in tqdm(genome_names)) {
    let result = db |> NCBI::query(name, cutoff = 0.9);

    index = append(index, result |> attr("index"));
    q = append(q, result);    
}

q = unlist(q);
index = unlist(index);

print(as.data.frame(index));

write.csv(q, file = relative_work("genomes.csv"));
write.csv(index, file = relative_work("query_index.csv"));