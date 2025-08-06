require(biocad_registry);

imports "massbank" from "mzkit";
imports "data_imports" from "biocad_registry";

let dataset = read.SDF(file = "G:\biocad_registry\data\lipidmaps.sdf", lazy = FALSE) |> lipid_metabolites();
let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");

biocad_registry |> imports_metab_repo(dataset);

