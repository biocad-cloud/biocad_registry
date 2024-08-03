imports "uniprot" from "seqtoolkit";
imports "dataset" from "MLkit";
imports "bioseq.fasta" from "seqtoolkit";

#' helper function for imports uniprot proteins
#' 
const imports_uniprot = function(biocad_registry, uniprot) {
    let sgt = SGT(alphabets = bioseq.fasta::chars("Protein"));

    let term_gene = biocad_registry |> vocabulary_id("Nucleic Acid","Molecule Type");
    let term_rna = biocad_registry |> vocabulary_id("RNA","Molecule Type");
    let term_prot = biocad_registry |> vocabulary_id("Polypeptide","Molecule Type");
    let term_metabolite = biocad_registry |> vocabulary_id("Metabolite","Molecule Type");
    let db_id = list();

    for(let prot in tqdm(uniprot)) {
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
        }

        cat("\n\n");
        print(fa_vec);
        print(info);
        print(loc);
        str(xrefs);

        stop();
    }
}