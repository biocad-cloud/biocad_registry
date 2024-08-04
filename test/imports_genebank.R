require(biocad_registry);

setwd(@dir);

let biocad_registry = open_registry("root", 123456);
let genebank =read.genbank("./Escherichia coli str. K-12 substr. MG1655.txt");

biocad_registry |> imports_genebank ( genebank);