#' Check if metabolite exists in database
#' 
#' Searches for a metabolite in the BioCAD registry database by cross-references 
#' (xrefs) and exact mass. If no direct xref matches are found, falls back to
#' checking common names and synonyms. Returns database ID if found.
#'
#' @param biocad_registry A database connection object representing the BioCAD 
#'        registry database.
#' @param compound A list containing metabolite information with the following
#'        elements:
#'        \itemize{
#'          \item{xref - List of cross-references (database identifiers)}
#'          \item{formula - Chemical formula string}
#'          \item{name - Common name of the compound}
#'          \item{synonym - Vector of alternative names/synonyms}
#'        }
#'
#' @return An integer representing the unique database ID of the matched 
#'         compound. Returns \code{NULL} if no matching metabolite is found 
#'         in the database.
#'
#' @details The function performs the following steps:
#' \enumerate{
#'   \item Extracts and validates chemical formula (calculates exact mass)
#'   \item Queries database using cross-reference identifiers (xrefs)
#'   \item Performs mass tolerance search (±1 Da)
#'   \item If xref search fails, calls \code{check_metabolite_synonym} to check
#'         by name and synonyms
#' }
#' 
#' @examples
#' \dontrun{
#' # Assume biocad_registry is an established database connection
#' compound <- list(
#'   name = "glucose",
#'   formula = "C6H12O6",
#'   xref = list(KEGG = "C00031", CHEBI = "CHEBI:4167"),
#'   synonym = c("dextrose", "blood sugar")
#' )
#' check_metabolite(biocad_registry, compound)
#' }
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

#' Internal metabolite synonym checker
#' 
#' Helper function that searches for metabolites by name and synonyms when
#' direct xref matching fails. Uses exact mass and MD5 hashing of synonyms
#' for efficient lookup.
#'
#' @param biocad_registry A database connection object for BioCAD registry
#' @param compound Compound list (see \code{check_metabolite} for structure)
#'
#' @return An integer database ID if match found by name/synonym, otherwise 
#'         \code{NULL}.
#'
#' @details Operates in two phases:
#' \enumerate{
#'   \item Direct name match with mass tolerance
#'   \item MD5 hash-based synonym search if name match fails
#' }
#' @note This function is not intended to be called directly - use 
#'       \code{check_metabolite} instead.
#'
#' @keywords internal
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