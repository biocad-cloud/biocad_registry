require(biocad_registry);

imports "exports" from "biocad_registry";

write.csv(
    exports::metabolite_table(registry = open_registry("xieguigang", 123456, host ="192.168.3.15"), 
                            dbname = "hmdb_id"), 
    file = relative_work("hmdb.csv"), 
    row.names = FALSE);