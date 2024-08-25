require(GCModeller);

imports "sabiork" from "biosystem";

setwd(@dir);

let docs = parseSbml("./d08d5cb9ea1482b8810574b0f434334c.xml");

str(docs);

docs = sbmlReader(docs);

for(rxn in [docs]::reactions) {
    str(rxn);
}