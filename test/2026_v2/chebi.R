require(biocad_registry);

imports "OBO" from "annotationKit";
imports "setup" from "biocad_registry";

let chebi = open.obo("G:\biocad_registry\test\chebi.obo");
let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");

biocad_registry |> setup_chebi(chebi);