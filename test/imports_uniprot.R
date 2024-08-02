require(biocad_registry);

imports "uniprot" from "seqtoolkit";

let biocad_registry = open_registry("root", 123456);
let uniprot = parseUniProt(readText(file.path(@dir,"uniprotkb_taxonomy_id_2_AND_model_organ_2024_08_02.xml")));

uniprot = as.data.frame(uniprot);

print(uniprot);