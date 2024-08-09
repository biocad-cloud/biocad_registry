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
    let db_xrefs = biocad_registry |> table("db_xrefs");
    let compartments = biocad_registry |> table("subcellular_compartments");
    let location_link = biocad_registry |> table("subcellular_location");
    let metabolite = biocad_registry |> table("molecule");
    let seq_graph = biocad_registry |> table("sequence_graph");

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

const __push_compound_metadata = function(biocad_registry, compound, mol) {
    let xrefs = compound$xref;
    let smiles = gsub(xrefs$SMILES, "%",""); 

    xrefs$InChIkey = NULL;
    xrefs$InChI  = NULL;
    xrefs$SMILES = NULL;
    xrefs$extras  = NULL;

    let met_struct = SMILES::parse(trim(smiles, '" '), strict = FALSE);
    let atoms_vec = SMILES::atoms(met_struct);

    atoms_vec = atoms_vec 
    |> groupBy("group") 
    |> lapply(grp -> sum(grp$links))
    ;

    let embedding = bencode( names(atoms_vec));
    atoms_vec = as.numeric(unlist(atoms_vec));
    atoms_vec = base64(packBuffer(atoms_vec )|> zlib_stream());

    if (!(seq_graph |> check(molecule_id = mol$id))) {
        seq_graph |> add(
            molecule_id = mol$id,
            sequence = smiles,
            seq_graph = atoms_vec,
            embedding = embedding
        );
    }

    for(dbname in names(xrefs)) {
        let idlist = xrefs[[dbname]];
        let db_key = biocad_registry |> vocabulary_id(dbname, "External Database");

        if (length(idlist) > 0) {
            for(id in idlist) {
                if (!(db_xrefs |> check(obj_id = mol$id,
                    db_key = db_key,
                    xref = id,
                    type = term_metabolite ))) {
                        db_xrefs |> add(
                            obj_id = mol$id,
                            db_key = db_key,
                            xref = id,
                            type =term_metabolite
                        );
                    }
            }
        }
    }
}