require(biocad_registry);

setwd(@dir);

let biocad_registry = open_registry("root", 123456, host ="127.0.0.1");
let genebank =read.genbank("./Escherichia coli str. K-12 substr. MG1655.txt");

print(unique(genebank |> featureKeys()));

biocad_registry |> imports_genebank ( genebank);