#' check of the metabolite is existsed inside database
#' 
#' @return an integer unique id of the compound that matched with the 
#'   compound xrefs information, NULL will be returns if the molecule 
#'   metabolite data has not been found.
#' 
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
        compound = biocad_registry 
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
        # str(compound);
        # stop(biocad_registry |> get_last_sql());
        
        if (is.null(compound)) {
            NULL;
        } else {
            compound$id;
        }
    } else {
        NULL;
    }
}