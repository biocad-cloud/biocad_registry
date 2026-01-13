require(biocad_registry);

imports "registry" from "biocad_registry";

let uniprot = open.uniprot("J:\uniprot-all.xml\92938b97fc8aeaef3291c581f36554cc0301be29.xml");
let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");

biocad_registry |> save_uniprot(uniprot);