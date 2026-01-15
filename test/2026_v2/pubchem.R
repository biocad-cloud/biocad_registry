require(biocad_registry);

imports "pubchem_kit" from "mzkit";
imports "registry" from "biocad_registry";

let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");
let pubchem = pubchem_kit::resolve_repository("J:\ossfs\pubchem\repo\pugViews");

biocad_registry |> imports_pubchem(pubchem);