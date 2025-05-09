#' Check if a metabolite exists in the BioCAD registry database
#' 
#' Performs a database lookup for metabolites using cross-references (xrefs) 
#' and exact mass matching. First attempts direct xref matching, then falls 
#' back to checking chemical synonyms if no direct matches are found.
#'
#' @param biocad_registry A database connection object to the BioCAD registry 
#'        (e.g., MySQL/MariaDB connection through DBI)
#' @param compound A list containing metabolite data with the following elements:
#'        - xref: Named list of cross-references (e.g., KEGG, CHEBI, HMDB)
#'        - formula: Chemical formula string
#'        - name: Common name of metabolite
#'        - synonym: Vector of synonym names
#'
#' @return An integer ID of the matching compound record if found, NULL if no 
#'         match is found in the database. Return priority:
#'         1. Direct xref matches in database
#'         2. Synonym name matches
#'         3. Exact name matches
#' 
#' @details The function:
#'          - Calculates exact mass from chemical formula
#'          - Filters structural identifiers (InChI, SMILES)
#'          - Searches within ±1 Da mass tolerance window
#'          - Uses MD5 hashes of lowercased synonyms for efficient matching
#'          - Prioritizes direct cross-reference matches before falling back 
#'            to name-based matching
const check_metabolite = function(biocad_registry, compound) {
    let xrefs = as.list(compound$xref);
    let exact_mass = formula::eval(compound$formula);
    let molecules = molecule_terms(biocad_registry);

    if (is.null(exact_mass)) {
        # generic compound has no specific formula
        # 
        exact_mass       = 0;
        compound$formula = "";
    }

    # removes all molecular strucutre data
    xrefs$InChIkey <- NULL;
    xrefs$InChI    <- NULL;
    xrefs$SMILES   <- NULL;
    xrefs$extras   <- NULL;

    xrefs <- unlist(xrefs);
    xrefs <- xrefs[nchar(xrefs) > 0];

    if (length(xrefs) > 0) {
        let q = biocad_registry 
        |> table("db_xrefs") 
        |> left_join("molecule") 
        |> on(molecule.id = db_xrefs.obj_id) 
        |> where(
            molecule.mass between [exact_mass - 1, exact_mass + 1],
            db_xrefs.type in molecules,
            xref in xrefs
        ) 
        |> find("molecule.*")
        ;

        # debug mysql
        # str(q);
        # stop(biocad_registry |> get_last_sql());
        
        if (is.null(q)) {
            biocad_registry |> check_metabolite_synonym(compound);
        } else {
            return(q$id);
        }
    } else {
        biocad_registry |> check_metabolite_synonym(compound);
    }
}

#' Internal metabolite lookup by name and synonyms
#'
#' Performs secondary metabolite lookup using common names and synonym hashes 
#' when no direct xref matches are found. Not designed for direct use (called 
#' internally by check_metabolite).
#'
#' @param biocad_registry Database connection object (see check_metabolite)
#' @param compound Compound data list (see check_metabolite)
#'
#' @return An integer ID if name/synonym match found, NULL otherwise. Searches:
#'         1. Exact name match first
#'         2. MD5 hash matches of lowercased synonyms
#' 
#' @note Uses identical mass tolerance window (±1 Da) as check_metabolite.
#'       Synonym matching uses hashed values to protect sensitive data.
const check_metabolite_synonym = function(biocad_registry, compound) {
    let exact_mass = formula::eval(compound$formula);

    if (is.null(exact_mass)) {
        # generic compound has no specific formula
        # 
        exact_mass       = 0;
        compound$formula = "";
    }

    # find by common name
    let q = biocad_registry 
    |> table("molecule") 
    |> where(molecule.mass between [exact_mass - 1, exact_mass + 1], 
        name = compound$name) 
    |> find()
    ;

    if (is.null(q)) {
        let hashset = sapply(compound$synonym, name -> md5(tolower(name)));

        # bind by synonym 
        q = biocad_registry 
        |> table("molecule") 
        |> left_join("synonym") 
        |> on(molecule.id = synonym.obj_id) 
        |> where(molecule.mass between [exact_mass - 1, exact_mass + 1], 
            synonym.hashcode in hashset) 
        |> find("molecule.*")
        ;
    }

    if (is.null(q)) {
        NULL;
    } else {
        return(q$id);
    }
}