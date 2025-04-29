require(biocad_registry);

let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");

str(as.list(biocad_registry |> get_taxinfo("Pholiotina communis")));


print(as.data.frame(biocad_registry |> find_taxinfo("Pholiotina")));


print(biocad_registry |> taxonomy_lineage("Pholiotina vexans") |> toString());