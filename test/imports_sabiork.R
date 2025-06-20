require(biocad_registry);

let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");
let repo = list.files("J:\2022_nar\sabio-rk\.cache", pattern = "*.xml", recursive =TRUE);

print(repo);

biocad_registry |> imports_sabiork(repo);