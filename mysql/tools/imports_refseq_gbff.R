require(biocad_registry);

options(memory.load = "max");

let repo_dir = ?"--repo" || stop("a directory folder path that contains the unzip gbff genbank files must be provided!");
let gbff_files = list.files(repo_dir, pattern = "*.gbff",recursive =TRUE);

print("imports genbank refseq files");
print(basename(gbff_files));

for(let file in tqdm(gbff_files)) {
    
}
