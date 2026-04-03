require(biocad_registry);

let registry = open_registry("xieguigang", 123456, host ="192.168.3.15");
let urls = NULL;

for(taxname in c("Providencia stuartii","Leucobacter luti","Stenotrophomonas acidaminiphila","Brevundimonas diminuta","Alcaligenes faecalis","Comamonas thiooxydans","Comamonas thiooxydans")) {
    urls = append(urls, query_refseq(registry, taxname));
}

writeLines(urls, con = relative_work( "urls.txt"));

