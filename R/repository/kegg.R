#' imports kegg compound into data repository
#' 
#' @param kegg a kegg compound collection
#' 
const imports_kegg = function(biocad_registry, kegg) {
    let term_metabolite = biocad_registry::metabolite_term(biocad_registry);
    let entity_metabolite = biocad_registry::molecule_entity(biocad_registry);
    let metabolite = biocad_registry |> table("molecule");    

    for(let cpd in tqdm(kegg)) {
        cpd <- as.list(cpd);
        cpd <- list(
            ID = meta$uniqueId,
            formula = formula_str,
            exact_mass = 0,
            name = ifelse(nchar(meta$commonName) > 0, meta$commonName, meta$uniqueId),
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

        let mol = biocad_registry |> find_molecule(cpd, cpd$ID);

        if (is.null(mol)) {
            # error while add new metabolite
            next;
        } else {
            biocad_registry |> __push_compound_metadata(cpd, mol);
        }
    }
}