require(umap);

setwd(@dir);

let fingerprint = read.csv("Z:\fingerprint.csv", row.names = "Cluster", check.names = FALSE);
fingerprint[,"ID"] = NULL;
str(fingerprint);
let embedding = umap(fingerprint, dimension = 9, numberOfNeighbors = 32);
let num = $"\d+";

embedding = as.data.frame(embedding$umap, labels = embedding$labels);
embedding[,"class"] = `class_${num(embedding$labels)}`;

write.csv(embedding, file = "./enzyme_embedding.csv");