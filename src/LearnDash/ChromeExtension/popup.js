var xhr = new XMLHttpRequest();
xhr.open("GET", "http://learndash.mfranc.com/api/flows/2", true);
xhr.onreadystatechange = function() {
  if (xhr.readyState == 4) {
    var resp = JSON.parse(xhr.responseText);
	$("#name").text(resp.Name);
	$.each(resp.Tasks, function(index, item){
		$("#items").append('<span>'+ item.Name +'</span><br />')
	});
  }
}
xhr.send();
