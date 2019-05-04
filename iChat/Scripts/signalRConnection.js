// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

var signalRController = (function () {
    var connection;

    function init(channelID) {
        connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
        connection.on("UpdateChannel",
            function (channelID) {
                if (channelID === app.selectedChannelId) {
                    document.location.reload();
                }
            });

        connection.on("ReceiveMessage",
            function () {
                document.location.reload();
            });

        connection.start().then(function () {
            $("#container").removeClass("page-loading");
            addUserToChannelGroup(channelID);
        }).catch(function (err) {
            return console.error(err.toString());
        });
    }

    function addUserToChannelGroup(channelID) {
        connection.invoke("AddToChannelGroup", channelID).catch(function (err) {
            return console.error(err.toString());
        });
    }

    return {
        init: init
    };
})();