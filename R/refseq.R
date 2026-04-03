const query_refseq = function(registry, taxname) {
    let refs = registry 
    |> table("refseq") 
    |> where(organism_name = taxname, 
            assembly_level in c("Chromosome","Complete Genome")
    ) 
    |> select()
    ;

    print(refs);
}