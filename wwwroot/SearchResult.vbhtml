<!DOCTYPE html>
<html>
<head>
	<%= ./includes/head.vbhtml %>
	<%= ./includes/breadcrumb/styles.vbhtml %>
	
	<?vb $title = "Welcome to the GCModeller biostack platform" ?>
	<?vb $active1 = "active" ?>
</head>
<body>

	<div id="main-wrapper">
    
		<%= ./includes/navigation-bar.vbhtml %>
    
		<div class="row">
			<div class="small-12 columns">
				<h1>Search Result</h1>
				<%= ./includes/breadcrumb/search.vbhtml %>
			</div>
		</div>
    
		<%= ./includes/footer.vbhtml %>

	</div>
	
</body>
</html>
