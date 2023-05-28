require(ggplot);

options(strict = FALSE);

bitmap(file = `${@dir}/UMAP3d.png`, size = [2400, 2000]) {
    
	let data = read.csv(`${@dir}/UMAP3.csv`, row.names = 1);
	# data[, "class"] = `class_${data[, "class"]}`;
	
	# colorset = {
	# class_5:"red",
	# class_0:"blue",
	# class_4:"white",
	# class_1:"yellow",
	# class_9:"green",
	# class_2:"skyblue",
	# class_3:"lime",
	# class_6:"orange",
	# class_7:"steelblue",
	# class_8:"brown"
	# };
	
	print(unique(data[, "class"]));
	
	# create ggplot layers and tweaks via ggplot style options
	ggplot(data, aes(x = "dimension_1", y = "dimension_2", z = "dimension_3"), padding = "padding:250px 500px 100px 100px;")
	+ geom_point(aes(color = "class"), color = "paper", shape = "triangle", size = 20)
	+ view_camera(angle = [31.5,65,125], fov = 100000)
	+ ggtitle("Scatter UMAP 3D")
	# + theme_black()
	;

}