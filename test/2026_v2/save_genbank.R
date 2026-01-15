require(biocad_registry);
require(GCModeller);

imports "registry" from "biocad_registry";
imports "GenBank" from "seqtoolkit";

let registry = open_registry("xieguigang", 123456, host ="192.168.3.15");
let repo = list.files("D:\datapool\regprecise_genbank\genomes",  pattern = "*.gbff");
let genbank_asm = GenBank::load_genbanks(repo);

# registry |> save_genbank(genbank_asm);
registry |> make_genbank_dbxrefs(genbank_asm);