require(GCModeller);
require(biocad_registry);

imports "file" from "gokit";

go = read.go_obo("E:\UniProt\go.obo");
terms = [go]::terms;

for(term in terms) {
    term = as.list(term);

    biocad_registry::put.go_term(term);

    str(term);
    stop();
}