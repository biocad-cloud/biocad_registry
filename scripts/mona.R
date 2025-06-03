require(biocad_registry);
require(Daisy);

imports "data_exports" from "biocad_registry";

let mona_msp_repo = ?"--mona" || stop("A directory path that contains multiple mona database msp file must be provided!");
let lib_output = ?"--output" || normalizePath(file.path(mona_msp_repo,"libs"));
let list_repo_files = list.files(mona_msp_repo, pattern = "*.msp");
let biocad_registry = open_registry("root", 123456, host ="localhost");
let mona_metab = data_exports::export_mona_metabolites(biocad_registry);

print("Start to build mona reference library:");
print(mona_msp_repo);
print(`  -> ${lib_output}`);
print("library files:");
print(basename(list_repo_files));

Daisy::build_mona_lcms(list_repo_files, lib_output, metabolites = mona_metab);