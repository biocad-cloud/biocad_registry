imports "ftp" from "webKit";

#' Download GenBank Assembly File from NCBI FTP Server
#' 
#' This helper function constructs the NCBI FTP path for a given assembly ID, retrieves 
#' the corresponding GenBank genomic file (*.gbff.gz), and downloads it to a local directory.
#'
#' @param asm_id Character. NCBI GenBank assembly accession ID in the format "prefix_Integers" 
#'   (e.g., "GCF_123456789"). The prefix typically indicates the assembly type (e.g., GCF/GCA), 
#'   followed by a 9+ digit identifier.
#' @param repo_dir Character. Local directory path to save the downloaded file. 
#'   Defaults to the current working directory ("./").
#' 
#' @return Invisibly returns the full path to the downloaded file. Primarily called for 
#'   its side effect of downloading the GenBank file to the specified directory.
#' 
#' @details The function:
#' \enumerate{
#'   \item Parses the assembly ID into prefix and numeric components
#'   \item Constructs the NCBI FTP directory path using the ID structure
#'   \item Retrieves the compressed GenBank file (*_genomic.gbff.gz)
#'   \item Downloads the file to the specified local directory
#' }
#' 
#' @note Requires an active internet connection and proper NCBI FTP access. File paths 
#'   are case-sensitive. The function assumes `ncbi` is a pre-configured FTP connection 
#'   object with `list.ftp_dirs()` and `ftp.get()` methods.
#' 
#' @examples
#' \dontrun{
#' # Download assembly GCF_123456789 to the current directory
#' get_genbank("GCF_123456789")
#' 
#' # Save to a custom directory
#' get_genbank("GCA_987654321", repo_dir = "path/to/genbank_files")
#' }
const get_genbank = function(asm_id, repo_dir = "./") {
    let parts <- strsplit(asm_id, "_");
    let prefix = unlist(parts[1]);
    let int_id = unlist(parts[2]);
    let num_str = c(substr(int_id, 1, 3), substr(int_id, 4, 6), substr(int_id, 7, 9)); 
    let path = ["/genomes/all", prefix] |> append(num_str);
    let ncbi = new ftp(server = "ftp.ncbi.nlm.nih.gov");

    # build ftp path
    path   <- paste(path, sep = "/");
    asm_id <- list.ftp_dirs(ncbi, dir = path);

    # create genbank file url reference on the ftp server
    # may contains multiple version assembly file
    #
    let genbank_url = `${path}/${asm_id}/${asm_id}_genomic.gbff.gz`;
    
    print(asm_id);
    print(genbank_url);

    for(let url in genbank_url) {
        let local_file = file.path(repo_dir, basename(url, TRUE));
        let check = file.exists(local_file);

        if (check) {
            check = gz_check(local_file);
        }

        cat(`${url} => ${local_file} ... `);

        # download all version assembly file at here?
        if (!check) {
            # make ftp download of the archive file to
            # local dir.
            ncbi |> ftp.get(file = genbank_url, save = local_file);
            
            cat("done!\n");
        } else {
            cat("skip!\n");
        }
    }
}