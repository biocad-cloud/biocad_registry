#' imports the rhea reaction database
#' 
const imports_rhea = function(biocad_registry, rhea = "./rhea.rdf") {
    let key = biocad_registry |> vocabulary_id("Rhea","External Database");

    if (is.character(rhea)) {
        imports "rhea" from "annotationKit";

        rhea <- rhea::open.rdf(rhea);
        rhea <- rhea::reactions(rhea);
    }

    for(let rxn in tqdm(rhea)) {
        biocad_registry |> push_reaction(reaction = as.list(rxn), source_db = key);
    }
}