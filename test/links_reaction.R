require(biocad_registry);

let biocad_registry = open_registry("root", 123456);

biocad_registry |> link_reaction_metabolites(); 