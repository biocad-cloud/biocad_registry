const get_molecule_by_dbxref = function(registry, db_xref, dbname = NULL) {
    if (nchar(dbname) > 0) {
        let db_key = registry |> vocabulary_id(dbname, "External Database");
        let xrefs = registry |> table("db_xrefs") |> where(
            db_key = db_key, xref = db_xref, type in molecule_terms(registry)
        ) |> find();

        xrefs$obj_id;
    } else {
        let xrefs = registry |> table("db_xrefs") |> where(
            xref = db_xref, type in molecule_terms(registry)
        ) |> find();

        xrefs$obj_id;
    }
}