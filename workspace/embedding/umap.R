require(umap);

setwd(@dir);

let fingerprint = read.csv("./nt_fingerprints.csv", row.names = 1, check.names = FALSE);
let embedding = umap(fingerprint, dimension = 9, numberOfNeighbors = 128);

embedding = as.data.frame(embedding$umap, labels = embedding$labels);

write.csv(embedding, file = "./nt_embedding.csv");