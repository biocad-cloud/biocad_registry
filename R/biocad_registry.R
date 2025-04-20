#' get vocabulary term id of ``Nucleic Acid``
const gene_term = function(biocad_registry) {
    biocad_registry |> vocabulary_id("Nucleic Acid","Molecule Type");
}

#' get vocabulary term id of ``RNA``
const rna_term = function(biocad_registry) {
    biocad_registry |> vocabulary_id("RNA","Molecule Type");
}

#' get vocabulary term id of ``Polypeptide``
const protein_term = function(biocad_registry) {
    biocad_registry |> vocabulary_id("Polypeptide","Molecule Type");
}

#' get vocabulary term id of ``Metabolite``
const metabolite_term = function(biocad_registry) {
    biocad_registry |> vocabulary_id("Metabolite","Molecule Type");
}

#' get vocabulary term id collection of multiple molecule types
#' 
#' @details "Nucleic Acid" "RNA" "Polypeptide" "Metabolite"
#' 
const molecule_terms = function(biocad_registry) {
    sapply(["Nucleic Acid" "RNA" "Polypeptide" "Metabolite"], term -> biocad_registry |> vocabulary_id(term,"Molecule Type"));
}

#' the molecule entity term
#' 
#' @details the "Nucleic Acid", "RNA", "Polypeptide", "Metabolite" is belong to the molecule entity object.
const molecule_entity = function(biocad_registry) {
    biocad_registry |> vocabulary_id("Molecule","Entity Type");
}

const reaction_model = function(biocad_registry) {
    biocad_registry |> vocabulary_id("Biological Reaction","Biological Process");
}

const enzyme_regulation = function(biocad_registry) {
    biocad_registry |> vocabulary_id("Enzymatic Catalysis","Regulation Type");
}