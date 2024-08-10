#' save compound metadata
#' 
#' @param compound the compound metadata
#' @param mol the molecule record about this compound inside current biocad_registry. 
#'    a ``id`` data property is required inside this molecule record.
#' 
const __push_compound_metadata = function(biocad_registry, compound, mol) {
    let seq_graph = biocad_registry |> table("sequence_graph");
    let db_xrefs  = biocad_registry |> table("db_xrefs");
    let xrefs     = compound$xref;
    let smiles    = gsub(xrefs$SMILES, "%",""); 
    let term_metabolite = biocad_registry::metabolite_term(biocad_registry);

    # removes all molecular strucutre data
    xrefs$InChIkey <- NULL;
    xrefs$InChI    <- NULL;
    xrefs$SMILES   <- NULL;
    xrefs$extras   <- NULL;
    
    let met_struct = SMILES::parse(trim(smiles, '" '), strict = FALSE);

    # null means parser error
    if (!is.null(met_struct)) {
        let atoms_vec = SMILES::atoms(met_struct);

        atoms_vec = atoms_vec 
        |> groupBy("group") 
        |> lapply(grp -> sum(grp$links))
        ;

        let embedding = bencode( names(atoms_vec));
        atoms_vec = as.numeric(unlist(atoms_vec));
        atoms_vec = base64(packBuffer(atoms_vec ) |> zlib_stream());

        if (!(seq_graph |> check(molecule_id = mol$id))) {
            seq_graph |> add(
                molecule_id = mol$id,
                sequence    = smiles,
                seq_graph   = atoms_vec,
                embedding   = embedding
            );
        }
    }

    for(dbname in names(xrefs)) {
        let idlist = xrefs[[dbname]];
        let db_key = biocad_registry |> vocabulary_id(dbname, "External Database");

        if (length(idlist) > 0) {
            for(id in idlist) {
                if (nchar(id) > 0) {
                    if (!(db_xrefs |> check(obj_id = mol$id,
                        db_key = db_key,
                        xref = id,
                        type = term_metabolite ))) {
                            db_xrefs |> add(
                                obj_id = mol$id,
                                db_key = db_key,
                                xref = id,
                                type =term_metabolite
                            );
                        }
                }
            }
        }
    }
}