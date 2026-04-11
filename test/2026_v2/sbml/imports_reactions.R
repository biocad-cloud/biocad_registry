require(biocad_registry);
require(GCModeller);

imports "SBML" from "biosystem";

let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");
let dir = "D:\datapool\reactions\pathbank_primary_sbml";

for(file in list.files(dir, pattern = "*.sbml")) {
    biocad_registry |> imports_sbml_reactions(read.sbml(file) );
}




