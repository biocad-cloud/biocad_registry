require(biocad_registry);

imports "data_imports" from "biocad_registry";
imports "annotation.genomics" from "seqtoolkit";

let operon = operon_set();

str(operon[1]);