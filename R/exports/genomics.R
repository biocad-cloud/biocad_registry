const export_genomics_fasta = function(biocad_registry, parent_taxname, fasta = TRUE) {
    let taxnode = biocad_registry |> get_taxinfo(parent_taxname);
    let tax_list = biocad_registry |> child_list(tax_id = [taxnode]::ncbi_taxid,
                                direct_list = FALSE);
    let multiple_blocks = FALSE;

    if (length(tax_list) > 1000) {
        # split into multiple blocks
        multiple_blocks = TRUE;
        tax_list = tax_list |> split(size = 1000);
    }

    print("get a collection of the child taxonomy id:");

    if (multiple_blocks) {
        str(tax_list);
    } else {
        print(tax_list);
    }

    let genomics_seq = {
        if (multiple_blocks) {
            let pull = NULL;

            for(let block in tqdm(tax_list)) {
                block = biocad_registry 
                |> table("genomics") 
                |> where(ncbi_taxid in block) 
                |> select(["db_xref","ncbi_taxid","biom_string","def","length","nt"])
                ;
                pull = rbind(pull, block);

                invisible(NULL);
            }

            pull;
        } else {
            biocad_registry 
            |> table("genomics") 
            |> where(ncbi_taxid in tax_list) 
            |> select(["db_xref","ncbi_taxid","biom_string","def","length","nt"])
            ;
        }
    }

    if (fasta) {
        imports "bioseq.fasta" from "seqtoolkit";

        genomics_seq[,"ncbi_taxid"] = `ncbi_taxid:${genomics_seq$ncbi_taxid}`;
        genomics_seq[,"length"] = `length=${genomics_seq$length}`;
        genomics_seq = as.list(genomics_seq,byrow = TRUE);
        genomics_seq = sapply(genomics_seq, g -> bioseq.fasta::fasta(g$nt, attrs = [g$db_xref,g$ncbi_taxid,g$biom_string,g$length,g$def]));
    } 

    return(genomics_seq);
}