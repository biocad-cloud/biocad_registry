<!DOCTYPE html>
<html>
<head>
    <%= ../../includes/head.vbhtml %>
    <%= ../../includes/breadcrumb/styles.vbhtml %>

    <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-3.2.1.min.js"></script>

    <?vb $title = "Biostack Server Configuration" ?>
    <?vb $active1 = "active" ?>
    <?vb $appname = "Settings" ?>
</head>
<body>

    <div id="main-wrapper">

        <%= ../../includes/navigation-bar.vbhtml %>

        <div class="row">
            <div class="small-12 columns">
                <h1>Biostack Server Configuration</h1>

				<form method="POST" enctype="multipart/form-data" action="/Application/admin/write_settings.vbs">
                    <fieldset>
                        <legend>
                            NCBI localblast
                        </legend>
                        
						<input name="localblast"></input>
                    </fieldset>
					
					<button>Write Settings</button>
				</form>

                <%= ../../includes/breadcrumb/applications.vbhtml %>
            </div>
        </div>

        <%= ../../includes/footer.vbhtml %>

    </div>

</body>
</html>
