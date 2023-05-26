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