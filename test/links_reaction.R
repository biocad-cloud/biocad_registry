require(biocad_registry);

let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");

biocad_registry |> link_reaction_metabolites(refresh=TRUE); 