require(biocad_registry);

imports "pubchem_kit" from "mzkit";
imports "data_imports" from "biocad_registry";

let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");
# imports single
# let pubchem = resolve_repository("J:\ossfs\pubchem\repo\pugViews");

# biocad_registry |> imports_pubchem (pubchem);
# biocad_registry |> imports_odor(pubchem,fast_check=TRUE); 

# imports batch
let pubchem = pubchem_repo("J:\ossfs\pubchem\repo\pugViews");

biocad_registry |> imports_pubchem_repo(pubchem);