require(biocad_registry);

let registry = open_registry("xieguigang", 123456, host ="192.168.3.15");

for(taxname in c("Providencia stuartii","Leucobacter luti","Stenotrophomonas acidaminiphila","Brevundimonas diminuta","Alcaligenes faecalis","Comamonas thiooxydans","Comamonas thiooxydans")) {
    query_refseq(registry, taxname);
}

