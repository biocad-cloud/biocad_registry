imports "BioCyc" from "annotationKit";

#' helper function for imports the metacyc molecules and reactions
#' 
const imports_metacyc = function(biocad_registry, metacyc) {
    let term_metabolite   = biocad_registry::metabolite_term(biocad_registry);
    let entity_metabolite = biocad_registry::molecule_entity(biocad_registry);

    metacyc <- open.biocyc(metacyc);

    # imports metabolites
    let compartments = biocad_registry |> table("subcellular_compartments");
    let location_link = biocad_registry |> table("subcellular_location");
    let metabolite = biocad_registry |> table("molecule");    

    for(let meta in tqdm(metacyc |> getCompounds())) {
        let formula_str = meta |> BioCyc::formula(meta);
        let dbkeys = meta |> BioCyc::db_links(meta);

        if (length(dbkeys) > 0) {
            str(dbkeys);
            stop();
        }

        meta <- as.list(meta);
        meta$formula <- formula_str;
        meta <- list(
            ID = meta$uniqueId,
            formula = formula_str,
            exact_mass = 0,
            name = meta$commonName,
            IUPACName = meta$commonName,
            description = meta$comment,
            synonym = meta$synonyms,
            xref = list(
                chebi = NULL,
                KEGG = NULL,
                pubchem = NULL,
                HMDB = NULL,
                Wikipedia = NULL,
                lipidmaps = NULL,
                MeSH = NULL,
                MetaCyc = meta$uniqueId,
                foodb = NULL,
                CAS = NULL,
                InChIkey = "-",
                InChI = meta$nonStandardInChI,
                SMILES = meta$SMILES
            )
        );

        str(meta);

        # stop();
        # try({
        #     let compound = as.list(metadata.pugView(meta));
        #     let cid = `PubChem:${compound$ID}`;
        #     let mol = biocad_registry |> find_molecule(compound, cid);

        #     if (is.null(mol)) {
        #         # error while add new metabolite
        #         next;
        #     } else {
        #         biocad_registry |> __push_compound_metadata(compound, mol);
        #     }
        # });
    }
}