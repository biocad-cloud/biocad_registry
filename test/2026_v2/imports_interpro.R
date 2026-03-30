require(biocad_registry);

imports "hmmer" from "seqtoolkit";
imports "models" from "biocad_registry";

let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");
let terms = load_interprodb("F:\interpro.xml");

biocad_registry |> imports_interprodb(terms);
