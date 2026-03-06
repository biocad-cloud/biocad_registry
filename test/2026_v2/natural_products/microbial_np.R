require(biocad_registry);

imports "models" from "biocad_registry";

let registry = open_registry("xieguigang", 123456, host ="192.168.3.15");

build_microbial_nps(registry);