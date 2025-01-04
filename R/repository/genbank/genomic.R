const imports_genomic_refseq = function(biocad_registry, gbff) {
    imports "GenBank" from "seqtoolkit";

    if (is.character(gbff)) {
        gbff <- GenBank::load_genbanks(gbff);
    }

    for(let genome in gbff) {

    }
}