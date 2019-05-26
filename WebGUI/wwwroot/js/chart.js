var connection = new signalR.HubConnectionBuilder().withUrl("/chartHub").build();





connection.on("RestApiMsg", function(json) {
    var navbar = document.getElementsByClassName("nbar");
    var activeLink = document.getElementsByClassName("active")[0].textContent;
    if (activeLink == "Logs") {
        DeserealizeAndControlForLogging(json);
    } else {
        DeserealizeAndControlForVisualisation(json);
    }


});




connection.start().then(function() {

}).catch(function(err) {
    return console.error(err.toString());
});
/*
document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});*/