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
                <h1>Task progress</h1>

				<!-- 
				
				1. 当用户提交了一个分析任务之后，系统会为所提交的用户任务分配一个md5作为该任务的uid
				2. 用户访问查看任务状态或者结果的url为： /Application/TaskManager.vbhtml?uid=<user_task_md5_uid>
				3. 可能服务器处于忙状态的时候，用户任务会被放入一个任务队列之中排队
				4. 则在服务器系统之中会首先检查任务队列之中的排队状态，如果是处于排队状态的，则系统会重定向返回的页面至TaskPending.vbhtml页面查看排队的状态位置
				5. 假若任务没有处于排队状态，而是已经在执行之中了，则会停留在当前的页面，定时更新任务的执行状态
				6. 当任务执行完毕之后，则会重定向跳转到任务模型之中的定义的结果页面的url
				
				-->
				
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
