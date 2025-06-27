require(biocad_registry);
require(GCModeller);

imports "pubmed" from "kb";

let pubmed_kb = ?"--pubmed" || stop("a json file of the pubmed list about the compound reference must be provided!");
let cache_dir = ?"--cache" || file.path(dirname(pubmed_kb),".cache/");

pubmed_kb = read.article_json(pubmed_kb);

for(let article in tqdm(pubmed_kb)) {
    let cids = [article]::cids;

    if (length(cids) > 0) {
        
    }
}