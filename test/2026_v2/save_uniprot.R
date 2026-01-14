require(biocad_registry);

imports "registry" from "biocad_registry";

let uniprot = open.uniprot("G:\biocad_registry\test\uniprotkb_taxonomy_id_2_AND_model_organ_2024_08_02.xml");
let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");

biocad_registry |> save_uniprot(uniprot);