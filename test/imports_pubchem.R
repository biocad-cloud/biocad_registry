require(biocad_registry);

imports "pubchem_kit" from "mzkit";

let biocad_registry = open_registry("root", 123456);
let pubchem = resolve_repository("J:\ossfs\pubchem\repo\pugViews");

biocad_registry |> imports_pubchem (pubchem);