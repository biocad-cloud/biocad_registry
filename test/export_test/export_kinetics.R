require(biocad_registry);

let biocad_registry = open_registry("root", 123456, host ="192.168.3.233");
let km = export_enzymatic(biocad_registry);

print(km);

write.csv(km, file = file.path(@dir, "km.csv"), row.names =FALSE);