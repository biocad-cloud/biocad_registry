imports "GenBank" from "seqtoolkit";

const imports_genebank = function(biocad_registry, genebank) {
    let term_gene = biocad_registry |> gene_term();
    let genes = genebank |> enumerateFeatures(keys = ["CDS","tRNA","ncRNA","rRNA"]);
    let gene_pool =  biocad_registry |> table("molecule");
    let sgt = SGT(alphabets = bioseq.fasta::chars("DNA"));
    let db_xrefs = biocad_registry |> table("db_xrefs");
    let Nucleotide_graph = biocad_registry |> vocabulary_id("Nucleotide_graph","Embedding", 
        desc =bencode( [sgt]::feature_names)
    );
    let seq_graph = biocad_registry |> table("sequence_graph");

    for(gene in tqdm(genes)) {
        let nt_seq = as.fasta(gene);

        gene <- featureMeta(gene );

        let note_str = gene$product;
        let locus_tag = gene$locus_tag;
        let gene_synonym = strsplit(gene$gene_synonym, ";\s*"); 
        let db_xref =   tagvalue(gene$db_xref ,
                delimiter = ":",
                trim_value = TRUE,
                as_list = TRUE,
                union_list = TRUE);
        let fa_vec = sgt |> fit_embedding([nt_seq]::SequenceData);

        gene <- gene$gene;

        let mol = gene_pool   |> where(type = term_gene ,
            xref_id =locus_tag) |> find();

        if (is.null(mol)) {
            gene_pool |> add(
                xref_id = locus_tag,
                name = gene,
                mass = bioseq.fasta::mass(nt_seq, type="DNA"),
                type =  term_gene,
                formula = seq_formula(nt_seq, type="DNA"),
                parent = 0,
                note = note_str
            );

            mol =gene_pool  |> where(type =term_gene,
                xref_id = locus_tag ) |> find();
        }

        if (is.null(mol)) {
            # error while add new metabolite
            next;
        }

        fa_vec = base64(packBuffer(fa_vec)|> zlib_stream());

        if (!(seq_graph |> check(molecule_id = mol$id))) {
            seq_graph |> add(
                molecule_id = mol$id,
                sequence = [nt_seq]::SequenceData,
                seq_graph = fa_vec,
                embedding = Nucleotide_graph
            );
        }

        for(dbname in names(db_xref)) {
            let idlist = db_xref[[dbname]];
            let db_key = biocad_registry |> vocabulary_id(dbname, "External Database");

            for(id in idlist) {
                if (!(db_xrefs |> check(obj_id = mol$id,
                    db_key = db_key,
                    xref = id,
                    type = term_gene ))) {
                        db_xrefs |> add(
                            obj_id = mol$id,
                            db_key = db_key,
                            xref = id,
                            type = term_gene
                        );
                    }
            }
        }
    }
}