require(umap);

setwd(@dir);

let fingerprint = read.csv("Z:\\enzyme.csv", row.names = "Cluster", check.names = FALSE);
fingerprint[,"ID"] = NULL;
str(fingerprint);
let embedding = umap(fingerprint, dimension = 9, numberOfNeighbors = 32, method = "NormalizedCosine");
let num = $"\d+";

embedding = as.data.frame(embedding$umap, labels = embedding$labels);
embedding[,"class"] = `class_${rownames(embedding) |> strsplit(".",fixed=TRUE) |> sapply(s -> .Internal::first(s))}`;

write.csv(embedding, file = "./enzyme_embedding.csv");