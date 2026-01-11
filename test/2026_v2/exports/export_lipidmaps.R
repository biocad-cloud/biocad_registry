require(biocad_registry);

imports "exports" from "biocad_registry";

write.csv(
    exports::export_smiles_data(registry = open_registry("xieguigang", 123456, host ="192.168.3.15"), 
                            dbname = "lipidmaps_id"), 
    file = relative_work("lipidmaps.csv"), 
    row.names = FALSE);