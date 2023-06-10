imports "http" from "webKit";
imports "regprecise" from "TRNtoolkit";
imports "models" from "biocad_registry";

require(GCModeller);

const .onLoad = function() {
    options(biocad = "http://registry.biocad.cloud:8848");
}