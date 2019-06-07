import React from "react";
import "./ContentFooter.css";
import Quill from "quill";
import AuthService from "services/AuthService";

class ContentFooter extends React.Component {
  constructor(props) {
    super(props);

    this.quill = {};
    this.authService = new AuthService(props);
    this.keydownEventHandler = keydownEventHandler.bind(this);
  }

  componentDidMount() {
    this.initQuill();
    this.captureKeydownEvent();
    this.registerEventHandlers();
  }

  componentWillUnmount() {
    this.unregisterEventHandlers();
  }

  configPlainClipboard() {
    var Clipboard = Quill.import("modules/clipboard");
    var Delta = Quill.import("delta");

    class PlainClipboard extends Clipboard {
      convert(html = null) {
        if (typeof html === "string") {
          this.container.innerHTML = html;
        }
        let text = this.container.innerText;
        this.container.innerHTML = "";
        return new Delta().insert(text);
      }
    }

    Quill.register("modules/clipboard", PlainClipboard, true);
  }

  initQuill() {
    this.configPlainClipboard();
    var submitMessage = function() {
      var text = this.quill.getText().trim();
      if (!text) return;

      var message = this.quill.root.innerHTML;

      this.quill.root.innerHTML = "";

      if (this.props.isChannel) {
        this.authService.fetch(`/api/messages/${this.props.section}/${this.props.id}`, {
          method: "POST",
          body: JSON.stringify(message)
        });
      } else {
        this.authService.fetch(`/api/messages/${this.props.section}/${this.props.id}`, {
          method: "POST",
          body: JSON.stringify(message)
        });
      }
    };
    submitMessage = submitMessage.bind(this);

    var bindings = {
      list: {
        key: "enter",
        shiftKey: false,
        handler: function() {
          submitMessage();
          return false;
        }
      }
    };

    this.quill = new Quill("#messageEditor", {
      theme: "snow",
      placeholder: `Type your message...`,
      modules: {
        keyboard: {
          bindings: bindings
        }
      }
    });
  }

  captureKeydownEvent() {
    var messageBox = document.querySelector(".message-box");
    messageBox.addEventListener("keydown", this.keydownEventHandler, true); // true - event capturing phase
  }

  registerEventHandlers() {
    let editor = document.querySelector(".ql-editor");
    editor.addEventListener("focus", toggleFocus);
    editor.addEventListener("blur", toggleFocus);
  }

  unregisterEventHandlers() {
    var messageBox = document.querySelector(".message-box");
    let editor = document.querySelector(".ql-editor");
    messageBox.removeEventListener("keydown", this.keydownEventHandler, true); // true - event capturing phase
    editor.removeEventListener("focus", toggleFocus);
    editor.removeEventListener("blur", toggleFocus);
  }

  render() {
    return (
      <div className="footer">
        <form id="messageForm" method="post">
          <div className="message-box">
            <div id="messageEditor" />
          </div>
        </form>
        <div className="message-prompt">
          <b>*bold*</b>&nbsp;
          <span className="grey-background">`code`</span>&nbsp;
          <span className="grey-background">```preformatted```</span>&nbsp;
          <i>_italics_</i>&nbsp;
          <span>~strike~</span>&nbsp;
          <span>>quote</span>&nbsp;
        </div>
      </div>
    );
  }
}

function toggleFocus() {
  let messageBox = document.querySelector(".message-box");
  messageBox.classList.toggle("focus");
}

function keydownEventHandler(event) {
  var quill = this.quill;
  var toggleFormatChars = function(char) {
    var selection = quill.getSelection();
    var length = selection.length;
    if (length < 1) return;

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
  };

  var ret = true;
  var char = "";
  if (event.ctrlKey || event.metaKey) {
    switch (event.keyCode) {
      case 66: // ctrl+B or ctrl+b
      case 98:
        ret = false;
        char = "*";
        break;
      case 73: // ctrl+I or ctrl+i
      case 105:
        ret = false;
        char = "_";
        break;
      case 85: // ctrl+U or ctrl+u
      case 117:
        ret = false;
        break;
      default:
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
}

export default ContentFooter;
