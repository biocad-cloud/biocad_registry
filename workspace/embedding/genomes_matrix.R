require(biocad_registry);

imports "data_exports" from "biocad_registry";

open_registry("xieguigang", 123456, host ="192.168.3.15")
|> data_exports::export_fingerprints()
|> write.csv(file = file.path(@dir, "nt_fingerprints.csv"))
;