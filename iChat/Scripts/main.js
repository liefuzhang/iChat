"use strict";

var mainController = (function () {
    var quill;

    function toggleFormatChars(range, char) {
        var text = quill.getText(range.index, range.length);
        var newText;
        if (text.length > 2 && text.startsWith(char) && text.endsWith(char)) {
            // remove char
            newText = text.substr(1, range.length - 2);
            quill.deleteText(range.index, range.length);
            quill.insertText(range.index, newText);
            quill.setSelection(range.index, range.length - 2);
        } else {
            // add char
            newText = char + text + char;
            quill.deleteText(range.index, range.length);
            quill.insertText(range.index, newText);
            quill.setSelection(range.index, range.length + 2);
        }
    }

    //var bindings = {
    //    enter: {
    //        key: 'enter',
    //        shiftKey: false,
    //        handler: function () {
    //            var message = quill.root.innerHTML;
    //            if (!message)
    //                return false;

    //            var form = document.querySelector('#messageForm');
    //            var messageInput = document.querySelector('input[name=newMessage]');
    //            messageInput.value = message;
    //            form.submit();
    //            return false;
    //        }
    //    },
    //    custom_bold: {
    //        key: 'B',
    //        shortKey: true,
    //        handler: function (range, context)
    //        {
    //            if (range.length < 1)
    //                return false;

    //            toggleFormatChars(range, '*');

    //            return false;
    //        }
    //    },
    //    bold: {
    //        key: 'I',
    //        shortKey: true,
    //        handler: function (range, context) {
    //            if (range.length < 1)
    //                return false;

    //            toggleFormatChars(range, '_');

    //            return false;
    //        }
    //    }
    //};

    function init(channelName) {
        quill = new Quill('#messageEditor',
            {
                theme: 'snow',
                placeholder: `Message ${channelName}`,
                //modules: {
                //    keyboard: {
                //        bindings: bindings
                //    }
                //}
            });

        var messageBox = document.querySelector('.message-box');
        messageBox.addEventListener("keydown",
            function(event) {
                var ret = true;
                if (event.ctrlKey || event.metaKey) {
                    switch (event.keyCode) {
                        case 66: // ctrl+B or ctrl+b
                        case 98:
                            ret = false;
                            break;
                        case 73: // ctrl+I or ctrl+i
                        case 105:
                            ret = false;
                            break;
                        case 85: // ctrl+U or ctrl+u
                        case 117:
                            ret = false;
                            break;
                    }
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