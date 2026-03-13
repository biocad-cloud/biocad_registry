require(biocad_registry);
require(mzkit);

imports "models" from "biocad_registry";
imports "pubchem_kit" from "mzkit";

let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");

for(file in [
#     "F:\pathways\PubChem_pathway_Plant_Reactome__Data_Source__Plant_Reactome.json"
# "F:\pathways\PubChem_pathway_PlantCyc__Data_Source__PlantCyc.json"
# "F:\pathways\PubChem_pathway_reactome.json"
# "F:\pathways\PubChem_pathway_wikipathway.json"
"F:\pathways\PubChem_pathway_PathBank__Data_Source__PathBank.json"]) {

    biocad_registry |> imports_pathways(parse_pathway_graph(readText(file)) );
}