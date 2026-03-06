require(biocad_registry);

imports "exports" from "biocad_registry";

open_registry("xieguigang", 123456, host ="192.168.3.15")
|> export_smiles_data(topic = "plant natural products")
|> write.csv(file = relative_work("plant_np.csv"))
;