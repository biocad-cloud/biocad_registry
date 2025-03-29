imports "ftp" from "webKit";

#' ftp reference to the ncbi ftp server
const ncbi = new ftp(server = "ftp.ncbi.nlm.nih.gov");

#' Helper function for get ncbi genbank file
#' 
#' @param asm_id the genbank assembly reference id of the target genome
#' @param repo_dir the local directory path for save the downloaded ncbi genbank file
#' 
const get_genbank = function(asm_id, repo_dir = "./") {
    let parts <- strsplit(asm_id, "_");
    let prefix = unlist(parts[1]);
    let int_id = unlist(parts[2]);
    let num_str = c(substr(int_id, 1, 3), substr(int_id, 4, 6), substr(int_id, 7, 9)); 
    let path = ["/genomes/all", prefix] |> append(num_str);
    
    # build ftp path
    path   <- paste(path, sep = "/");
    asm_id <- list.ftp_dirs(ncbi, dir = path);

    # create genbank file url reference on the ftp server
    let genbank_url = `${path}/${asm_id}/${asm_id}_genomic.gbff.gz`;
    
    print(asm_id);
    print(genbank_url);

    # make ftp download of the archive file to
    # local dir.
    ncbi |> ftp.get(file = genbank_url, save = repo_dir);
}