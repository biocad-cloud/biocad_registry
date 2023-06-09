imports "file" from "gokit";

const go_namespace = list(
    cellular_component = 1,
    biological_process = 2,
    molecular_function = 3
);

const put.go_term = function(term) {
    const base = getOption("biocad");
    const url = `${base}/gene_ontology/add/`;

    term$namespace = go_namespace[[term$namespace]];
    term$is_a      = is_a(term$is_a)$id;
    term$is_a      = wrap_list(unlist($"\d+"(term$is_a)));
    term$synonym   = wrap_list(synonym(term$synonym)$name);
    term$xref      = term_xrefs(term$xref);
    term$id        = $"\d+"(term$id);
    term$def       = gsub(term$def, '"', "");

    str(term);

    const resp = url 
    |> requests.post(term) 
    |> http::content(throw.http.error = FALSE)
    ;

    str(resp);
}