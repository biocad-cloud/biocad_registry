const pull_taxonomic = function(id = NULL) {
    const base = getOption("biocad");
    const url = {
        if (is.null(id)) {
            `${base}/pull/taxonomic_group/`;
        } else {
            `${base}/pull/taxonomic_group/?id=${id}`;
        };
    }
    const resp = requests.get(url) |> http::content();

    if (resp$code == 0) {
        resp$info;
    } else {
        stop(resp$info);
    }
}

#' Fetch all motif family
#'  
const pull_motif_family = function(genome_id = NULL) {
    const base = getOption("biocad");
    const url = {
        if (is.null(genome_id)) {
            `${base}/pull/motif_site_family/`;
        } else {
            `${base}/pull/motif_site_family/?genome=${genome_id}`;
        };
    }
    const resp = requests.get(url) |> http::content();

    if (resp$code == 0) {
        const data = resp$info;
        const df = data.frame(
            family = data@family,
            nsize = as.integer(data@nsize)
        );

        df;
    } else {
        stop(resp$info);
    }
}