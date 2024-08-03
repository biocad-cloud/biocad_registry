imports "uniprot" from "seqtoolkit";
imports "dataset" from "MLkit";
imports "bioseq.fasta" from "seqtoolkit";

require(buffer);

#' helper function for imports uniprot proteins
#' 
const imports_uniprot = function(biocad_registry, uniprot) {
    let sgt = SGT(alphabets = bioseq.fasta::chars("Protein"));
    let term_prot = biocad_registry::protein_term(biocad_registry);
    let entity_prot = biocad_registry::molecule_entity(biocad_registry);
    let pool = {
        if (is.array(uniprot)) {
            tqdm(uniprot);
        } else {
            # is pipeline enumerator
            # tqdm can not be used
            uniprot;
        }
    }
    let db_xrefs = biocad_registry |> table("db_xrefs");
    let protein_graph = biocad_registry |> vocabulary_id("protein_graph","Embedding");
    let compartments = biocad_registry |> table("subcellular_compartments");
    let location_link = biocad_registry |> table("subcellular_location");

    for(let prot in pool) {
        let fa = uniprot::get_sequence(prot);
        let info = uniprot::get_description(prot);
        let loc = uniprot::get_subcellularlocation(prot);
        let xrefs = uniprot::get_xrefs(prot);
        let fa_vec = sgt |> fit_embedding([fa]::SequenceData);

        info = paste(info, sep = "; ");

        let mol = biocad_registry |> table("molecule") |> where(type = term_prot,
            name = xrefs$name) |> find();

        if (is.null(mol)) {
            biocad_registry |> table("molecule") |> add(
                name = xrefs$name,
                mass = bioseq.fasta::mass(fa),
                type = term_prot,
                formula = "prot",
                parent = 0,
                note = info
            );

            mol = biocad_registry |> table("molecule") |> where(type = term_prot,
                name = xrefs$name) |> find();
        }

        xrefs = xrefs$xrefs;

        for(dbname in names(xrefs)) {
            let idlist = xrefs[[dbname]];
            let db_key = biocad_registry |> vocabulary_id(dbname, "External Database");

            for(id in idlist) {
                if (!(db_xrefs |> check(obj_id = mol$id,
                    db_key = db_key,
                    xref = id,
                    type = term_prot ))) {
                        db_xrefs |> add(
                            obj_id = mol$id,
                            db_key = db_key,
                            xref = id,
                            type = term_prot 
                        );
                    }
            }
        }

        fa_vec = base64(packBuffer(fa_vec)|> zlib_stream());

        if (!(biocad_registry |> table("sequence_graph") |> check("molecule_id"))) {
            biocad_registry |> table("sequence_graph") |> add(
                molecule_id = mol$id,
                sequence = [fa]::SequenceData,
                seq_graph = fa_vec,
                embedding = protein_graph
            );
        }

        for(loc in as.list(loc, byrow = TRUE)) {
            if (!(compartments |> check(compartment_name = loc$location))) {
                compartments |> add(compartment_name = loc$location,
                    topology = loc$topology);
            }

            let compartment = compartments |> where(compartment_name = loc$location) |> find();

            location_link |> add(
                compartment_id = compartment$id,
                obj_id = mol$id,
                entity = entity_prot
            );
        }

        cat("\n\n");
        print(fa_vec);
        print(info);
        print(loc);
        str(xrefs);

        stop();
    }
}