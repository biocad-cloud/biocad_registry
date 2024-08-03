const imports_pubchem = function(biocad_registry, pubchem) {
    let term_metabolite = biocad_registry::metabolite_term(biocad_registry);
    let entity_metabolite = biocad_registry::molecule_entity(biocad_registry);
    let pool = {
        if (is.array(pubchem)) {
            tqdm(pubchem);
        } else {
            # is pipeline enumerator
            # tqdm can not be used
            pubchem;
        }
    }
    let db_xrefs = biocad_registry |> table("db_xrefs");
    let compartments = biocad_registry |> table("subcellular_compartments");
    let location_link = biocad_registry |> table("subcellular_location");

    for(let compound in pubchem) {
        
    }
}