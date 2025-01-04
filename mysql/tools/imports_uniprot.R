require(biocad_registry);

let uniprot_file = ?"--uniprot" || stop("a file path to the uniprot xml database file must be provided!");
let hostname = ?"--host" || "192.168.3.233";
let do_fastcheck as boolean = ?"--fast_check" || FALSE;

let biocad_registry = open_registry("root", 123456, host =hostname);
let uniprot = open.uniprot(uniprot_file);

biocad_registry |> imports_uniprot(uniprot, 
    fast_check = as.logical(do_fastcheck)
);