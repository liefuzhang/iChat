"use strict";

var mainController = (function () {
    var quill;

    function toggleFormatChars(char) {
        var selection = quill.getSelection();
        var length = selection.length;
        if (length < 1)
            return;

        var start = selection.index;
        var text = quill.getText(start, length);
        var newText;
        if (text.length > 2 && text.startsWith(char) && text.endsWith(char)) {
            // remove char
            newText = text.substr(1, length - 2);
            quill.deleteText(start, length);
            quill.insertText(start, newText);
            quill.setSelection(start, length - 2);
        } else {
            // add char
            newText = char + text + char;
            quill.deleteText(start, length);
            quill.insertText(start, newText);
            quill.setSelection(start, length + 2);
        }
    }

    function init(channelName) {
        var messageScrollable = document.querySelector(".message-scrollable");
        messageScrollable.scrollTop = messageScrollable.scrollHeight;
        messageScrollable.style.visibility = 'visible'; // show now to avoid flickering

        var bindings = {
            list: {
                key: 'enter',
                shiftKey: false,
                handler: function () {
                    var message = quill.root.innerHTML;
                    if (!message)
                        return false;
                    var form = document.querySelector('#messageForm');
                    var messageInput = document.querySelector('input[name=newMessage]');
                    messageInput.value = message;
                    form.submit();
                    return false;
                }
            }
        };

        quill = new Quill('#messageEditor',
            {
                theme: 'snow',
                placeholder: `Message ${channelName}`,
                modules: {
                    keyboard: {
                        bindings: bindings
                    }
                }
            });

        var messageBox = document.querySelector('.message-box');
        messageBox.addEventListener("keydown",
            function(event) {
                var ret = true;
                var char ='';
                if (event.ctrlKey || event.metaKey) {
                    switch (event.keyCode) {
                        case 66: // ctrl+B or ctrl+b
                        case 98:
                            ret = false;
                            char = '*';
                            break;
                        case 73: // ctrl+I or ctrl+i
                        case 105:
                            ret = false;
                            char = '_';
                            break;
                        case 85: // ctrl+U or ctrl+u
                        case 117:
                            ret = false;
                            break;
                    }
                }
                if (!!char) {
                    toggleFormatChars(char);
                }

                if (ret === false) {
                    event.stopPropagation();
                    event.preventDefault();
                    return false;
                }

                return ret;
            }, true); // true - event capturing phase
    }

    return {
        init: init
    };
})();
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

"use strict";

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

