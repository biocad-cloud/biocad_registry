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
    let offset = 0;
    let kinetic_law = registry |> table("kinetic_law") ;

    for(let page in 1:100000) {
        offset = (page - 1) * page_size;
        page_data = kinetic_law 
            |> where(enzyme_mol = 0)
            |> limit(offset, page_size) 
            |> select()
            ;
        
        print(get_last_sql(kinetic_law ));
        # print(page_data);

        if (length(page_data) == 0) {
            break;
        } else {
            for(let law in tqdm(as.list(page_data,byrow = TRUE))) {
                if (nchar(law$uniprot) > 1) {
                    # link protein molecule
                    let prot = db_xrefs |> where(type = prot_key, db_key = uniprot_key, xref = law$uniprot) |> find();

                    if (!is.null(prot)) {
                        kinetic_law
                        |> where(id = law$id)
                        |> save(enzyme_mol = prot$obj_id)
                        ;
                    }
                }

                let params = JSON::json_decode(law$params);
                let xrefs = JSON::json_decode(law$json_str);
                
                xrefs <- xrefs$xref;

                for(let val in params) {
                    if (val in xrefs) {
                        let xref_ids = xrefs[[val]];

                        if (length(xref_ids) > 0) {
                            let meta = db_xrefs 
                            |> where(type = metab_key, xref in xref_ids) 
                            |> select()
                            ;

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
}