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

    for(let meta in pubchem) {
        # try({
            let compound = as.list(metadata.pugView(meta));
            let cid = `PubChem:${compound$ID}`;
            let mol = biocad_registry |> find_molecule(compound, cid);

            if (is.null(mol)) {
                # error while add new metabolite
                next;
            } else {
                biocad_registry |> __push_compound_metadata(compound, mol);
            }
        # });
    }
}

const imports_odor = function(biocad_registry, pubchem) {
    let pubchem_term = biocad_registry |> vocabulary_id("pubchem","External Database");
    let odor_class = list(
        odor  = biocad_registry |> vocabulary_id("odor","Odor Category"),
        taste = biocad_registry |> vocabulary_id("taste","Odor Category"),
        color = biocad_registry |> vocabulary_id("color","Odor Category")
    ); 
    let odors = biocad_registry |> table("odor");

    for(let meta in pubchem) {
        let data = metadata.pugView(meta);
        let odors_data = odors(data);
        let cid  = [data]::ID;
        let molecule = biocad_registry |> table("db_xrefs") 
            |> where(db_key=pubchem_term, xref=cid)
            |> find()
            ;

        if (nrow(odors_data) > 0 && !is.null(molecule)) {
            let registry_id = molecule$obj_id;

            for(let term in as.list(odors_data,byrow=TRUE)) {
                let class_id = odor_class[[term$category]];

                if (!(odors |> check(molecule_id= registry_id, category = class_id, odor = term$odor))) {
                    odors |> add(
                        molecule_id= registry_id, 
                        category = class_id, 
                        odor = term$odor,
                        hashcode = md5(term$odor),
                        value = 0,
                        unit = 0,
                        text = term$text
                    );
                }
            }
        }
    }
}