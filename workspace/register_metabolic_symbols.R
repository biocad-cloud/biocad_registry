require(biocad_registry);

imports "models" from "biocad_registry";

let registry = open_registry("xieguigang", 123456, host ="192.168.3.15");

registry |> register_metabolic_symbols();
# registry |> update_symbolname();
