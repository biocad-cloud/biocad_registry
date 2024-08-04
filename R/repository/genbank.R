imports "GenBank" from "seqtoolkit";

const imports_genebank = function(biocad_registry, genebank) {
    let term_gene = biocad_registry |> gene_term();
    let genes = genebank |> enumerateFeatures(keys = ["gene","tRNA","ncRNA","rRNA"]);

    for(gene in tqdm(genes)) {
        let nt_seq = as.fasta(gene);

        gene <- featureMeta(gene );

        let locus_tag = gene$locus_tag;
        let gene_synonym = strsplit(gene$gene_synonym, ";\s*"); 
        let db_xref =   tagvalue(gene$db_xref ,
                delimiter = ":",
                trim_value = TRUE,
                as_list = TRUE,
                union_list = TRUE);
        
        gene <- gene$gene;

        print(nt_seq);
        str(gene);
        str(gene_synonym);
        str(db_xref);
        stop(); 
    }
}