require(biocad_registry);

imports "pubchem_kit" from "mzkit";

let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");
let pubchem = resolve_repository("J:\ossfs\pubchem\repo\pugViews");

biocad_registry |> imports_pubchem (pubchem);
# biocad_registry |> imports_odor(pubchem,fast_check=TRUE); 