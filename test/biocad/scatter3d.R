require(ggplot);
options(strict = FALSE);

bitmap(file = `${@dir}/UMAP3d.png`, size = [4000, 2700]) {
	let data = read.csv(`${@dir}/UMAP3.csv`, row.names = 1);
	# data[, "class"] = `class_${data[, "class"]}`;
	let i = data$class in ["Crp","Fur","LexA","CcpA","ArgR","Zur",
		"Fnr","NrdR","CodY","Rex","MetJ","NtrC","HrcA","HexR",
		"PurR","LiuR","NarP","PsrA","IscR","MetR","GlxR","RpoN","TyrR","FadR",
		"FruR","NagR","HutC","PerR","FabR","BirA"
	];

	data = data[i, ];

	let o = as.numeric(data$dimension_1) ^ 2 
	+ as.numeric(data$dimension_2) ^ 2 
	+ as.numeric(data$dimension_3) ^ 2;

	# removes outlier
	o = order(o);
	o = o[1:as.integer(0.95*length(o))];

	data = data[o, ];

	print(unique(data[, "class"]));

	# create ggplot layers and tweaks via ggplot style options
	ggplot(data, aes(
		x = "dimension_1", 
		y = "dimension_2", 
		z = "dimension_3"
	), padding = "padding:250px 500px 100px 100px;")
	+ geom_point(aes(color = "class"), 
		color = "paper",
		shape = "circle", 
		size = 13)
	+ view_camera(angle = [31.5,65,125], fov = 100000)
	+ ggtitle("Bacterial Gene Regulatory Motif Landscape")
	# + theme_black()
	;
}