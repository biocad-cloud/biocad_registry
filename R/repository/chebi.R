#' imports the metabolite set from chebi database
#' 
const imports_chebi = function(biocad_registry, chebi) {
    let term_metabolite = biocad_registry::metabolite_term(biocad_registry);
    let entity_metabolite = biocad_registry::molecule_entity(biocad_registry);
    let db_xrefs = biocad_registry |> table("db_xrefs");
    let compartments = biocad_registry |> table("subcellular_compartments");
    let location_link = biocad_registry |> table("subcellular_location");
    let metabolite = biocad_registry |> table("molecule");
    let seq_graph = biocad_registry |> table("sequence_graph");
}