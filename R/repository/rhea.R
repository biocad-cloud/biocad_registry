#' imports the rhea reaction database
#' 
const imports_rhea = function(biocad_registry, rhea) {
    let reactions = biocad_registry |> table("reaction");
    let graph = biocad_registry |> table("regulation_graph");
    let metabolite_graph = biocad_registry |> table("reaction_graph");
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
                equation = rxn$definition,
                note = rxn$comment
            );

            r = reactions |> where(db_xref = rxn$entry) |> find();
        } 

        # add ec number for the enzymatic regulation
        for(ec_number in rxn$enzyme) {
            if (!(graph |> check(term = ec_number, role = enzyme_term, reaction_id = r$id))) {
                graph |> add(
                    term = ec_number,
                    role = enzyme_term,
                    reaction_id = r$id
                );
            }
        }

        # add external reference id of current reaction object
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

        # how to link the reaction with metabolite molecules
        for(compound in rxn$compounds) {
            if (compound$side != "*") {
                let role_id = biocad_registry |> vocabulary_id(compound$side, "Compound Role");

                compound = compound$compound;

                if (!(metabolite_graph |> check(
                        reaction = r$id,
                        db_xref = compound$entry,
                        role = role_id
                    ))) {
                    metabolite_graph |> add(
                        reaction = r$id,
                        molecule_id = 0,
                        db_xref = compound$entry,
                        role = role_id,
                        factor = 1,
                        note = `${compound$name} (${compound$formula})`
                    );
                }
            }
        }
    }
}