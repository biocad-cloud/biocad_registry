<!DOCTYPE html>
<html>
	<%= ./includes/head.vbhtml %>
	<%= ./includes/breadcrumb/styles.vbhtml %>
	
	<?vb $title = "KEGG reference navigation" ?>
	<?vb $active2 = "active" ?>
<body>

	<div id="main-wrapper">
    
		<%= ./includes/navigation-bar.vbhtml %>
    
		<div class="row">
			<div class="small-12 columns">
			
			<%= ./includes/breadcrumb/kegg.vbhtml %>
			
			<h1><%= @KEGG %></h1>
    
			</div>
		</div>
	
		<%= ./includes/footer.vbhtml %>

	</div>
		
</body>
</html>
