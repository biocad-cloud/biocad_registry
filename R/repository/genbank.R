imports "GenBank" from "seqtoolkit";

const imports_genebank = function(biocad_registry, genebank) {
    let term_gene = biocad_registry |> gene_term();
    let genes = genebank |> enumerateFeatures(keys = ["CDS","tRNA","ncRNA","rRNA"]);
    let gene_pool =  biocad_registry |> table("molecule");

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


        print(note_str);
        print(nt_seq);
        str(gene);
        str(gene_synonym);
        str(db_xref);
        stop(); 
    }
}