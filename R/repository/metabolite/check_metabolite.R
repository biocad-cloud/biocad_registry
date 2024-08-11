#' check of the metabolite is existsed inside database
#' 
#' @return an integer unique id of the compound that matched with the 
#'   compound xrefs information, NULL will be returns if the molecule 
#'   metabolite data has not been found.
#' 
const check_metabolite = function(biocad_registry, compound) {
    let xrefs = as.list(compound$xref);
    let exact_mass = formula::eval(compound$formula);

    # removes all molecular strucutre data
    xrefs$InChIkey <- NULL;
    xrefs$InChI    <- NULL;
    xrefs$SMILES   <- NULL;
    xrefs$extras   <- NULL;

    xrefs <- unlist(xrefs);
    xrefs <- xrefs[nchar(xrefs) > 0];
    
    if (lenth(xrefs) > 0) {
        compound = biocad_registry 
        |> table("db_xrefs") 
        |> left_join("molecule") 
        |> on(molecule.id = db_xrefs.obj_id) 
        |> where(
            mass between [exact_mass - 1, exact_mass + 1],
            db_xrefs.type in molecules,
            xref in xrefs
        ) 
        |> find("molecule.*")
        ;

        # debug mysql
        stop(biocad_registry |> get_last_sql());
        
        if (is.null(compound)) {
            NULL;
        } else {
            compound$id;
        }
    } else {
        NULL;
    }
}