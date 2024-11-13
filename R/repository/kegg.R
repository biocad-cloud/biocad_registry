#' imports kegg compound into data repository
#' 
#' @param kegg a kegg compound collection
#' 
const imports_kegg = function(biocad_registry, kegg) {
    let term_metabolite = biocad_registry::metabolite_term(biocad_registry);
    let entity_metabolite = biocad_registry::molecule_entity(biocad_registry);
    let metabolite = biocad_registry |> table("molecule");    

    for(let cpd in tqdm(kegg)) {
        let dblinks = [cpd]::DbLinks;
    
        cpd <- as.list(cpd);

        if (length(dblinks) > 0) {
            dblinks <- data.frame(
                dbname = [dblinks]::DBName,
                db_xref = [dblinks]::entry 
            );
            dblinks <- dblinks 
                |> groupBy("dbname") 
                |> lapply(a -> unique(a$db_xref))
                ; 
        } else {
            dblinks <- NULL;
        }

        cpd <- list(
            ID = cpd$entry,
            formula = cpd$formula,
            exact_mass = 0,
            name = ifelse(length(cpd$commonNames) == 0, cpd$entry, .Internal::first(cpd$commonNames)),
            IUPACName = cpd$commonNames,
            description = paste(cpd$commonNames, sep = "; "),
            synonym = cpd$commonNames,
            xref = list(
                chebi = {
                    if (length(dblinks$ChEBI) > 0) {
                        `CHEBI:${dblinks$ChEBI}`;
                    } else {
                        NULL;
                    }
                },
                KEGG = cpd$entry,
                pubchem = dblinks$PubChem,               
                CAS = dblinks$CAS
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