var host = "http://localhost:56698";

function sendRequest(url, func){
	var xhr = new XMLHttpRequest();
	xhr.open("GET", url, true);
	xhr.onreadystatechange = function() {
	  if (xhr.readyState == 4) {
	  	var messageReceived = xhr.responseText;
	    var resp = JSON.parse(messageReceived);
	    
	    func(resp);

	  }
	}
	xhr.send();
}

function getAllFlows(func) {
	sendRequest(host + "/api/flows/getall", func);
}

function getSingleFlow(flowId, func){
	sendRequest(host + "/api/flows/get/" + flowId, func);
}

$(function(){
	getAllFlows(function(data){
		$.each(data, function(index, item){
				$("#items").append('<a class="itemClick" href="#" data-id="'+ item.ID +'"" >' + item.Name +'</a><br />')
			});

		$(".itemClick").click(function(){
				var id = $(this).attr("data-id");
				getSingleFlow(id, function(data){
					$("#children").empty();
					$.each(data.Tasks, function(index, item){
					$("#children").append('<span>'+ item.Name +'</span><br/>');
				});
			});
		});
	});
})
