imports "package_utils" from "devkit";
imports ["jQuery", "Html", "http"] from "webKit";

package_utils::attach(`${@dir}/../../`);

motifs = biocad_registry::get_motif_sites(family = "PsrA"); 

print(motifs);

imports "bioseq.patterns" from "seqtoolkit";

motif_familys = biocad_registry::pull_motif_family();
i = motif_familys$nsize > 5;
motif_familys = motif_familys[i, ];
i = order(motif_familys$nsize, decreasing = TRUE);
motif_familys = motif_familys[i, ];

print(motif_familys);

for(family in motif_familys$family) {
    motifs = biocad_registry::get_motif_sites(family = family); 
    print(motifs);
    fa = as.fasta(data.frame(title = rownames(motifs), sequence = motifs$motif_site));
    write.fasta(fa, file = `${@dir}/exports/${normalizeFileName(family)}.fa`);

    
}
