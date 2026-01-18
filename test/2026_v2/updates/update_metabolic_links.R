require(biocad_registry);

imports "models" from "biocad_registry";

open_registry("xieguigang", 123456, host ="192.168.3.15")
|> update_metabolic_network()
;