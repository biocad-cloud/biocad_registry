require(biocad_registry);

let biocad_registry = open_registry("root", 123456);
let genebank = resolve_repository("J:\ossfs\pubchem\repo\pugViews");

biocad_registry |> imports_pubchem (pubchem);