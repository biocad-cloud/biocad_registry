require(biocad_registry);

let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");
# let uniprot = parseUniProt(readText(file.path(@dir,"uniprotkb_taxonomy_id_2_AND_model_organ_2024_08_02.xml")));

biocad_registry |> imports_uniprot(relative_work("uniprotkb_taxonomy_id_2_AND_model_organ_2024_08_02.xml"));