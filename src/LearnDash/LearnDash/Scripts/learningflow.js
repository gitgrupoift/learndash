function CreateTask(taskData) {
    var flowTaskName = taskData.find("span").html();
    var isNext = taskData.hasClass("flow-next");
    var timesDone = taskData.attr("data-counter");
    var id = taskData.attr("data-id");
    var task =
            {
                ID : id,
                Name: flowTaskName,
                IsNext: isNext,
                TimesDone: timesDone
            };
    return task;
}

    function SaveFlow(flowId,url) {
        var flowName = $(".flow-name span").html();
        var flowTasks = new Array();

        $(".flow-task").each(function () {
            var newFlowTask = CreateTask($(this));
            flowTasks.push(newFlowTask);
        });

        var learningFlow = {
            Id    : flowId,
            Name  : flowName,
            Tasks : flowTasks
        };

        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify(learningFlow),
            dataType : "json",
            contentType : 'application/json',
            success: function (data) {
                var jsonData = $.parseJSON(data);
                if (jsonData.items.length <= 0) {
                }
            }
        });
    }