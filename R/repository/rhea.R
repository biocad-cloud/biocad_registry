const imports_rhea = function(biocad_registry, rhea) {
    let reactions = biocad_registry |> table("reaction");
    let graph = biocad_registry |> table("regulation_graph");
    let xrefs = biocad_registry |> table("db_xrefs");
    let enzyme_term = biocad_registry |> enzyme_regulation();
    let rxn_term = biocad_registry |> reaction_model();  

    for(let rxn in tqdm(rhea)) {
        rxn = as.list(rxn);

        # check reaction
        let r = reactions |> where(db_xref = rxn$entry) |> find();

        if (is.null(r)) {
            reactions |> add(
                db_xref = rxn$entry,
                name = rxn$definition,
                equation = rxn$equation,
                note = rxn$comment
            );

            r = reactions |> where(db_xref = rxn$entry) |> find();
        } 

        for(ec_number in rxn$enzyme) {
            if (!(graph |> check(term = ec_number, role = enzyme_term, reaction_id = r$id))) {
                graph |> add(
                    term = ec_number,
                    role = enzyme_term,
                    reaction_id = r$id
                );
            }
        }

        for(db_xref in rxn$db_xrefs) {
            let [name, text] = db_xref;
            let db_key = biocad_registry |> vocabulary_id(name, "External Database");

            if (!(xrefs |> check(obj_id = r$id, db_key = db_key, xref = text, type = rxn_term))) {
                xrefs |> add(
                    obj_id = r$id,
                    db_key = db_key, 
                    xref = text, 
                    type = rxn_term
                );
            }
        }
    }
}