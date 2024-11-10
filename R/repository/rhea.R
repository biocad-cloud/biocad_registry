#' imports the rhea reaction database
#' 
const imports_rhea = function(biocad_registry, rhea) {
    for(let rxn in tqdm(rhea)) {
        biocad_registry |> push_reaction(reaction = as.list(rxn));
    }
}