require(biocad_registry);
require(Daisy);

imports "data_exports" from "biocad_registry";

# tool script for update the /opt/libs/metadata.dat library file for daisy workflow
let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");
let metadb_libs = data_exports::export_metabolites(biocad_registry);

write_metadata(metadata, meta = relative_work("metadata.dat"));