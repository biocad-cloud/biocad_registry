require(GCModeller);

imports "taxonomy_kit" from "metagenomics_kit";

ncbi_tax = Ncbi.taxonomy_tree("E:\\UniProt\\taxdmp");
ncbi_tax = [ncbi_tax]::Taxonomy;

print(names(ncbi_tax));

for(tax in ncbi_tax) {
    str(as.list(tax));
    biocad_registry::put.ncbi_tax(tax);
}