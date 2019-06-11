import React from "react";
import "./ContentFooter.css";
import Quill from "quill";
import AuthService from "services/AuthService";
import UserMention from "./UserMention";
import { tsImportEqualsDeclaration } from "@babel/types";

class ContentFooter extends React.Component {
  constructor(props) {
    super(props);

    this.quill = {};
    this.authService = new AuthService(props);
    this.keydownEventHandler = this.keydownEventHandler.bind(this);
    this.onTextChange = this.onTextChange.bind(this);
    this.onOtherUserTyping = this.onOtherUserTyping.bind(this);
    this.onMentionSelecting = this.onMentionSelecting.bind(this);
    this.onMentionSelected = this.onMentionSelected.bind(this);
    this.isSendingTypingMessage = false;
    this.mention = {
      mentionAtIndex: undefined,
      typingMentionName: "",
      isTypingMention: false,
      isSelecting: false,
      isSelectionInserted: false,
      mentionRegex: new RegExp(
        '^<span data-user-id="([0-9]+)" class="mentioned-user">@.+</span>$'
      )
    };

    if (props.hubConnection) {
      props.hubConnection.on("UserTyping", this.onOtherUserTyping);
    }

    this.otherTypingNames = [];
    this.userList = [];

    this.state = {
      showOtherTypingInfo: false,
      otherTypingUserName: undefined,
      showMention: false,
      mentionUserList: []
    };
  }

  componentDidMount() {
    this.initQuill();
    this.registerEventHandlers();
    this.fecthUsers();
  }

  componentWillUnmount() {
    this.unregisterEventHandlers();
  }

  componentDidUpdate(prevProps) {
    if (
      this.props.isChannel !== prevProps.isChannel ||
      this.props.id !== prevProps.id
    ) {
      this.otherTypingNames = [];
      this.toggleOtherTypingInfo(false);
    }
  }

  fecthUsers() {
    this.authService.fetch("/api/users").then(users => {
      let currentUserId = this.props.userProfile.id;
      this.userList = users.filter(u => u.id !== currentUserId);
    });
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
    class MentionTag extends Embed {
      static create(value) {
        let node = super.create(value);
        node.innerHTML = `${value}`;
        return node;
      }
    }
    MentionTag.blotName = "mentionTag";
    MentionTag.tagName = "span";

    Quill.register({
      "formats/mentionTag": MentionTag
    });
  }

