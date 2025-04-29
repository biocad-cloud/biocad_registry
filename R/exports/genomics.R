const export_genomics_fasta = function(biocad_registry, parent_taxname) {
    let taxnode = biocad_registry |> get_taxinfo(parent_taxname);
    let tax_list = biocad_registry |> child_list(tax_id = [taxnode]::ncbi_taxid,
                                direct_list = FALSE);

    print("get a collection of the child taxonomy id:");
    print(tax_list);

    let genomics_seq = biocad_registry 
    |> table("genomics") 
    |> where(ncbi_taxid in tax_list) 
    |> select(["db_xref","ncbi_taxid","biom_string","def","length","nt"])
    ;

    return(genomics_seq);
}