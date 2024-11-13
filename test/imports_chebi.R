require(biocad_registry);

imports "OBO" from "annotationKit";

let chebi = open.obo("G:\biocad_registry\test\chebi.obo");
let biocad_registry = open_registry("root", 123456, host ="192.168.3.233");

biocad_registry |> imports_chebi(chebi);