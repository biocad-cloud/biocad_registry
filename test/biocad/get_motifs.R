imports "package_utils" from "devkit";
imports ["jQuery", "Html", "http"] from "webKit";

package_utils::attach(`${@dir}/../../`);

motifs = biocad_registry::get_motif_sites(family = "PsrA"); 

print(motifs);