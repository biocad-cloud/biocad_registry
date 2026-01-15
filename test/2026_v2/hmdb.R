require(biocad_registry);
require(GCModeller);
require(mzkit);

imports "hmdb_kit" from "mzkit";
imports "setup" from "biocad_registry";

let registry = open_registry("root","123456");
let hmdb = read.hmdb(
    xml = "K:\hmdb_metabolites.xml", 
    convert_std = FALSE, 
    tqdm = TRUE
);

registry |> setup_hmdb(hmdb);