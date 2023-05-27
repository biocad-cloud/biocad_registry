const get_motif_sites = function(family) {
    const base = getOption("biocad");
    const url = `http://registry.biocad.cloud:8848/exportServlet/motif_sites/family/?q=${family}`;
    const data = url 
    |> requests.get()
    |> http::content()
    ;

    if (data$code != 0) {
        stop(data$info);
    }

    const [count, sites] = data$info;

    if (count == 0) {
        NULL;
    } else {
        data.frame(
            gene_id    = sites@gene_id,
            gene_name  = sites@gene_name,
            loci       = sites@loci,
            score      = sites@score,
            motif_site = sites@site,
            row.names  = `${sites@gene_id}:${sites@loci}`
        );
    }
}