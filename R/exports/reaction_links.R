const export_reactionLinks = function(biocad_registry) {
    let all_reactions = biocad_registry |> table("reaction_graph") |> distinct() |> project("reaction");

    print("get all reaction id list:");
    print(all_reactions);
}