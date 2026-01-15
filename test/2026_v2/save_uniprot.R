require(biocad_registry);

imports "registry" from "biocad_registry";

# let uniprot = open.uniprot("U:\fungi-20251120\Saccharomycetes.xml");
# let uniprot = open.uniprot("U:\fungi-20251120\Eurotiomycetes.xml");

# let uniprot = open.uniprot("U:\uniprot_sprot_archaea.xml");
# let uniprot = open.uniprot("U:\uniprot_sprot_bacteria.xml");
# let uniprot = open.uniprot("U:\uniprot_sprot_fungi.xml");

let uniprot = open.uniprot("U:\uniprot_trembl_bacteria.xml");
let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");

biocad_registry |> save_uniprot(uniprot);