imports "BioCyc" from "annotationKit";

#' helper function for imports the metacyc molecules and reactions
#' 
const imports_metacyc = function(biocad_registry, metacyc) {
    metacyc <- open.biocyc(metacyc);

    biocad_registry |> load_biocyc_reactions(metacyc);
    # biocad_registry |> load_biocyc_compounds(metacyc);
}

const load_biocyc_reactions = function(biocad_registry, metacyc) {
    for(let rxn in tqdm(metacyc |> getReactions())) {
        let equation_string = rxn |> BioCyc::formula(rxn);
        let ec = [rxn]::ec_number;
        let left = lapply([rxn]::left, function(c) {
            list(side = "substrate", compound = list(
                entry = [c]::ID ,
                name = [c]::ID,
                formula = [c]::ID, 
                factor = as.numeric([c]::Stoichiometry)
            ));
        });
        let right = lapply( [rxn]::right, function(c) {
            list(side = "product", compound = list(
                entry = [c]::ID ,
                name = [c]::ID,
                formula = [c]::ID, 
                factor = as.numeric([c]::Stoichiometry)
            ));
        });

        rxn <- as.list(rxn);
        rxn <- list(
            entry = rxn$uniqueId,
            definition = equation_string,
            comment = rxn$comment,
            enzyme = [ec]::ECNumberString,
            db_xrefs = [],
            compounds = append(left,right)
        );
        
        biocad_registry |> push_reaction(reaction = rxn);
    }
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

        let mol = biocad_registry |> find_molecule(meta, meta$ID);

        if (is.null(mol)) {
            # error while add new metabolite
            next;
        } else {
            biocad_registry |> __push_compound_metadata(meta, mol);
        }
    }
}