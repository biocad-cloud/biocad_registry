require(biocad_registry);

imports "rhea" from "annotationKit";
imports "registry" from "biocad_registry";

let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");
let rhea = rhea::open.rdf("J:\ossfs\rhea.rdf");

rhea = rhea::reactions(rhea);

imports_rhea(biocad_registry, rhea);