<!DOCTYPE html>
<html>
	<%= ../../includes/head.vbhtml %>
	<?vb $title = "Welcome to the GCModeller biostack platform" ?>
	<?vb $active2 = "active" ?>
	<?vb $app.name = "COG myva analysis" ?>
<body>

	<div id="main-wrapper">
    
		<%= ../../includes/navigation-bar.vbhtml %>
    
		<div class="row">
			<div class="small-12 columns">
				<h1>COG myva</h1>
				
				<form>
					Upload your protein fasta: <input type="file"></input>
				</form>
			</div>
		</div>
    
		<%= ../../includes/footer.vbhtml %>

	</div>
	
</body>
</html>
