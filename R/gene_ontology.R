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
    term$is_a = is_a(term$is_a)$id;
    term$synonym = synonym(term$synonym)$name;

    str(term);

    stop();
}