require(biocad_registry);
require(Plantea);

imports "registry" from "biocad_registry";
imports "motif_tool" from "TRNtoolkit";

let db = locate_meme_dir("Tae");
let motifs = list.files(db,pattern = "*.meme") 
    |> lapply(file -> read_meme(file)) 
    |> unlist() 
    |> unlist()
    ;

open_registry("xieguigang", 123456, host ="192.168.3.15")
|> imports_planttfdb(motifs, motif_tfinfo("Tae"))
;