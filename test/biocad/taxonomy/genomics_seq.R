require(biocad_registry);

let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");
let nucleotide = biocad_registry |> export_genomics_fasta(parent_taxname = "Bacillales");

print(nucleotide);