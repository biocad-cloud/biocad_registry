require(biocad_registry);

let biocad_registry = open_registry("root", 123456, host ="192.168.3.233");

biocad_registry |> link_reaction_metabolites(); 