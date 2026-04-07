require(biocad_registry);

imports "annotation.workflow" from "seqtoolkit";
imports "models" from "biocad_registry";
imports "hmmer" from "seqtoolkit";

let registry = open_registry("xieguigang", 123456, host ="192.168.3.15");

registry |> make_protein_clusters(eval_cutoff = 1e-10);
# registry |> link_prot_ko(hmmer::parse_kofamscan("K:\ko_result.tsv"));