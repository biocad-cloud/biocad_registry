#' link the kinetics law with the enzyme and metabolite substrate molecules
#' 
#' @param registry the mysql database connection to the biocad registry database.
#' 
const link_kinetics = function(registry) {
    let page_size = 1000;
    let page_data = NULL;
    let uniprot_key = registry |> vocabulary_id("UniProtKB/Swiss-Prot", "External Database");
    let prot_key = registry |> vocabulary_id("Polypeptide", "Molecule Type");
    let metab_key = registry |> vocabulary_id("Metabolite", "Molecule Type");
    let db_xrefs = registry |> table("db_xrefs");
    let links = registry |> table("kinetic_substrate");

    for(let page in 1:100000) {
        page_data = registry 
            |> table("kinetic_law") 
            |> limit((page-1)* page_size, page_size) 
            |> select()
            ;
        
        if (length(page_data) == 0) {
            break;
        } else {
            for(let law in tqdm(page_data)) {
                if (nchar(law$uniprot) > 1) {
                    # link protein molecule
                    let prot = db_xrefs |> where(type = prot_key, db_key = uniprot_key, xref = law$uniprot) |> find();

                    if (!is.null(prot)) {
                        registry 
                        |> table("kinetic_law") 
                        |> where(id = law$id)
                        |> save(enzyme_mol = prot$obj_id)
                        ;
                    }
                }

                let params = JSON::json_decode(law$params);
                let xrefs = JSON::json_decode(law$json_str)
                
                xrefs <- xrefs$xref;

                for(let val in params) {
                    if (val in xrefs) {
                        let xref_ids = xrefs[[val]];
                        let meta = db_xrefs |> where(type = metab_key, xref in xref_ids) |> select();

                        for(let ref in meta) {
                            let check = links |> where(kinetic_id = law$id, metabolite_id = ref$obj_id) |> find();

                            # fill missing links
                            if (is.null(check)) {
                                links |> add(
                                    kinetic_id = law$id, 
                                    metabolite_id = ref$obj_id
                                );
                            }
                        }
                    }
                }
            }
        }
    }
}