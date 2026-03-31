require(biocad_registry);

imports "registry" from "biocad_registry";

# let uniprot = open.uniprot("U:\fungi-20251120\Saccharomycetes.xml");
# let uniprot = open.uniprot("U:\fungi-20251120\Eurotiomycetes.xml");

# let uniprot = open.uniprot("U:\uniprot_sprot_archaea.xml");
# let uniprot = open.uniprot("U:\uniprot_sprot_bacteria.xml");
# let uniprot = open.uniprot("U:\uniprot_sprot_fungi.xml");

# "F:\datapool\20260301\background\uniprotkb_taxonomy_id_4565_2026_03_27.xml"

let uniprot = open.uniprot("C:\Users\Administrator\Downloads\uniprotkb_taxonomy_id_9606_2026_03_04.xml");
let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");

# biocad_registry |> save_uniprot(uniprot);
biocad_registry |> imports_struct_domains(uniprot);