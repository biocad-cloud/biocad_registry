require(biocad_registry);
require(Plantea);

imports "registry" from "biocad_registry";
imports "motif_tool" from "TRNtoolkit";

let plant_code = "Ccl";
let db = locate_meme_dir(plant_code);
let motifs = list.files(db,pattern = "*.meme") 
    |> lapply(file -> read_meme(file)) 
    |> unlist() 
    |> unlist()
    ;

open_registry("xieguigang", 123456, host ="192.168.3.15")
|> imports_planttfdb(motifs, motif_tfseq(plant_code), taxname = "Citrus x clementina")
;