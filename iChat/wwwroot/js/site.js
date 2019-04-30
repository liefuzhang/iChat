// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

"use strict";

var app = {};


var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.on("UpdateChannel", function (channelID) {
    if (channelID === app.channelID) {
        document.location.reload();
    }
});

connection.start().then(function () {
    // TODO finish loading page here after connection started

}).catch(function (err) {
    return console.error(err.toString());
});

//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var user = document.getElementById("userInput").value;
//    var message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", user, message).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});

