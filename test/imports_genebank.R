require(biocad_registry);

imports "data_imports" from "biocad_registry";
imports "GenBank" from "seqtoolkit";

setwd(@dir);

let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");
let genebank =read.genbank("./Escherichia coli str. K-12 substr. MG1655.txt", repliconTable=TRUE);

for(rep in genbank) {
    biocad_registry |> imports_genebank (rep);
}

