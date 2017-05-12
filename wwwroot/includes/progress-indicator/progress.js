/*
   // Current progress phase is phase2
   {
      "current": 1,
      "progress": ["phase1", "phase2", "phase3"]
   }

*/


/**
 * 假若是第一次加载，则会创建所需要的html元素对象
 * 假若不是第一次加载，则只会更新已有的html元素对象的属性值
 *
 */

/**
 * 在这个js函数之中每隔一段时间都会像服务器请求数据，更新DOM的属性，
 * 从而能够实时显示任务的进度
 *
 * @param div 所需要显示的节点位置
 * @param url 请求的数据源，后面也会定时的从这个数据源请求任务的状态数据
 */ 
function showProgress(div, url) {

    var node = document.getElementById(div);
    var ul   = document.createElement("ul");

    $.getJSON(url, function(data) {

        var current = data.current;
        var list = [];

        data.progress.forEach(function (name, i) {

            var progress = document.createElement("li");

            if (i == current) {
                // 字体加粗表示当前在执行的任务
                progress.html = "<strong>" + name + "</strong>";
            } else {
                // 字体使用灰色，表示已经执行完毕或者还没有执行的任务
                progress.html = name;
                progress.style = "color:grey";
            }

            list.push(progress);
            ul.appendChild(progress);
        });

        node.appendChild(ul);

        /**
         * 使用定时器进行属性更新
         */ 
        function update() {
            $.ajax({
                url: url,
                success: function (prog) {

                    // 恢复状态
                    list[current].html = prog.progress[current];
                    list[current].style = "color:grey";

                    current = prog.current;

                    // 将当前的任务置为加粗
                    list[current].html = "<strong>" + prog.progress[current] + "</strong>";
                    list[current].style = null;
                }
            });
        }

        setInterval("update()", 30 * 1000);
    });
}