imports "uniprot" from "seqtoolkit";
imports "dataset" from "MLkit";
imports "bioseq.fasta" from "seqtoolkit";

#' helper function for imports uniprot proteins
#' 
const imports_uniprot = function(biocad_registry, uniprot) {
    let sgt = SGT(alphabets = bioseq.fasta::chars("Protein"));
    let term_prot = biocad_registry::protein_term(biocad_registry);
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
                db_xrefs |> add(
                    obj_id = mol$id,
                    db_key = db_key,
                    xref = id,
                    type = term_prot 
                );
            }
        }

        cat("\n\n");
        print(fa_vec);
        print(info);
        print(loc);
        str(xrefs);

        stop();
    }
}