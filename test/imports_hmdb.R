require(biocad_registry);

imports "hmdb_kit" from "mzkit";
imports "data_imports" from "biocad_registry";

let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");

biocad_registry |> imports_metab_repo(read.hmdb(
    xml = "U:\hmdb\hmdb_metabolites.xml", 
    convert_std = TRUE, 
    tqdm = FALSE
));