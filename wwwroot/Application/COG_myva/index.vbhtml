<!DOCTYPE html>
<html>
	<%= ../../includes/head.vbhtml %>
	<%= ../../includes/breadcrumb/styles.vbhtml %>
	
	<?vb $title = "COG myva online annotation" ?>
	<?vb $active2 = "active" ?>
	<?vb $appname = "COG myva analysis" ?>
<body>

	<div id="main-wrapper">
    
		<%= ../../includes/navigation-bar.vbhtml %>
    
		<div class="row">
			<div class="small-12 columns">
				<h1>COG myva</h1>
				
				<form>
					Upload your protein fasta: <input type="file"></input>
				</form>
				
				<%= ../../includes/breadcrumb/applications.vbhtml %>
			</div>
		</div>
    
		<%= ../../includes/footer.vbhtml %>

	</div>
	
</body>
</html>
