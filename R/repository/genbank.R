imports "GenBank" from "seqtoolkit";

const imports_genebank = function(biocad_registry, genebank) {
    let term_gene = biocad_registry |> gene_term();
    let genes = genebank |> enumerateFeatures(keys = ["gene","tRNA","ncRNA"]);
}