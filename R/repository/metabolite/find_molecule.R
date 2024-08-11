#' A common function for find a target metabolite molecule
#' 
const find_molecule = function(biocad_registry, meta, xref_id) {
    # check database via xrefs at first
    let mol_id = check_metabolite(biocad_registry, compound = meta);

    if (is.null(mol_id)) {
        # current compound is not found
        # add new
        let term_metabolite = biocad_registry::metabolite_term(biocad_registry);
        let mol = metabolite |> where(type = term_metabolite ,
            xref_id = xref_id) |> find();
        let exact_mass = formula::eval(meta$formula);

        if (exact_mass < 0) {
            exact_mass = 0;
        }

        if (is.null(mol)) {
            metabolite |> add(
                xref_id = xref_id,
                name = meta$name,
                mass = exact_mass,
                type =  term_metabolite,
                formula = meta$formula,
                parent = 0,
                note = meta$description  
            );

            mol = metabolite |> where(type =term_metabolite,
                xref_id = xref_id ) |> find();
        }

        if (is.null(mol)) {
            # error while add new metabolite
            NULL;
        } else {
            mol;
        }
    } else {
        metabolite 
        |> where(id = mol_id) 
        |> find()
        ;
    }
}