imports "pubchem_kit" from "mzkit";
imports "formula" from "mzkit";
imports "SMILES" from "mzkit";

#' imports the reference metabolite set from pubchem database
#' 
const imports_pubchem = function(biocad_registry, pubchem) {
    let term_metabolite = biocad_registry::metabolite_term(biocad_registry);
    let entity_metabolite = biocad_registry::molecule_entity(biocad_registry);
    let pool = {
        if (is.array(pubchem)) {
            tqdm(pubchem);
        } else {
            # is pipeline enumerator
            # tqdm can not be used
            pubchem;
        }
    }
    let compartments = biocad_registry |> table("subcellular_compartments");
    let location_link = biocad_registry |> table("subcellular_location");
    let metabolite = biocad_registry |> table("molecule");    

    for(let compound in pubchem) {
        compound = as.list(metadata.pugView(compound));

        let cid = `PubChem:${compound$ID}`;
        let mol = metabolite  |> where(type = term_metabolite ,
            xref_id = cid ) |> find();

        if (is.null(mol)) {
            metabolite |> add(
                xref_id = cid,
                name = compound$name,
                mass = formula::eval(compound$formula),
                type =  term_metabolite,
                formula = compound$formula,
                parent = 0,
                note = compound$description  
            );

            mol =metabolite  |> where(type =term_metabolite,
                xref_id = cid ) |> find();
        }

        if (is.null(mol)) {
            # error while add new metabolite
            next;
        } else {
            biocad_registry |> __push_compound_metadata(compound, mol);
        }
    }
}

