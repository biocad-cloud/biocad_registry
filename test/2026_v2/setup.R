require(biocad_registry);
require(GCModeller);
require(mzkit);

imports "massbank" from "mzkit";
imports "setup" from "biocad_registry";

let registry = open_registry("root","123456");

# registry |> setup_kegg(GCModeller::kegg_compounds(rawList = TRUE,reference_set = FALSE));
registry |> setup_refmet(read.RefMet("G:\biocad_registry\data\refmet.csv"));