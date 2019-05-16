
var connection = new signalR.HubConnectionBuilder().withUrl("/chartHub").build();




connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    //document.getElementById("messagesList").appendChild(li);

    
});

connection.on("RestApiMsg", function (json) {

    InitTemperatureDatas(json);
    InitHumidityDatas(json);
    drawCurveTypes();
    drawCurveTypes1();
});




connection.start().then(function () {
   
}).catch(function (err) {
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