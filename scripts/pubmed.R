require(biocad_registry);
require(GCModeller);
require(mzkit);

imports "pubmed" from "kb";
imports "massbank" from "mzkit";
imports "data_exports" from "biocad_registry";

let pubmed_kb = ?"--pubmed" || stop("a json file of the pubmed list about the compound reference must be provided!");
let output_dir = ?"--outputdir" || dirname(pubmed_kb);
let cache_dir = ?"--cache" || file.path(output_dir,".cache/");

let pull_list = [];
let article_list = [];
let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");

pubmed_kb = read.article_json(pubmed_kb);

for(let article in tqdm(pubmed_kb)) {
    let cids = [article]::cids;

    if (length(cids) > 0) {
        pull_list = append(pull_list, biocad_registry |> export_by_cids(cids));
        article_list = append(article_list, article);
    }

    NULL;
}

let metabolites = as.data.frame(pull_list);

pubmed_kb = data.frame(
    pubmed_id = [article_list]::pmid,
    title = [article_list]::title,
    citation = [article_list]::citation,
    doi = [article_list]::doi
);

write.csv(metabolites, file = file.path(output_dir, "metabolites.csv"));
write.csv(pubmed_kb, file = file.path(output_dir, "articles.csv"), row.names = FALSE);