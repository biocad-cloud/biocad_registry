imports "umap" from "MLkit";

setwd(@dir);

let data = read.csv("./motif_graph.csv", row.names = 1, check.names = FALSE);
let labels = data$class;

data[, "class"] = NULL;
data[, "v1"] = NULL;
data[, "v2"] = NULL;
data[, "v3"] = NULL;
data[, "v4"] = NULL;

let dim3 = umap(data, dimension = 3, spectral_cos = TRUE);
let result = as.data.frame(dim3$umap, labels = dim3$labels);

result[, "class"] = labels;

write.csv(result, file = "./umap3.csv", row.names = TRUE);
