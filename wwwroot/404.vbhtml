<!DOCTYPE html>
<html>
	<%= ./includes/head.vbhtml %>
	
		<link rel="stylesheet" href="./styles/404.css" type="text/css" />
	
	<?vb $title = "The requested page can't be found." ?>
	<?vb $active1 = "active" ?>
<body>

	<div id="main-wrapper">
    
		<%= ./includes/navigation-bar.vbhtml %>
    
		<div class="row">
			<div class="small-12 columns">
		
	<br />
		
		<!-- Begin Content -->
					<h1 class="page-header">The requested page can't be found.</h1>
						<div class="uk-grid">
							<div class="uk-width-medium-6-10">
								<p><strong>An error has occurred while processing your request.</strong></p>
								<p>You may not be able to visit this page because of:</p>
								<ul>
									<li>an <strong>out-of-date bookmark/favourite</strong></li>
									<li>a <strong>mistyped address</strong></li>
									<li>a search engine that has an <strong>out-of-date listing for this site</strong></li>
									<li>you have <strong>no access</strong> to this page</li>
								</ul>
							</div>
		
						</div>
						<hr />
						<p>If difficulties persist, please contact the System Administrator of this site and report the error below.</p>
						<blockquote>
							<span class="uk-badge uk-badge-danger">404</span> Article not found						</blockquote>
					<!-- End Content -->
		
			</div>
		</div>
    
		<%= ./includes/footer.vbhtml %>

	</div>
	
</body>
</html>
