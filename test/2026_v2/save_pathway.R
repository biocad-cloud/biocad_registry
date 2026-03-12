require(biocad_registry);
require(mzkit);

imports "models" from "biocad_registry";
imports "pubchem_kit" from "mzkit";

let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");
let pathways = parse_pathway_graph(readText("F:\\PubChem_pathway_biocyc.json"));

biocad_registry |> imports_pathways(pathways );