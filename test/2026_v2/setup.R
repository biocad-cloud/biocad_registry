require(biocad_registry);
require(GCModeller);
require(mzkit);

imports "massbank" from "mzkit";
imports "setup" from "biocad_registry";

let registry = open_registry("root","123456");

registry |> setup_metabolites(
    kegg = GCModeller::kegg_compounds(rawList = TRUE,reference_set = FALSE),
    refmet = read.RefMet("G:\biocad_registry\data\refmet.csv") 
);