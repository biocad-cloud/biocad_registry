require(biocad_registry);

let biocad_registry = open_registry("root", 123456);
let uniprot = parseUniProt(readText(file.path(@dir,"uniprotkb_taxonomy_id_2_AND_model_organ_2024_08_02.xml")));

biocad_registry |> imports_uniprot(uniprot);