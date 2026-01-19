require(biocad_registry);
require(GCModeller);

imports "setup" from "biocad_registry";
imports "regprecise" from "TRNtoolkit";

let registry = open_registry("xieguigang", 123456, host ="192.168.3.15");
let motif_db = read.regprecise("F:\ecoli\regprecise.xml");

registry |> save_motif(motif_db);