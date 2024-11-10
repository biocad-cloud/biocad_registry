imports "BioCyc" from "annotationKit";

#' helper function for imports the metacyc molecules and reactions
#' 
const imports_metacyc = function(biocad_registry, metacyc) {
    metacyc <- open.biocyc(metacyc);

    biocad_registry |> load_biocyc_reactions(metacyc);
    biocad_registry |> load_biocyc_compounds(metacyc);
}

const load_biocyc_reactions = function(biocad_registry, metacyc) {

}

#' load and imports compounds from the metacyc database
#'
const load_biocyc_compounds = function(biocad_registry, metacyc) {
    let term_metabolite   = biocad_registry::metabolite_term(biocad_registry);
    let entity_metabolite = biocad_registry::molecule_entity(biocad_registry);
    # imports metabolites
    let compartments = biocad_registry |> table("subcellular_compartments");
    let location_link = biocad_registry |> table("subcellular_location");
    let metabolite = biocad_registry |> table("molecule");    

    for(let meta in tqdm(metacyc |> getCompounds())) {
        let formula_str = meta |> BioCyc::formula(meta);
        let dbkeys = meta |> BioCyc::db_links(meta);

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
                chebi = {
                    if (length(dbkeys$chebi) > 0) {
                        `CHEBI:${dbkeys$chebi}`;
                    } else {
                        NULL;
                    }
                },
                KEGG = dbkeys$kegg,
                pubchem = dbkeys$pubchem,
                HMDB = dbkeys$hmdb,
                Wikipedia = dbkeys$wikipedia,
                lipidmaps = dbkeys$lipidmaps,
                DrugBank = dbkeys$drugbank,
                MeSH = dbkeys$mesh,
                MetaCyc = meta$uniqueId,
                foodb = dbkeys$foodb,
                CAS = dbkeys$cas,
                InChIkey = (meta$InChIKey) || "-",
                InChI = (meta$InChI) || (meta$nonStandardInChI),
                SMILES = meta$SMILES,
                METANETX = dbkeys$metanetx,
                refmet = dbkeys$refmet,
                Metabolights = dbkeys$metabolights,
                Bigg = dbkeys$bigg
            )
        );

        let mol = biocad_registry |> find_molecule(meta, meta$ID);

        if (is.null(mol)) {
            # error while add new metabolite
            next;
        } else {
            biocad_registry |> __push_compound_metadata(meta, mol);
        }
    }
}