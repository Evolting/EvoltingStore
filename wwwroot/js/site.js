// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/commentHub").build();
document.getElementById("sendButton").disabled = true;

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("ReceivedMess", function (user, message) {
    console.log("Received Data");
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var userId = document.getElementById("userId").value;
    var gameId = document.getElementById("gameId").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("PostComment", userId, gameId, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
