require(biocad_registry);
require(GCModeller);
require(mzkit);

imports "massbank" from "mzkit";
imports "setup" from "biocad_registry";

let registry = open_registry("root","123456");
let dataset = read.SDF(file = "G:\biocad_registry\data\lipidmaps.sdf", lazy = FALSE) |> as.lipidmaps(lazy=FALSE);

registry |> setup_lipidmaps(dataset);