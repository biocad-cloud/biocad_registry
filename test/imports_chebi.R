require(biocad_registry);

imports "OBO" from "annotationKit";
imports "data_imports" from "biocad_registry";

let chebi = open.obo("G:\biocad_registry\test\chebi.obo");
let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");

# imports single one by one
# biocad_registry |> imports_chebi(chebi);

# imports in batch mode
biocad_registry |> imports_chebi_repo(chebi);