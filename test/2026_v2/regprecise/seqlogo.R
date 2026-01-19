require(biocad_registry);

imports "registry" from "biocad_registry";

open_registry("xieguigang", 123456, host ="192.168.3.15")
|> update_logo()
;
