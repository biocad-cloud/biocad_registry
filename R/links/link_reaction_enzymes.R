const link_reaction_enzymes = function(biocad_registry) {
    let page_size = 3000;
    let molecule = biocad_registry |> table("molecule");
    let func = biocad_registry |> table("molecule_function");
    let term_protein = protein_term(biocad_registry); 

    for(page in 1:20000) {
        let start = (page - 1) * page_size;
        let graph_links = biocad_registry 
        |> table("regulation_graph") 
        |> limit(start, page_size) 
        |> select()
        ;

        print(biocad_registry |> get_last_sql());

        if (length(graph_links) == 0) {
            break;
        } else {
            # print(graph_links);
        }

        for(let link in tqdm(as.list(graph_links, byrow = TRUE))) {
            let ec_number = link$term;
            let proteins = molecule 
            |> left_join("db_xrefs") 
            |> on(db_xrefs.obj_id = molecule.id) 
            |> where(molecule.type = term_protein, xref = ec_number) 
            |> select("molecule.*")
            ;
            let link_term_id = link$id;

            for(enzyme in as.list(proteins, byrow = TRUE)) {
                # add function links
                ec_number = enzyme$id;

                if (!(func |> check(molecule_id = ec_number, regulation_term = link_term_id))) {
                    func |> add(
                        molecule_id = ec_number, 
                        regulation_term = link_term_id
                    );
                }
            }
        }
    }
}