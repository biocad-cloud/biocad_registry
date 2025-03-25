imports "ftp" from "webKit";

let ncbi = new ftp(server = "ftp.ncbi.nlm.nih.gov");

print(ncbi |> list.ftp_dirs(dir = "/genomes/all/GCA/000/012/105/"));