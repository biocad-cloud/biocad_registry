require(biocad_registry);
require(GCModeller);

imports "setup" from "biocad_registry";

let registry = open_registry("xieguigang", 123456, host ="192.168.3.15");

setup_ko(registry);