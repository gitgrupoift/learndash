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

    function ClearFlow(flowId, url, func) {
        var flowName = $(".flow-name span").html();
        var learningFlow = {
            Id    : flowId,
            Name  : flowName
        };

        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify(learningFlow),
            dataType : "json",
            contentType : 'application/json',
            success: func
        });
    }

    function CompleteTask(flowId, newId, currId, url, func) {

        var data;    

        if (newId) {
            data = {
                flowId: flowId,
                newCompleteTaskId: newId,
                currentCompleteTaskId: currId
            };
        }
        else {
            data = {
                flowId: flowId,
                currentCompleteTaskId: currId
            };
        }

        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify(data),
            dataType: "json",
            contentType: 'application/json',
            success: func
        });
    }

    function MakeNext(flowId, newId, url, func) {
        var data = {
            flowId : flowId, 
            newCompleteTaskId: newId
        };

        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify(data),
            dataType: "json",
            contentType: 'application/json',
            success: func
        });
    }