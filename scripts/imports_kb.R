require(biocad_registry);
require(mzkit);

imports "pubchem_kit" from "mzkit";
imports "data_imports" from "biocad_registry";

let pubmed_json = ?"--kb" || stop("the pubmed knowledge kb json file is required!");
let topic = ?"--topic";
let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");

pubmed_json = read.pubmed(pubmed_json, lazy = TRUE, parse.json = TRUE);
biocad_registry |> imports_pubmed_kb(
    pubmed = pubmed_json, 
    topic = strsplit(topic,",")
);
