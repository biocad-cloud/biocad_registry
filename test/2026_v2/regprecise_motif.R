require(biocad_registry);
require(GCModeller);

imports "setup" from "biocad_registry";
imports "regprecise" from "TRNtoolkit";

let registry = open_registry("root","123456");
let motif_db = read.regprecise("F:\ecoli\regprecise.xml");

registry |> save_motif(motif_db);