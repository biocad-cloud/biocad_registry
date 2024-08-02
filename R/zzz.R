imports "http" from "webKit";
imports "regprecise" from "TRNtoolkit";
imports "models" from "biocad_registry";
imports "mysql" from "graphR";

require(GCModeller);
require(graphQL);

const .onLoad = function() {
    options(biocad = "http://registry.biocad.cloud:8848");
}