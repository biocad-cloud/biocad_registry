let umap = read.csv(relative_work("./enzyme_embedding.csv"), row.names = 1, check.names = FALSE);

umap[,"class"] = `class_${rownames(umap) |> strsplit(".",fixed=TRUE) |> sapply(s -> .Internal::first(s))}`;

write.csv(umap, file = relative_work("./enzyme_embedding.csv"));