<!DOCTYPE html>
<html>
	<%= ./includes/head.vbhtml %>
	<?vb $title = "KEGG reference navigation" ?>
	<?vb $active2 = "active" ?>
<body>

	<div id="main-wrapper">
    
		<%= ./includes/navigation-bar.vbhtml %>
    
		<div class="row">
			<div class="small-12 columns">        
			
			<h1><%= @Uniprot %></h1>
	
			</div>
		</div>
	
		<%= ./includes/footer.vbhtml %>

	</div>
		
</body>
</html>
