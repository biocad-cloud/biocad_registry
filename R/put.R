#' Create a new taxonomic group in the registry
#' 
#' @return the unique reference id of the new taxonomic group
#'   in the registry.
#' 
const put.taxonomic_group = function(name, id, note = "") {
    const base = getOption("biocad");
    const url  = `${base}/put/taxonomic/`;
    const resp = requests.post(url, list(name, note, id)) |> http::content();

    # view of the taxonomic object put 
    # response
    str(resp);

    return(resp$info$id);
}