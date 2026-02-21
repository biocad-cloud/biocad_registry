require(biocad_registry);

imports "rhea" from "annotationKit";
imports "registry" from "biocad_registry";

let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");

import_brenda_enzymes(
    biocad_registry, load_brenda_enzymes("F:\UniProt\brenda_2025_1.json")
);
