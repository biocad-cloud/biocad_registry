#' Import KEGG compound data into biocad registry
#'
#' This function imports chemical compound data from KEGG database into a biocad 
#' registry system, handling metabolite registration, cross-reference management, 
#' and compound metadata integration.
#'
#' @param biocad_registry A biocad database registry connection object
#' @param kegg KEGG compound data input. Can be either:
#'             - KEGG Compound object collection
#'             - List of parsed KEGG entries
#'             - Pipeline enumerator for stream processing
#'
#' @return Invisibly returns NULL. Mainly used for populating the biocad registry 
#'         with metabolite data and associated cross-references.
#' 
#' @details The function performs these key operations:
#' 1. Initializes database terminology for metabolite entities
#' 2. Processes KEGG compound entries with progress tracking
#' 3. For each compound:
#'    - Extracts database cross-references (ChEBI, PubChem, CAS)
#'    - Builds standardized compound metadata structure
#'    - Checks for existing registry entries
#'    - Updates database with new compounds and related information
#' 4. Maintains cross-database consistency through:
#'    - KEGG-to-external database identifier mapping
#'    - Synonym management
#'    - Structural data preservation
#'
#' @section Data Structure Transformation:
#' Converts KEGG compound objects to biocad registry format with:
#' - Core identifiers (KEGG ID)
#' - Chemical descriptors (formula, common names)
#' - Cross-system references (ChEBI, PubChem, CAS)
#' - Synonym management through common names aggregation
#'
#' @section Database Integration:
#' - molecule: Main metabolite entity storage
#' - Associated tables via __push_compound_metadata (assumed to handle):
#'   - Synonym tables
#'   - Structural data storage
#'   - Cross-reference management
#'
#' @note Important implementation considerations:
#' - Requires properly formatted KEGG compound objects/entries
#' - Exact mass values default to 0 (verify upstream calculation needs)
#' - ChEBI IDs are automatically prefixed with "CHEBI:" 
#' - Skips compounds with existing registry entries
#' - Common names handling:
#'   - First common name used as primary name
#'   - All names aggregated in description/synonym fields
#' - Cross-reference fields accept multiple IDs per database
#'
#' @examples
#' \dontrun{
#' # Import from KEGG API response
#' imports_kegg(biocad_registry, kegg_compounds)
#'
#' # Import from parsed file
#' imports_kegg(biocad_registry, read_kegg("compound.xml"))
#' }
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

const imports_kegg_reaction = function(biocad_registry, kegg) {
    for(let rxn in tqdm(kegg)) {
        try({
            biocad_registry |> push_reaction(reaction = cast_kegg_reaction(rxn));
        });        
    }
}

const cast_kegg_reaction = function(rxn) {
    let model = NULL;
    let left = NULL;
    let right = NULL;

    rxn <- as.list(rxn); 
    model <- rxn$ReactionModel;
    left <- model$Reactants;        
    left <- lapply(left, a -> list(
        side = "substrate", 
        compound = list(entry=a$ID,name=a$ID,formula="", factor=a$Stoichiometry)
    )); 
    right <- model$Products;
    right <- lapply(right, a -> list(
        side = "product",
        compound = list(entry=a$ID,name=a$ID,formula="", factor=a$Stoichiometry)
    ));

    names(left) <- sapply(left, a -> a$compound$entry);
    names(right) <- sapply(right, a -> a$compound$entry);

    list(
        entry = rxn$ID,
        name =  rxn$CommonNames,
        definition = rxn$Definition,
        comment = rxn$CommonNames,
        enzyme = rxn$Enzyme ,
        db_xrefs = NULL,
        compounds = append(left,right)
    );
}