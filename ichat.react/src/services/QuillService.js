import Quill from "quill";

class QuillService {
  constructor(params) {
    this.params = params;
    this.quill = undefined;
    this.spanTagName = "spanTag";

    initQuill = initQuill.bind(this);
    configQuill = configQuill.bind(this);
    registerEventHandlers = registerEventHandlers.bind(this);
    toggleFormatChars = toggleFormatChars.bind(this);
    keydownEventHandler = keydownEventHandler.bind(this);
    this.unregisterEventHandlers = this.unregisterEventHandlers.bind(this);

    initQuill();
  }

  getCursorIndex() {
    return this.quill.getSelection().index;
  }

  setCursorIndex(index) {
    this.quill.setSelection(index, 0);
  }

  getSpanTagName() {
    return this.spanTagName;
  }

  deleteText(index, length) {
    this.quill.deleteText(index, length);
  }

  insertHtml(index, tagName, html) {
    this.quill.insertEmbed(index, tagName, html);
  }

  unregisterEventHandlers() {
    let editorContainer = document.querySelector(this.params.editorSelector);
    editorContainer.removeEventListener(
      "keydown",
      keydownEventHandler,
      true
    ); // true - event capturing phase
    let editor = document.querySelector(".ql-editor");
    editor.removeEventListener("focus", this.toggleFocus);
    editor.removeEventListener("blur", this.toggleFocus);
    this.quill.off("text-change", this.params.onTextChange);
  }
}

// private to this class

function configQuill() {
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

  var Embed = Quill.import("blots/embed");
  class SpanTag extends Embed {
    static create(value) {
      let node = super.create(value);
      node.innerHTML = `${value}`;
      return node;
    }
  }
  SpanTag.blotName = this.spanTagName;
  SpanTag.tagName = "span";

  Quill.register({
    [`formats/${this.spanTagName}`]: SpanTag
  });
}

function initQuill() {
  configQuill();
  var submitMessage = function() {
    var text = this.quill.getText().trim();
    var message = this.quill.root.innerHTML;
    this.quill.root.innerHTML = "";
    this.params.onSubmitMessage(message, text);
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

  this.quill = new Quill(this.params.editorSelector, {
    theme: "snow",
    placeholder: `Type your message...`,
    modules: {
      keyboard: {
        bindings: bindings
      }
    }
  });

  registerEventHandlers();
}

function registerEventHandlers() {
  this.quill.on("text-change", this.params.onTextChange);

  let editor = document.querySelector(".ql-editor");
  editor.addEventListener("focus", this.params.toggleFocus);
  editor.addEventListener("blur", this.params.toggleFocus);

  var editorContainer = document.querySelector(this.params.editorSelector);
  editorContainer.addEventListener("keydown", keydownEventHandler, true); // true - event capturing phase
}

function toggleFormatChars(char) {
  var selection = this.quill.getSelection();
  var length = selection.length;
  if (length < 1) return;

  var start = selection.index;
  var text = this.quill.getText(start, length);
  var newText;
  if (text.length > 2 && text.startsWith(char) && text.endsWith(char)) {
    // remove char
    newText = text.substr(1, length - 2);
    this.quill.deleteText(start, length);
    this.quill.insertText(start, newText);
    this.quill.setSelection(start, length - 2);
  } else {
    // add char
    newText = char + text + char;
    this.quill.deleteText(start, length);
    this.quill.insertText(start, newText);
    this.quill.setSelection(start, length + 2);
  }
}

function keydownEventHandler(event) {
  var handled = false;
  var char = "";
  if (event.ctrlKey || event.metaKey) {
    switch (event.keyCode) {
      case 66: // ctrl+B or ctrl+b
      case 98:
        handled = true;
        char = "*";
        break;
      case 73: // ctrl+I or ctrl+i
      case 105:
        handled = true;
        char = "_";
        break;
      case 85: // ctrl+U or ctrl+u
      case 117:
        handled = true;
        break;
      default:
        break;
    }
  }
  if (!!char) {
    toggleFormatChars(char);
  }

  if (handled === true) {
    event.stopPropagation();
    event.preventDefault();
    return false;
  }

  return true;
}

export default QuillService;
