require(biocad_registry);
require(GCModeller);

imports "registry" from "biocad_registry";

let registry = open_registry("xieguigang", 123456, host ="192.168.3.15");
let metacyc = open.biocyc("F:\UniProt\BioCyc\tier1\meta\25.1");

registry |> imports_metacyc_compounds(metacyc);
registry |> imports_metacyc_reactions(metacyc);