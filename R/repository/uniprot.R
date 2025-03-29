imports "uniprot" from "seqtoolkit";
imports "dataset" from "MLkit";
imports "bioseq.fasta" from "seqtoolkit";

require(buffer);

#' Import UniProt proteins into biocad registry database
#' 
#' This helper function imports protein data from UniProt into a biocad registry database, 
#' handling sequence embeddings, cross-references, subcellular locations, and keyword associations.
#'
#' @param biocad_registry A biocad database registry connection object.
#' @param uniprot UniProt protein accession(s). Can be either:
#'                - A character vector of UniProt IDs
#'                - A pipeline enumerator for stream processing
#' @param fast_check Logical. If TRUE, skips proteins already existing in the database 
#'                   using quick verification. Default = FALSE.
#'
#' @return Invisibly returns NULL. Mainly used for its side effects of populating 
#'         the biocad registry database.
#'
#' @details The function performs these main operations:
#' 1. Initializes sequence embedding transformer (SGT) for protein sequences
#' 2. Sets up database terminology references for proteins
#' 3. Processes input proteins with progress tracking where possible
#' 4. For each protein:
#'    - Retrieves sequence, descriptions, locations, and cross-references from UniProt
#'    - Checks existing records if fast_check is enabled
#'    - Creates new molecule entry if not existing
#'    - Adds keyword associations and cross-references (both UniProt and external databases)
#'    - Generates and stores sequence graph embeddings
#'    - Records subcellular localization information
#'
#' @section Database Tables Modified:
#' - molecule: Main protein entity storage
#' - db_xrefs: Cross-references to external databases
#' - molecule_tags: Keyword associations
#' - sequence_graph: Protein sequence embeddings
#' - subcellular_compartments: Cellular localization data
#' - subcellular_location: Protein-compartment relationships
#'
#' @note Requires connection to UniProt web services and proper configuration of 
#'       the biocad registry database schema. The SGT embedding process may be
#'       computationally intensive for large datasets.
#'
#' @examples
#' \dontrun{
#' # Import single protein with progress tracking
#' imports_uniprot(biocad_registry, "P12345")
#' 
#' # Batch import with fast checking
#' imports_uniprot(biocad_registry, c("P12345", "Q67890"), fast_check = TRUE)
#' }
const imports_uniprot = function(biocad_registry, uniprot, fast_check = FALSE) {
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
    let labels = biocad_registry |> table("molecule_tags");
    let uniprot_key = biocad_registry |> vocabulary_id("uniprot", "External Database");
    let protein_graph = biocad_registry |> vocabulary_id("Protein_graph","Embedding", 
        desc =bencode( [sgt]::feature_names)
    );
    let compartments = biocad_registry |> table("subcellular_compartments");
    let location_link = biocad_registry |> table("subcellular_location");
    let seq_graph = biocad_registry |> table("sequence_graph");

    for(let prot in pool) {
        let fa = uniprot::get_sequence(prot);
        let info = uniprot::get_description(prot);
        let loc = uniprot::get_subcellularlocation(prot);
        let xrefs = uniprot::get_xrefs(prot);        
        let keywords = uniprot::get_keywords(prot);
        let uniprot_id = [prot]::accessions;

        # if any uniprot id was found inside database
        # then skip of current protein
        # in fast check mode
        if (fast_check) {
            if (db_xrefs |> check(db_key = uniprot_key, type = term_prot, xref in uniprot_id)) {
                # skip of current exiisted protein record
                # cat(["skip protein ", uniprot_id[1], " by fast check!"]);
                next;
            }
        }

        uniprot_id = uniprot_id[1];
        info = paste(info, sep = "; ");

        let mol = biocad_registry |> table("molecule") |> where(type = term_prot,
            xref_id = uniprot_id) |> find();

        if (is.null(mol)) {
            biocad_registry |> table("molecule") |> add(
                xref_id = uniprot_id,
                name = xrefs$name,
                mass = bioseq.fasta::mass(fa, type="Protein"),
                type = term_prot,
                formula = seq_formula(fa, type="Protein"),
                parent = 0,
                note = info
            );

            mol = biocad_registry |> table("molecule") |> where(type = term_prot,
                xref_id = uniprot_id) |> find();
        }

        if (is.null(mol)) {
            # error while add new metabolite
            next;
        } else {
            # add keyword association
            # [id,keyword]
            for(let word in as.list(keywords,byrow=TRUE)) {
                let word_id = biocad_registry |> vocabulary_id(
                    word$id, "UniProt Keywords",
                    desc = word$keyword
                );

                if (!(labels |> check(tag_id=word_id,molecule_id=mol$id))) {
                    labels |> add(
                        tag_id=word_id,
                        molecule_id=mol$id,
                        description = word$keyword
                    );
                }
            }

            # add all uniprot id into database
            for(let id in uniprot_id) {
                if (!(db_xrefs |> check(obj_id = mol$id,
                    db_key =  uniprot_key,
                    xref = id,
                    type = term_prot ))) {
                        db_xrefs |> add(
                            obj_id = mol$id,
                            db_key = uniprot_key,
                            xref = id,
                            type = term_prot 
                        );
                    }
            }
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

        if (!(seq_graph |> check(molecule_id = mol$id))) {
            let fa_vec = sgt |> fit_embedding([fa]::SequenceData,safe =TRUE);

            fa_vec <- base64(packBuffer(fa_vec)|> zlib_stream());

            seq_graph |> add(
                molecule_id = mol$id,
                sequence = [fa]::SequenceData,
                seq_graph = fa_vec,
                embedding = protein_graph,
                hashcode = md5(tolower([fa]::SequenceData))
            );
        }

        for(loc in as.list(loc, byrow = TRUE)) {
            if (!(compartments |> check(compartment_name = loc$location))) {
                # str(loc);
                compartments |> add(compartment_name = loc$location,
                    topology = trim(loc$topology) || "");
            }

            let compartment = compartments |> where(compartment_name = loc$location) |> find();

            if (!(location_link |> check(compartment_id = compartment$id,
                obj_id = mol$id,
                entity = entity_prot))) {
                    location_link |> add(
                        compartment_id = compartment$id,
                        obj_id = mol$id,
                        entity = entity_prot
                    );
                }
        }
    }

    invisible(NULL);
}