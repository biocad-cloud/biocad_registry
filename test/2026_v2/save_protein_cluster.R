require(biocad_registry);

imports "annotation.workflow" from "seqtoolkit";
imports "models" from "biocad_registry";

let alignments = read_m8(file = "K:\\protein_clusters.tsv", stream = TRUE); 
# let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");

# biocad_registry |> imports_diamond(blastp = alignments, batch_size = 100000,  check_unique = TRUE);
diamond_transaction(blastp = alignments, dir = "K:\diamonds_small" ,batch_size  = 500000);