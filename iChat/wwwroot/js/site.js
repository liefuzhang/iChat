"use strict";

var mainController = (function () {
    var quill;
    
    function initSimpleBar() {
        var messageScrollable = document.querySelector(".message-scrollable");

        // init simplebar
        new SimpleBar(messageScrollable);

        var scrollable = document.querySelector(".simplebar-content");
        scrollable.scrollTop = scrollable.scrollHeight;
        messageScrollable.style.visibility = 'visible'; // show now to avoid flickering
    }

    function configPlainClipboard() {
        var Clipboard = Quill.import('modules/clipboard');
        var Delta = Quill.import('delta');

        class PlainClipboard extends Clipboard {
            convert(html = null) {
                if (typeof html === 'string') {
                    this.container.innerHTML = html;
                }
                let text = this.container.innerText;
                this.container.innerHTML = '';
                return new Delta().insert(text);
            }
        }

        Quill.register('modules/clipboard', PlainClipboard, true);
    }

    function initQuill(channelName) {
        configPlainClipboard();

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
    }

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

    function captureKeydownEvent() {
        var messageBox = document.querySelector('.message-box');
        messageBox.addEventListener("keydown",
            function (event) {
                var ret = true;
                var char = '';
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

    function init(channelName) {
        initSimpleBar();
        initQuill(channelName);
        captureKeydownEvent();
    }

    return {
        init: init
    };
})();
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

var signalRController = (function () {
    var connection;

    function init(channelID) {
        connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
        connection.on("UpdateChannel",
            function (channelID) {
                if (channelID === app.channelID) {
                    document.location.reload();
                }
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