const link_reaction_metabolites = function(biocad_registry) {
    let page_size = 3000;
    let molecule = biocad_registry |> table("molecule");
    let term_metabolite = metabolite_term(biocad_registry); 

    for(page in 1:20000) {
        let start = (page_size - 1) * page_size;
        let graph_links = biocad_registry 
        |> table("reaction_graph") 
        |> limit(start, page_size) 
        |> select()
        ;

        if (length(graph_links) == 0) {
            break;
        }

        for(link in tqdm(graph_links)) {
            if (link$molecule_id == 0) {
                let mol = molecule 
                    |> left_join("db_xrefs") 
                    |> on(db_xrefs.obj_id = molecule.id) 
                    |> where(molecule.type = term_metabolite, 
                        xref = link$db_xref) 
                    |> find("molecule.id")
                    ;

                str(mol);
                stop();
            }
        }
    }
}