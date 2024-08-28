require(GCModeller);
require(biocad_registry);

imports "sabiork" from "biosystem";

setwd(@dir);

let biocad_registry = open_registry("root", 123456);
let docs = parseSbml("./1.3.1  With NAD+ or NADP+ as acceptor.xml");

str(docs);

docs = sbmlReader(docs);

for(rxn in [docs]::reactions) {
    let metabolites = docs |> metabolite_species(rxn);
    let all_db_xrefs = lapply(metabolites, i -> i$xrefs$xref) |> unlist() |> unique();
    let info = docs |> enzyme_info(rxn);
    let [ec_number,uniprot] = info;
    let funcs = biocad_registry |> enzyme_function(
        enzyme_id = uniprot,
        ec_number = ec_number,
        metabolites = metabolites);

    # str(metabolites);
    print(all_db_xrefs);
    str(info);
    print(funcs);

    # stop();
}