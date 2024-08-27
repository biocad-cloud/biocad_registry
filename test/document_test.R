require(GCModeller);

imports "sabiork" from "biosystem";

setwd(@dir);

let docs = parseSbml("./1.3.1  With NAD+ or NADP+ as acceptor.xml");

str(docs);

docs = sbmlReader(docs);

for(rxn in [docs]::reactions) {
    str(docs |> metabolite_species(rxn));
    stop();
}