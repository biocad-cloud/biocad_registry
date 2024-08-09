imports "massbank" from "mzkit";

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

    chebi = massbank::extract_chebi_compounds(chebi);

    for(let meta in tqdm(chebi)) {
        let xref_id = [meta]::ID;
        let mol = metabolite |> where(type = term_metabolite ,
            xref_id = xref_id ) |> find();

        meta = as.list(meta);

        if (is.null(mol)) {
            metabolite |> add(
                xref_id = xref_id,
                name = meta$name,
                mass = formula::eval(meta$formula),
                type =  term_metabolite,
                formula = meta$formula,
                parent = 0,
                note = meta$description  
            );

            mol =metabolite  |> where(type =term_metabolite,
                xref_id = xref_id ) |> find();
        }

        if (is.null(mol)) {
            # error while add new metabolite
            next;
        } else {
            biocad_registry |> __push_compound_metadata(meta, mol);
        }
    }
}