require(biocad_registry);

imports "annotation.workflow" from "seqtoolkit";
imports "models" from "biocad_registry";

open_registry("xieguigang", 123456, host ="192.168.3.15")
|> make_protein_clusters()
;
