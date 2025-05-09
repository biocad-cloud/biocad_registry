require(biocad_registry);
require(GCModeller);

let biocad_registry = open_registry("root", 123456, host ="192.168.3.233");
let kegg = GCModeller::kegg_compounds(reference_set=FALSE);
# let reactions = GCModeller::kegg_reactions();

biocad_registry |> imports_kegg(kegg);
# biocad_registry |> imports_kegg_reaction(reactions);