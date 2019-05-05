'use strict';

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var LikeButton = function (_React$Component) {
    _inherits(LikeButton, _React$Component);

    function LikeButton(props) {
        _classCallCheck(this, LikeButton);

        var _this = _possibleConstructorReturn(this, (LikeButton.__proto__ || Object.getPrototypeOf(LikeButton)).call(this, props));

        _this.state = { liked: false };
        return _this;
    }

    _createClass(LikeButton, [{
        key: 'render',
        value: function render() {
            var _this2 = this;

            if (this.state.liked) {
                return 'You liked this.';
            }

            return React.createElement(
                'button',
                { onClick: function onClick() {
                        return _this2.setState({ liked: true });
                    } },
                'Like'
            );
        }
    }]);

    return LikeButton;
}(React.Component);

var domContainer = document.querySelector('#like_button_container');
ReactDOM.render(React.createElement(LikeButton, null), domContainer);

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

    function registerEventHandlers() {
        $('.ql-editor').on('focus', () => {
            $('.message-box').addClass('focus');
        }).on('blur', () => {
            $('.message-box').removeClass('focus');
        });
    }

    function init(channelName) {
        initSimpleBar();
        initQuill(channelName);
        captureKeydownEvent();
        registerEventHandlers();
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