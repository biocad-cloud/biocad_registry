require(biocad_registry);

imports "annotation.workflow" from "seqtoolkit";
imports "models" from "biocad_registry";
imports "hmmer" from "seqtoolkit";

# open_registry("xieguigang", 123456, host ="192.168.3.15")
# |> make_protein_clusters()
# ;

open_registry("xieguigang", 123456, host ="192.168.3.15") 
|> link_prot_ko(hmmer::parse_kofamscan("M:\\KEGG\\ko.tsv"))
;