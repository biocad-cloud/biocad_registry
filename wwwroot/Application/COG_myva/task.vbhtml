<!DOCTYPE html>
<html>
<head>
    <%= ../../includes/head.vbhtml %>
    <%= ../../includes/breadcrumb/styles.vbhtml %>

    <?vb $title = "COG myva online annotation" ?>
    <?vb $active2 = "active" ?>
    <?vb $appname = "Check COG task progress" ?>
</head>
<body>

    <div id="main-wrapper">

        <%= ../../includes/navigation-bar.vbhtml %>

        <div class="row">
            <div class="small-12 columns">
                <h1>COG myva annotation progress</h1>

                <%= ../../includes/progress-indicator/indicator.vbhtml %>
                <?vb $url = "/Application/COG_task.vbs?uid=$uid" ?>

                <%= ../../includes/breadcrumb/applications.vbhtml %>
            </div>
        </div>

        <%= ../../includes/footer.vbhtml %>

    </div>

</body>
</html>
