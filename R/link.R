#' Get molecule id via a specific database cross reference link id
#' 
#' @return an integer id of the target molecule, NULL when object not found.
#' 
const get_molecule_by_dbxref = function(registry, db_xref, dbname = NULL) {
    let xrefs = registry |> table("db_xrefs");
    let molecules = molecule_terms(registry);

    xrefs = {
        if (nchar(dbname) > 0) {
            xrefs = xrefs |> where(
                db_key = registry |> vocabulary_id(dbname, "External Database"), 
                xref = db_xref, 
                type in molecules
            );
        } else {
            xrefs = xrefs |> where(
                xref = db_xref, 
                type in molecules
            );
        }
    }

    if (is.null(xrefs = xrefs |> find())) {
        NULL;
    } else {
        xrefs$obj_id;
    }
}