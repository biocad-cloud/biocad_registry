require(biocad_registry);

imports "exports" from "biocad_registry";

let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");
let repo = relative_work("./localdb");

exports::virtualcell_componentdb(biocad_registry, repo);