  initQuill() {
    this.configQuill();
    var submitMessage = function() {
      var text = this.quill.getText().trim();
      if (!text) return;

      var message = this.quill.root.innerHTML;

      this.quill.root.innerHTML = "";

      this.authService.fetch(
        `/api/messages/${this.props.section}/${this.props.id}`,
        {
          method: "POST",
          body: JSON.stringify(message)
        }
      );
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

    this.quill.on("text-change", this.onTextChange);
  }

  onTextChange(event) {
    if (!this.mention.isSelecting) {
      let insertOp = event.ops.find(o => !!o.insert);
      let deleteOp = event.ops.find(o => !!o.delete);
      if (
        insertOp &&
        insertOp.insert === "@" &&
        !this.mention.isTypingMention
      ) {
        this.setState({
          mentionUserList: this.userList.slice(0, 8),
          showMention: true
        });
        this.mention.isTypingMention = true;
        this.mention.mentionAtIndex = this.quill.getSelection().index - 1;
        return;
      }
      if (
        deleteOp &&
        this.mention.isTypingMention &&
        !this.mention.typingMentionName
      ) {
        this.onMentionFinish();
        return;
      }

      if (this.mention.isTypingMention) {
        if (this.mention.isSelectionInserted) {
          // type after mention selection inserted
          this.onMentionFinish();
        } else {
          insertOp
            ? (this.mention.typingMentionName += insertOp.insert)
            : (this.mention.typingMentionName = this.mention.typingMentionName.substring(
                0,
                this.mention.typingMentionName.length - deleteOp.delete
              ));
          let mentionList = this.userList
            .filter(
              u =>
                u.displayName
                  .toLowerCase()
                  .indexOf(this.mention.typingMentionName.toLowerCase()) > -1
            )
            .slice(0, 8);
          this.setState({
            mentionUserList: mentionList
          });
          if (mentionList.length === 0 && insertOp && insertOp.insert === " ")
            this.onMentionFinish();
        }
        return;
      }
    }

    if (this.isSendingTypingMessage === false) {
      this.isSendingTypingMessage = true;
      this.sendTypingMessage();
      setTimeout(() => {
        this.isSendingTypingMessage = false;
      }, 10000); // throttle it to once in 10 secs
    }
  }

  formatMentionUser(user) {
    // TODO escape displayName, using html-entities
    return `<span data-user-id="${user.id}" class="mentioned-user">@${
      user.displayName
    }</span>`;
  }

  onMentionSelecting(id) {
    if (this.mention.isTypingMention && !!this.mention.typingMentionName)
      return;
    this.mention.isSelecting = true;
    let user = this.userList.find(u => u.id === id);
    this.quill.deleteText(this.mention.mentionAtIndex, 1); // delete the previous @ or @userName
    this.quill.insertEmbed(
      this.mention.mentionAtIndex,
      "mentionTag",
      this.formatMentionUser(user)
    );
    this.quill.setSelection(this.mention.mentionAtIndex + 1, 0);
    this.mention.isSelecting = false;
    this.mention.isSelectionInserted = true;
  }

  onMentionSelected(id) {
    this.mention.isTypingMention = false;
    this.onMentionSelecting(id);
    if (this.mention.typingMentionName)
      this.quill.deleteText(
        this.mention.mentionAtIndex + 1,
        this.mention.typingMentionName.length
      );
    this.onMentionFinish();
  }

  onMentionFinish() {
    this.mention.isTypingMention = false;
    this.mention.isSelecting = false;
    this.mention.isSelectionInserted = false;
    this.mention.typingMentionName = "";
    this.setState({ showMention: false });
    let editor = document.querySelector(".ql-editor");
    editor.focus();
  }

  sendTypingMessage() {
    var url = this.props.isChannel
      ? `/api/channels/notifyTyping`
      : `/api/conversations/notifyTyping`;
    this.authService.fetch(url, {
      method: "POST",
      body: JSON.stringify(this.props.id)
    });
  }

  toggleOtherTypingInfo(show) {
    let names =
      this.otherTypingNames.length > 2
        ? "Multiple users"
        : this.otherTypingNames.join(", ");

    this.setState({
      showOtherTypingInfo: show,
      otherTypingUserName: names
    });
  }

  onOtherUserTyping(name, isChannel, id) {
    if (isChannel === this.props.isChannel && id === this.props.id) {
      this.otherTypingNames.push(name);
      this.toggleOtherTypingInfo(true);

      setTimeout(() => {
        let index = this.otherTypingNames.indexOf(name);
        this.otherTypingNames.splice(index, 1);
        this.toggleOtherTypingInfo(this.otherTypingNames.length > 0);
      }, 10000);
    }
  }

  registerEventHandlers() {
    let editor = document.querySelector(".ql-editor");
    editor.addEventListener("focus", this.toggleFocus);
    editor.addEventListener("blur", this.toggleFocus);

    var messageBox = document.querySelector(".message-box");
    messageBox.addEventListener("keydown", this.keydownEventHandler, true); // true - event capturing phase
  }

  unregisterEventHandlers() {
    var messageBox = document.querySelector(".message-box");
    let editor = document.querySelector(".ql-editor");
    messageBox.removeEventListener("keydown", this.keydownEventHandler, true); // true - event capturing phase
    editor.removeEventListener("focus", this.toggleFocus);
    editor.removeEventListener("blur", this.toggleFocus);
    this.quill.off("text-change", this.onTextChange);
  }

  toggleFocus() {
    let messageBox = document.querySelector(".message-box");
    messageBox.classList.toggle("focus");
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
      this.toggleFormatChars(char);
    }

    if (ret === false) {
      event.stopPropagation();
      event.preventDefault();
      return false;
    }

    return ret;
  }

  render() {
    return (
      <div className="footer">
        <form id="messageForm" method="post">
          {this.state.showMention && (
            <div className="user-mention-container">
              <UserMention
                userList={this.state.mentionUserList}
                onMentionSelecting={this.onMentionSelecting}
                onMentionSelected={this.onMentionSelected}
              />
            </div>
          )}
          <div className="message-box">
            <div id="messageEditor" />
          </div>
        </form>
        {this.state.showOtherTypingInfo && (
          <div className="message-typing-info">
            <span>{this.state.otherTypingUserName}</span>{" "}
            {this.otherTypingNames.length > 1 ? "are" : "is"} typing
          </div>
        )}
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

export default ContentFooter;
