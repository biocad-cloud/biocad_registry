<!DOCTYPE html>
<html>
<head>
	<%= ../../includes/head.vbhtml %>
	<%= ../../includes/breadcrumb/styles.vbhtml %>
	
	<?vb $title = "COG myva online annotation" ?>
	<?vb $active2 = "active" ?>
	<?vb $appname = "COG myva analysis" ?>
</head>
<body>

	<div id="main-wrapper">
    
		<%= ../../includes/navigation-bar.vbhtml %>
    
		<div class="row">
			<div class="small-12 columns">
				<h1>COG myva</h1>
				
				<form>
		Paste your sequence in FASTA format:
		<input type="textarea"></input>
		Or upload your protein fasta: 
		<input type="file"></input>
		
		blast+ parameters:
		E-value: <input type="text"></input>
		WordSize: <input type="text"></input>
		matrix:<input type="file"></input>
		
		Export parameters:
		Identities: <input type="text"></input>
		Coverage: <input type="text"></input>
		
		<input type="submit"></input>
				</form>
				
				<%= ../../includes/breadcrumb/applications.vbhtml %>
			</div>
		</div>
    
		<%= ../../includes/footer.vbhtml %>

	</div>
	
</body>
</html>
