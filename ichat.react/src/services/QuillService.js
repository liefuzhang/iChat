import Quill from "quill";

class QuillService {
  constructor(params) {
    this.params = params;
    this.quill = undefined;
    this.spanTagName = "spanTag";

    this.keydownEventHandler = this.keydownEventHandler.bind(this);

    this.initQuill();
  }

  getCursorIndex() {
    return this.quill.selection.savedRange.index;
  }

  setCursorIndex(index) {
    this.quill.setSelection(index, 0);
  }

  getSpanTagName() {
    return this.spanTagName;
  }

  insertText(index, string) {
    this.quill.insertText(index, string);
  }

  getText() {
    return this.quill.getText().trim();
  }

  deleteText(index, length) {
    this.quill.deleteText(index, length);
  }

  insertHtml(index, tagName, html) {
    this.quill.insertEmbed(index, tagName, html);
  }

  unregisterEventHandlers() {
    let editorContainer = document.querySelector(
      this.params.editorContainerSelector
    );
    editorContainer.removeEventListener("keydown", this.keydownEventHandler, true); // true - event capturing phase
    let editor = document.querySelector(".ql-editor");
    editor.removeEventListener("focus", this.params.onToggleFocus);
    editor.removeEventListener("blur", this.params.onToggleFocus);
    this.quill.off("text-change", this.params.onTextChange);
  }

  configQuill() {
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
    SpanTag.className = "span-tag";

    Quill.register({
      [`formats/${this.spanTagName}`]: SpanTag
    });
  }

  initQuill() {
    this.configQuill();
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

    this.quill = new Quill(this.params.editorContainerSelector, {
      theme: "snow",
      placeholder: `Type your message...`,
      modules: {
        keyboard: {
          bindings: bindings
        }
      }
    });

    this.registerEventHandlers();
  }

  registerEventHandlers() {
    this.quill.on("text-change", this.params.onTextChange);

    let editor = document.querySelector(".ql-editor");
    editor.addEventListener("focus", this.params.onToggleFocus);
    editor.addEventListener("blur", this.params.onToggleFocus);

    var editorContainer = document.querySelector(
      this.params.editorContainerSelector
    );
    editorContainer.addEventListener("keydown", this.keydownEventHandler, true); // true - event capturing phase
  }

  toggleFormatChars(char) {
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

  keydownEventHandler(event) {
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
      this.toggleFormatChars(char);
    }

    if (handled === true) {
      event.stopPropagation();
      event.preventDefault();
      return false;
    }

    return true;
  }
}

export default QuillService;
