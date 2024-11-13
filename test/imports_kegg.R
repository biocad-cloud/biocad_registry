require(biocad_registry);
require(GCModeller);

let biocad_registry = open_registry("root", 123456, host ="192.168.3.233");
let kegg = GCModeller::kegg_compounds(reference_set=FALSE);

biocad_registry |> imports_kegg(kegg);