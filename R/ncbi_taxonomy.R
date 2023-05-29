const put.ncbi_tax = function(tax) {
    const base = getOption("biocad");
    const url = `${base}/put/ncbi_tax/`;
    const resp = url 
    |> requests.post(tax)
    |> http::content(throw.http.error = FALSE)
    ;

    str(resp);
}