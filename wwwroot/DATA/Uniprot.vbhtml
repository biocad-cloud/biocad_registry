<!DOCTYPE html>
<html>
<head>
	<%= ./includes/head.vbhtml %>
<%= ./includes/breadcrumb/styles.vbhtml %>

	<?vb $title = "KEGG reference navigation" ?>
	<?vb $active2 = "active" ?>
</head>
<body>

	<div id="main-wrapper">
    
		<%= ./includes/navigation-bar.vbhtml %>
    
		<div class="row">
			<div class="small-12 columns">        
			
			<br />
			
			<h1><%= @Uniprot %></h1>
	
			<%= ./includes/breadcrumb/uniprot.vbhtml %>

			</div>
		</div>
	
		<%= ./includes/footer.vbhtml %>

	</div>
		
</body>
</html>
