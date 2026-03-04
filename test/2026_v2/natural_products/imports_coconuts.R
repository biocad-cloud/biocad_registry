require(biocad_registry);
require(GCModeller);
require(mzkit);

imports "massbank" from "mzkit";
imports "registry" from "biocad_registry";

let registry = open_registry("xieguigang", 123456, host ="192.168.3.15");