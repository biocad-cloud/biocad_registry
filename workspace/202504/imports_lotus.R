require(mzkit);
require(biocad_registry);

imports "massbank" from "mzkit";
imports "data_imports" from "biocad_registry";

let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");
let nplib = read.lotus("G:\\LOTUSlatest\\NPOC2021\\NPOC2021\\lotusUniqueNaturalProduct.bson");

biocad_registry |> imports_lotus_np(nplib );