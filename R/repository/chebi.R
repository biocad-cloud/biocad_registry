imports "massbank" from "mzkit";

#' imports the metabolite set from chebi database
#' 
#' @param chebi the chebi ontology OBO database, the metabolite 
#'     compound data will be extract from this database obejct. 
#'
const imports_chebi = function(biocad_registry, chebi) {
    let term_metabolite   = biocad_registry::metabolite_term(biocad_registry);
    let entity_metabolite = biocad_registry::molecule_entity(biocad_registry);
    let compartments      = biocad_registry |> table("subcellular_compartments");
    let location_link     = biocad_registry |> table("subcellular_location");
    let metabolite        = biocad_registry |> table("molecule");

    chebi = massbank::extract_chebi_compounds(chebi);

    for(let meta in tqdm(chebi)) {
        let xref_id  = [meta]::ID;
        let compound = as.list(meta);
        let mol_info = biocad_registry |> find_molecule(compound, xref_id);

        if (is.null(mol_info)) {
            # error while add new metabolite
            next;
        } else {
            biocad_registry 
            |> __push_compound_metadata(compound, mol_info)
            ;
        }
    }
}