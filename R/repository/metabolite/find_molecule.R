#' Find or Register a Target Metabolite in BioCAD Registry
#' 
#' This function searches for a metabolite in the BioCAD registry using cross-referencing IDs. 
#' If not found, it attempts to register a new metabolite entry with provided metadata.
#'
#' @param biocad_registry A BioCAD registry connection object representing the database interface.
#' @param meta A list containing metabolite metadata. Expected elements:
#' \itemize{
#'   \item \code{name}: (string) Common name of the metabolite
#'   \item \code{formula}: (string) Chemical formula
#'   \item \code{description}: (string) Optional descriptive text
#' }
#' @param xref_id (string) Cross-referencing identifier used for database lookup
#'
#' @return Returns a metabolite object if found/registered successfully. Returns \code{NULL} if:
#' \itemize{
#'   \item Metabolite not found and failed to register
#'   \item Invalid input parameters
#'   \item Database errors occur
#' }
#' 
#' @section Workflow:
#' 1. Primary lookup: Checks existing metabolites using \code{check_metabolite()}
#' 2. If not found:
#'    a. Creates metabolite taxonomy term
#'    b. Attempts lookup with taxonomy term + xref_id
#'    c. If still not found, registers new metabolite with:
#'       - Calculated exact mass (handles negative mass as 0)
#'       - User-provided metadata
#'    d. Verifies registration by immediate post-creation lookup
#' 3. Returns either:
#'    - Existing metabolite object
#'    - Newly registered metabolite object
#'    - NULL on failure
#'
#' @section Side Effects:
#' - May create new entries in the BioCAD registry database
#' - Modifies the metabolite taxonomy index
#'
#' @examples
#' \dontrun{
#' result <- find_molecule(
#'     biocad_registry = my_registry,
#'     meta = list(
#'         name = "ATP",
#'         formula = "C10H16N5O13P3",
#'         description = "Adenosine triphosphate"
#'     ),
#'     xref_id = "CHEBI:15422"
#' )
#' }
#'
#' @seealso \code{\link{check_metabolite}} for primary lookup logic
#' @export
const find_molecule = function(biocad_registry, meta, xref_id) {
    # check database via xrefs at first
    let mol_id = check_metabolite(biocad_registry, compound = meta);

    if (is.null(mol_id)) {
        # current compound is not found
        # add new
        let term_metabolite = biocad_registry::metabolite_term(biocad_registry);
        let mol = metabolite |> where(type = term_metabolite ,
            xref_id = xref_id) |> find();
        let exact_mass = formula::eval(meta$formula);

        if (exact_mass < 0) {
            exact_mass = 0;
        }

        if (is.null(mol)) {
            metabolite |> add(
                xref_id = xref_id,
                name = meta$name,
                mass = exact_mass,
                type =  term_metabolite,
                formula = meta$formula,
                parent = 0,
                note = meta$description  
            );

            mol = metabolite |> where(type =term_metabolite,
                xref_id = xref_id ) |> find();
        }

        if (is.null(mol)) {
            # error while add new metabolite
            NULL;
        } else {
            mol;
        }
    } else {
        metabolite 
        |> where(id = mol_id) 
        |> find()
        ;
    }
}