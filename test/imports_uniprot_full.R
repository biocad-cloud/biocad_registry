require(biocad_registry);

let biocad_registry = open_registry("root", 123456, host ="192.168.3.233");
let uniprot = open.uniprot("J:\uniprot-all.xml\92938b97fc8aeaef3291c581f36554cc0301be29.xml");

biocad_registry |> imports_uniprot(uniprot, fast_check = TRUE);