require(biocad_registry);

imports "data_exports" from "biocad_registry";

let biocad_registry = open_registry("root", 123456, host ="192.168.3.15");

write.csv(export_topic_structdata(biocad_registry,"plant"), file = "U:\CFM-ID\plant.csv");