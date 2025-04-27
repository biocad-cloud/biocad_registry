require(biocad_registry);

imports "data_imports" from "biocad_registry";

let genbank = data_imports::genbank_repo("D:\datapool\ncbi_genbank");
let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");

biocad_registry |> imports_genes(genbank);
