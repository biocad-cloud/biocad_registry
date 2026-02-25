require(biocad_registry);

imports "registry" from "biocad_registry";
imports "NCBI" from "annotationKit";

let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");
let asm = genome_assembly_index("D:\datapool\assembly_summary_refseq.txt");

biocad_registry |> import_refseq(asm);