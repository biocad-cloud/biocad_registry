require(GCModeller);

imports "bioseq.fasta" from "seqtoolkit";
imports "proteinKit" from "seqtoolkit";

let enzymes = read.fasta(relative_work("enzymes.fasta"));
let model = enzymes |> take(100) |> enzyme_builder(kmer = 3);
let predicts = model |> predict_sequence(ec_number = [
    "2.1.1.33",
    "3.2.1.23",
    "2.1.3.3",
    "1.1.1.37",
    "1.1.1.205",
    "1.1.1.85",
    "2.7.4.7",
    "2.7.1.49",
    "3.1.11.-",
    "2.1.3.15"
]);

write.fasta(predicts, file = relative_work("predict.fasta"));
