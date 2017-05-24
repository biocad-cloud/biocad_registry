<!DOCTYPE html>
<html>
<head>
    <%= ../includes/head.vbhtml %>
    <%= ../includes/breadcrumb/styles.vbhtml %>

    <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-3.2.1.min.js"></script>

    <?vb $title = "Biostack Task Manager" ?>
    <?vb $active2 = "active" ?>
    <?vb $appname = "View Task" ?>
</head>
<body>

    <div id="main-wrapper">

        <%= ../includes/navigation-bar.vbhtml %>

        <div class="row">
            <div class="small-12 columns">
                
				<p>Your task is pending in queue position: $position</p>

                <%= ../includes/breadcrumb/applications.vbhtml %>
            </div>
        </div>

        <%= ../includes/footer.vbhtml %>

    </div>

</body>
</html>
