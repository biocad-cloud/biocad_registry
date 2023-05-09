const put.taxonomic_group = function(name, note = "") {
    const base = getOption("biocad");
    const url  = `${base}/registry/put/taxonomic/`;
    const resp = requests.post(url, list(name, note)) |> http::content();

    str(resp);
}