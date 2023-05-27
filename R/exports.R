imports "bioseq.fasta" from "seqtoolkit";

#' web url component path for export motif sites
#' 
const motif_sites_family = "exportServlet/motif_sites/family";

#' Export motif sites from the biocad registry
#' 
#' @param family A specific motif family for export data
#' @param fasta this parameter affect the return value is a dataframe
#'    or fasta collection object
#' 
const get_motif_sites = function(family, fasta = FALSE) {
    const base = getOption("biocad");
    const url = `${base}/${motif_sites_family}/?q=${family}`;
    const data = url
    |> requests.get()
    |> http::content()
    ;

    if (data$code != 0) {
        stop(data$info);
    }

    const [count, sites] = data$info;

    if (count == 0) {
        return(NULL);
    }

    if (!fasta) {
        data.frame(
            gene_id    = sites@gene_id,
            gene_name  = sites@gene_name,
            loci       = sites@loci,
            score      = sites@score,
            motif_site = sites@site,
            row.names  = `${sites@gene_id}:${sites@loci}`
        );
    } else {
        as.fasta(data.frame(
            title = `${sites@gene_id}:${sites@loci}`,
            sequence = sites@site
        ));
    }
}