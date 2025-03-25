imports "ftp" from "webKit";

let ncbi = new ftp(server = "ftp.ncbi.nlm.nih.gov");
let get_genbank = function(asm_id, repo_dir = "./") {
    let parts <- strsplit(asm_id, "_");
    let prefix = unlist(parts[1]);
    let int_id = unlist(parts[2]);
    let num_str = c(substr(int_id, 1, 3), substr(int_id, 4, 6), substr(int_id, 7, 9)); 
    let path = ["/genomes/all", prefix] |> append(num_str);
    
    path <- paste(path, sep = "/");
    asm_id <- ncbi |> list.ftp_dirs(dir = path);
    
    print(asm_id);

    let genbank_url = `${path}/${asm_id}/${asm_id}_genomic.gbff.gz`;

    print(genbank_url);

    ncbi |> ftp.get(file = genbank_url, save = repo_dir);
}

get_genbank("GCA_000012105", repo_dir = "Z:/");