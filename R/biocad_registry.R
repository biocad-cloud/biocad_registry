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

const molecule_terms = function(biocad_registry) {
    sapply(["Nucleic Acid" "RNA" "Polypeptide" "Metabolite"], term -> biocad_registry |> vocabulary_id(term,"Molecule Type"));
}

const molecule_entity = function(biocad_registry) {
    biocad_registry |> vocabulary_id("Molecule","Entity Type");
}

const reaction_model = function(biocad_registry) {
    biocad_registry |> vocabulary_id("Biological Reaction","Biological Process");
}

const enzyme_regulation = function(biocad_registry) {
    biocad_registry |> vocabulary_id("Enzymatic Catalysis","Regulation Type");
}