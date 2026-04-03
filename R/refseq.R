const query_refseq = function(registry, taxname) {
    let refs = registry 
    |> table("refseq") 
    |> where(organism_name = taxname, 
            assembly_level in c("Chromosome","Complete Genome")
    ) 
    |> select()
    ;
    let urls = refs$ftp_path;
    let names = basename(urls, withExtensionName =TRUE);

    urls = `${urls}/${names}_genomic.gbff.gz`;

    return(urls);
}