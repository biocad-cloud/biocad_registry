require(biocad_registry);

imports "registry" from "biocad_registry";

let uniprot = open.uniprot("U:\fungi-20251120\Saccharomycetes.xml");
let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");

biocad_registry |> save_uniprot(uniprot);