require(biocad_registry);
require(Daisy);

imports "data_exports" from "biocad_registry";

let mona_msp_repo = ?"--mona" || stop("A directory path that contains multiple mona database msp file must be provided!");
let lib_output = ?"--output" || normalizePath(file.path(mona_msp_repo,"libs"));
let list_repo_files = list.files(mona_msp_repo, pattern = "*.msp");
let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");
let mona_metab = data_exports::export_metabolites(biocad_registry);

print("Start to build mona reference library:");
print(mona_msp_repo);
print(`  -> ${lib_output}`);
print("library files:");
print(basename(list_repo_files));

attr(mona_metab, "mapping")
|> JSON::json_encode()
|> writeLines(con = file.path(lib_output,"mapping.json"))
;

Daisy::build_mona_lcms(list_repo_files, lib_output, metabolites = mona_metab);