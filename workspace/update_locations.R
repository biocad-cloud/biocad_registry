require(biocad_registry);

imports "models" from "biocad_registry";

let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");
let location_data = read.csv("G:\biocad_registry\data\subcellular-locations.csv", row.names = 1, check.names = FALSE);

print(location_data, max.print = 6);

biocad_registry |> models::update_location(location_data);