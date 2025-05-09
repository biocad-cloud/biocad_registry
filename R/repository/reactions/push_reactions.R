#' Push a single reaction model into the biocad registry database
#' 
#' @description
#' This function adds or updates a reaction entry in the biocad registry database, 
#' including associated enzymes, database cross-references, and metabolite compounds.
#' 
#' @param biocad_registry An object representing the connection to the biocad registry database.
#' @param reaction A list containing the reaction data to be pushed into the database. 
#'   See "Details" for the required structure.
#' 
#' @details
#' The `reaction` argument must be a list with the following structure:
#' 
#' ```r
#' list(
#'    entry = "unique_id",                 # Unique identifier for the reaction (string)
#'    definition = "name",                  # Reaction name or equation (string)
#'    comment = "description_string",       # Optional description or notes (string)
#'    enzyme = list("ec_number1", ...),     # List of EC numbers (strings) for enzymatic regulation
#'    db_xrefs = list(                      # List of cross-references to external databases
#'       list(name = "dbname", text = "xref_id"), 
#'       ...
#'    ),
#'    compounds = list(                     # List of compounds involved in the reaction
#'       list(
#'          side = "role_string",           # Compound role (e.g., "substrate", "product", "*" for unknown)
#'          compound = list(                # Compound details
#'             entry = "compound_id", 
#'             name = "compound_name", 
#'             formula = "chemical_formula",
#'             factor = stoichiometric_factor  # Numeric (default = 1.0 if missing)
#'          )
#'       ), 
#'       ...
#'    )
#' )
#' ```
#' 
#' - If a reaction with the same `entry` exists, it will be ​**updated**​ with new data.
#' - Enzymes, cross-references, and compounds are linked to the reaction entry. Duplicates are skipped.
#' - Compounds with `side = "*"` are ignored (used for placeholder entries).
#' 
#' @section Side Effects:
#' - Updates the `reaction`, `regulation_graph`, `reaction_graph`, and `db_xrefs` tables in the database.
#' - Adds new entries for missing enzymes, compounds, or cross-references.
#' 
#' @return 
#' Invisibly returns `NULL`. The function's primary purpose is to modify the database.
#' 
#' @examples
#' \dontrun{
#' # Example reaction data
#' reaction_data <- list(
#'   entry = "R00001",
#'   definition = "ATP + H2O -> ADP + Phosphate",
#'   comment = "Hydrolysis of ATP",
#'   enzyme = list("3.6.1.3"),
#'   db_xrefs = list(list(name = "KEGG", text = "R00001")),
#'   compounds = list(
#'     list(
#'       side = "substrate",
#'       compound = list(entry = "C00002", name = "ATP", formula = "C10H16N5O13P3", factor = 1)
#'     ),
#'     list(
#'       side = "product",
#'       compound = list(entry = "C00008", name = "ADP", formula = "C10H15N5O10P2", factor = 1)
#'     )
#'   )
#' )
#' 
#' # Push to database
#' push_reaction(biocad_registry = my_registry, reaction = reaction_data)
#' }
#' 
#' @seealso
#' - Use [link_reaction_metabolites()] to resolve compound IDs after pushing reactions.
#' - See the database schema documentation for table structures.
#' 
const push_reaction = function(biocad_registry, reaction) {
    let reactions = biocad_registry |> table("reaction");
    let graph = biocad_registry |> table("regulation_graph");
    let metabolite_graph = biocad_registry |> table("reaction_graph");
    let xrefs = biocad_registry |> table("db_xrefs");
    let enzyme_term = biocad_registry |> enzyme_regulation();
    let rxn_term = biocad_registry |> reaction_model();  
    let rxn = reaction;

    # check reaction
    let r = reactions |> where(db_xref = rxn$entry) |> find();

    if (is.null(r)) {
        reactions |> add(
            db_xref = rxn$entry,
            name = (rxn$name) || (rxn$definition),
            equation = rxn$definition,
            note = rxn$comment
        );

        r = reactions |> where(db_xref = rxn$entry) |> find();
    } 

    # add ec number for the enzymatic regulation
    for(ec_number in rxn$enzyme) {
        if (!(graph |> check(term = ec_number, role = enzyme_term, reaction_id = r$id))) {
            graph |> add(
                term = ec_number,
                role = enzyme_term,
                reaction_id = r$id
            );
        }
    }

    # add external reference id of current reaction object
    for(db_xref in rxn$db_xrefs) {
        let [name, text] = db_xref;
        let db_key = biocad_registry |> vocabulary_id(name, "External Database");

        if (!(xrefs |> check(obj_id = r$id, db_key = db_key, xref = text, type = rxn_term))) {
            xrefs |> add(
                obj_id = r$id,
                db_key = db_key, 
                xref = text, 
                type = rxn_term
            );
        }
    }

    # how to link the reaction with metabolite molecules
    for(compound in rxn$compounds) {
        if (compound$side != "*") {
            let role_id = biocad_registry |> vocabulary_id(compound$side, "Compound Role");

            compound = compound$compound;

            if (!(metabolite_graph |> check(
                    reaction = r$id,
                    db_xref = compound$entry,
                    role = role_id
                ))) {
                
                let stoichiometry = (compound$factor) || 1.0;

                metabolite_graph |> add(
                    reaction = r$id,
                    # do object linking at later via function
                    # ``link_reaction_metabolites``
                    molecule_id = 0,
                    db_xref = compound$entry,
                    role = role_id,
                    factor = ifelse( is.infinite(stoichiometry),1,stoichiometry) ,
                    note = `${compound$name} (${compound$formula})`
                );
            }
        }
    }
}