require(biocad_registry);
require(JSON);

let biocad_registry = open_registry("root", 123456, host ="192.168.3.233");
let graph = export_reactionLinks(biocad_registry); 
let links = sapply(graph, l -> json_encode(l));

writeLines(links, con = file.path(@dir, "reaction_graph.jsonl"));