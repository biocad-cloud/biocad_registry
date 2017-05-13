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
                <h1>COG myva annotation progress</h1>

				<!-- 在下面的位置区域之中显示任务的进度，内容包括已经完成的任务和未完成的任务 -->
                <%= ../includes/progress-indicator/indicator.vbhtml %>
                <?vb $url = "/Application/getTask_status.vbs?uid=$uid" ?>

                <%= ../includes/breadcrumb/applications.vbhtml %>
            </div>
        </div>

        <%= ../includes/footer.vbhtml %>

    </div>

</body>
</html>
