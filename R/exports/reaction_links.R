const export_reactionLinks = function(biocad_registry) {
    let all_reactions = biocad_registry |> table("reaction_graph") |> distinct() |> project("reaction");
    let graph = biocad_registry |> table("reaction_graph");

    print("get all reaction id list:");
    print(all_reactions);

    all_reactions <- as.list(all_reactions, names = `cad_rxn${all_reactions}`);

    lapply(tqdm(all_reactions), function(id) {
        let links = graph 
        |> where(reaction = id) 
        |> group_by("role") 
        |> select(role, "GROUP_CONCAT(molecule_id) AS graph")
        ;
        
        rownames(links) = ifelse( links$role == 30,"reactants","products");

        links = as.list(links, byrow=TRUE);
        links = lapply(links, molecule_id -> as.integer(strsplit(molecule_id$graph,",")));

        if (length(links) == 1) {
            NULL;
        } else {
            if (any(links$reactants == 0) || any(links$products == 0)) {
                NULL;
            } else {
                links$reaction_id = id;
                links;
            }
        }
    }) |> which(a -> !is.null(a))
    ;
}