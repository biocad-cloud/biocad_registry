const gene_term = function(biocad_registry) {
    biocad_registry |> vocabulary_id("Nucleic Acid","Molecule Type");
}

const rna_term = function(biocad_registry) {
    biocad_registry |> vocabulary_id("RNA","Molecule Type");
}

const protein_term = function(biocad_registry) {
    biocad_registry |> vocabulary_id("Polypeptide","Molecule Type");
}

const metabolite_term = function(biocad_registry) {
    biocad_registry |> vocabulary_id("Metabolite","Molecule Type");
}