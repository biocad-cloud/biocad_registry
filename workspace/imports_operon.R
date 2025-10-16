require(biocad_registry);

imports "data_imports" from "biocad_registry";
imports "annotation.genomics" from "seqtoolkit";

let operon = operon_set();
let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");

biocad_registry |> imports_operon(operon);