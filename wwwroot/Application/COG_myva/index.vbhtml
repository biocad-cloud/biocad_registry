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
				
				<form method="POST">
				<fieldset>
				<legend>
				Enter Query Sequence
				</legend>
		Paste your sequence in FASTA format:<br /><br />
		<p>
		<textarea name="fasta.text" placeholder="Input protein sequence in FASTA format." rows="15"></textarea>
		Or upload your protein fasta: 
		<input name="fasta.file" type="file"></input>
		</p>
		</fieldset>
		<fieldset>
            <legend>
              blast+ Parameters
            </legend>
          E-value: <input type="text"></input>
		WordSize: <input type="text"></input>
		matrix:<input type="file"></input>
        </fieldset>
        
		
		<fieldset>
		<legend>
		Export parameters:
		</legend>
		Identities: <input type="text"></input>
		Coverage: <input type="text"></input>
		</fieldset>
		
		
		
		
		
		<button name="submit">Run Annotation</button>
				</form>
				
				<%= ../../includes/breadcrumb/applications.vbhtml %>
			</div>
		</div>
    
		<%= ../../includes/footer.vbhtml %>

	</div>
	
</body>
</html>
