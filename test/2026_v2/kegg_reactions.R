require(biocad_registry);

imports "registry" from "biocad_registry";

let repo = kegg_reactions();
let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");

biocad_registry |> imports_kegg_reactions(repo);