import React from "react";
import "./ContentEditor.css";
import QuillService from "services/QuillService";
import ContentEditorUserMention from "./ContentEditor.UserMention";
import { Icon, Popup } from "semantic-ui-react";
import UploadFileForm from "modalForms/UploadFileForm";
import Modal from "modals/Modal";
import data from "emoji-mart/data/messenger.json";
import { EmojiConvertor } from "emoji-js";
import { NimblePicker } from "emoji-mart";
import "lib/emoji-mart.css";
import "lib/emoji.css";
import { toast } from "react-toastify";
import ApiService from "services/ApiService";

class ContentEditor extends React.Component {
  constructor(props) {
    super(props);

    this.apiService = new ApiService(props);
    this.isSendingTypingMessage = false;
    this.keydownEventHandler = this.keydownEventHandler.bind(this);
    this.onTextChange = this.onTextChange.bind(this);
    this.onMentionSelecting = this.onMentionSelecting.bind(this);
    this.onMentionSelected = this.onMentionSelected.bind(this);
    this.onSubmitMessage = this.onSubmitMessage.bind(this);
    this.onToggleFocus = this.onToggleFocus.bind(this);
    this.onFileUploaded = this.onFileUploaded.bind(this);
    this.onCloseUploadFile = this.onCloseUploadFile.bind(this);
    this.onUploadFileButtonClicked = this.onUploadFileButtonClicked.bind(this);
    this.onMentionButtonClicked = this.onMentionButtonClicked.bind(this);
    this.openMention = this.openMention.bind(this);
    this.closeMention = this.closeMention.bind(this);
    this.onEmojiButtonClicked = this.onEmojiButtonClicked.bind(this);
    this.closeEmoji = this.closeEmoji.bind(this);
    this.addEmoji = this.addEmoji.bind(this);
    this.onEmojiKeyup = this.onEmojiKeyup.bind(this);
    this.onEditorPaste = this.onEditorPaste.bind(this);
    this.userList = [];

    this.state = {
      isMentionOpen: false,
      mentionUserList: [],
      highlightMentionUserIndex: 0,
      isUploadFileModalOpen: false
    };
  }

  componentDidMount() {
    this.containerElement = document.getElementById(this.props.containerId);

    this.quillService = new QuillService({
      editorContainerSelector: `#${this.props.containerId} #messageEditor`,
      onSubmitMessage: this.onSubmitMessage,
      onTextChange: this.onTextChange,
      onToggleFocus: this.onToggleFocus
    });

    this.initMention();
    this.registerEventHandlers();
    this.fecthUsers();
  }

  componentWillUnmount() {
    this.unregisterEventHandlers();
  }

  fecthUsers() {
    this.apiService.fetch("/api/users").then(users => {
      let currentUserId = this.props.userProfile.id;
      this.userList = users.filter(u => u.id !== currentUserId);
    });
  }

  initMention() {
    this.mention = {
      mentionAtIndex: undefined,
      filterName: "",
      isSelecting: false,
      isSelectionInserted: false,
      isSelected: false, // true after clicked or enter pressed
      mentionRegex: /<span data-user-id="([0-9]+)" class="mentioned-user">@.+?<\/span>/g
    };
  }

  openMention() {
    this.setState(
      {
        isMentionOpen: true
      },
      () => {
        document.addEventListener("click", this.closeMention);
      }
    );
  }

  closeMention(event) {
    let mentionContainer = this.containerElement.querySelector(
      ".user-mention-container"
    );
    if (event && mentionContainer.contains(event.target)) return;
    this.setState({ isMentionOpen: false }, () => {
      document.removeEventListener("click", this.closeMention);
    });
  }

  onSubmitMessage(message, pureText) {
    let emojiRegex = /<span class="emoji-container">/;
    if (
      !pureText &&
      !this.mention.mentionRegex.test(message) &&
      !emojiRegex.test(message)
    )
      return;
    var mentionUserIds = [];
    var groups;
    this.mention.mentionRegex.lastIndex = 0;
    while ((groups = this.mention.mentionRegex.exec(message)) !== null) {
      mentionUserIds.push(+groups[1]);
    }

    this.apiService
      .fetch(`/api/messages/${this.props.section}/${this.props.id}`, {
        method: "POST",
        body: JSON.stringify({
          message: message,
          mentionUserIds: mentionUserIds
        })
      })
      .catch(error => {
        toast.error(`Send message failed: ${error}`);
      });
  }

  onTextChange(event) {
    if (!this.mention.isSelecting) {
      let insertOp = event.ops.find(o => !!o.insert);
      let deleteOp = event.ops.find(o => !!o.delete);
      if (
        insertOp &&
        insertOp.insert &&
        typeof insertOp.insert === "string" &&
        insertOp.insert.trim() === "@" &&
        !this.state.isMentionOpen
      ) {
        this.setMentionList(this.userList.slice(0, 8));
        this.openMention();
        this.mention.mentionAtIndex = this.quillService.getCursorIndex() - 1;
        return;
      } else if (
        deleteOp &&
        this.state.isMentionOpen &&
        !this.mention.filterName
      ) {
        this.onMentionFinish();
        return;
      }

      if (this.state.isMentionOpen) {
        if (this.mention.isSelectionInserted) {
          // type after mention selection inserted
          this.onMentionFinish();
        } else {
          insertOp
            ? (this.mention.filterName += insertOp.insert)
            : (this.mention.filterName = this.mention.filterName.substring(
                0,
                this.mention.filterName.length - deleteOp.delete
              ));
          let mentionList = this.userList
            .filter(
              u =>
                u.displayName
                  .toLowerCase()
                  .indexOf(this.mention.filterName.toLowerCase()) > -1
            )
            .slice(0, 8);
          this.setMentionList(mentionList);

          if (
            mentionList.length === 0 &&
            insertOp &&
            insertOp.insert.trim() === ""
          )
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

  setMentionList(list) {
    this.setState({
      mentionUserList: list,
      highlightMentionUserIndex: 0
    });
  }

  sendTypingMessage() {
    var url = this.props.isChannel
      ? `/api/channels/notifyTyping`
      : `/api/conversations/notifyTyping`;
    this.apiService.fetch(url, {
      method: "POST",
      body: JSON.stringify(this.props.id)
    });
  }

  formatMentionUser(user) {
    // TODO escape displayName, using html-entities
    return `<span data-user-id="${user.id}" class="mentioned-user">@${
      user.displayName
    }</span>`;
  }

  onMentionSelecting(id) {
    if (
      this.state.isMentionOpen &&
      !!this.mention.filterName &&
      !this.mention.isSelected
    )
      return;
    this.mention.isSelecting = true;
    let user = this.userList.find(u => u.id === id);
    this.quillService.deleteText(this.mention.mentionAtIndex, 1); // delete the previous @ or @userName
    this.quillService.insertHtml(
      this.mention.mentionAtIndex,
      this.quillService.getSpanTagName(),
      this.formatMentionUser(user)
    );
    this.quillService.insertText(this.mention.mentionAtIndex + 1, " "); // so that the cursor becomes visible
    this.quillService.setCursorIndex(this.mention.mentionAtIndex + 2, 0);
    this.mention.isSelecting = false;
    this.mention.isSelectionInserted = true;
  }

  onMentionSelected(id) {
    this.mention.isSelected = true;
    this.onMentionSelecting(id);
    if (this.mention.filterName)
      this.quillService.deleteText(
        this.mention.mentionAtIndex + 2,
        this.mention.filterName.length
      );
    this.onMentionFinish();
  }

  onMentionFinish() {
    this.initMention();
    this.closeMention();
    let editor = this.containerElement.querySelector(".ql-editor");
    editor.focus();
  }

  onMentionButtonClicked(event) {
    let editor = this.containerElement.querySelector(".ql-editor");
    editor.focus();
    let index = this.quillService.getCursorIndex();
    this.quillService.setCursorIndex(index, 0);
    this.quillService.insertText(index, "@");
  }

  registerEventHandlers() {
    var editorContainer = this.containerElement.querySelector("#messageEditor");
    editorContainer.addEventListener("keydown", this.keydownEventHandler, true); // true - event capturing phase
  }

  unregisterEventHandlers() {
    let editorContainer = this.containerElement.querySelector("#messageEditor");
    editorContainer.removeEventListener(
      "keydown",
      this.keydownEventHandler,
      true
    ); // true - event capturing phase

    this.quillService.unregisterEventHandlers();
  }

  onToggleFocus() {
    let messageBox = this.containerElement.querySelector(".message-box");
    messageBox.classList.toggle("focus");
  }

  keydownEventHandler(event) {
    var handled = false;

    if (
      this.state.isMentionOpen &&
      !event.ctrlKey &&
      !event.shiftKey &&
      !event.altKey
    ) {
      let index = this.state.highlightMentionUserIndex;
      switch (event.keyCode) {
        case 40: // arrow down
          this.setState({
            highlightMentionUserIndex:
              index + 1 > this.state.mentionUserList.length - 1 ? 0 : index + 1
          });
          handled = true;
          break;
        case 38: // arrow up
          this.setState({
            highlightMentionUserIndex:
              index - 1 < 0 ? this.state.mentionUserList.length - 1 : index - 1
          });
          handled = true;
          break;
        case 13: // enter
        case 9: // tab
          let user = this.state.mentionUserList[
            this.state.highlightMentionUserIndex
          ];
          if (!!user) {
            this.onMentionSelected(user.id);
            handled = true;
          }
          break;
        case 27: // esc
          this.onMentionFinish();
          handled = true;
          break;
        default:
          break;
      }
    }

    if (handled === true) {
      event.stopPropagation();
      event.preventDefault();
      return false;
    }

    return true;
  }

  onEditorPaste(event) {
    var imageRegex = /^image\/(gif|jpg|jpeg|tiff|png)$/i;
    var items = event.clipboardData.items;
    for (let i = 0; i < items.length; i++) {
      if (imageRegex.test(items[i].type)) {
        let file = items[i].getAsFile();
        if (file) {
          this.openUploadFile([file]);
          event.stopPropagation();
          event.preventDefault();
          return;
        }
      }
    }
  }

  onFileUploaded(event) {
    if (event.currentTarget.files && event.currentTarget.files[0]) {
      this.openUploadFile(event.currentTarget.files);
    }
  }

  openUploadFile(files) {
    this.uploadFiles = files;
    this.setState({
      isUploadFileModalOpen: true
    });
  }

  onCloseUploadFile() {
    this.containerElement.querySelector("#uploadFile").value = "";
    this.setState({
      isUploadFileModalOpen: false
    });
  }

  onUploadFileButtonClicked() {
    this.containerElement.querySelector("#uploadFile").click();
  }

  onEmojiButtonClicked() {
    this.setState({ isEmojiOpen: true }, () => {
      document.addEventListener("click", this.closeEmoji);
      document.addEventListener("keyup", this.onEmojiKeyup);
    });
  }

  onEmojiKeyup(event) {
    if (
      !event.ctrlKey &&
      !event.shiftKey &&
      !event.altKey &&
      event.keyCode === 27
    ) {
      // esc pressed
      this.closeEmoji();
    }
  }

  closeEmoji(event) {
    let emojiPicker = document.querySelector(".emoji-picker");
    if (event && emojiPicker.contains(event.target)) return;
    this.setState({ isEmojiOpen: false }, () => {
      document.removeEventListener("click", this.closeEmoji);
      document.removeEventListener("keyup", this.onEmojiKeyup);
    });
  }

  addEmoji(event) {
    var emoji = new EmojiConvertor();
    emoji.img_sets.google.path =
      "https://unpkg.com/emoji-datasource-google@4.0.4/img/google/sheets-256/";
    emoji.img_sets.google.sheet =
      "https://unpkg.com/emoji-datasource-google@4.0.4/img/google/sheets-256/64.png";
    emoji.img_set = "google";
    emoji.use_sheet = true;
    var imgHtml = emoji.replace_colons(event.colons);

    const index = this.quillService.getCursorIndex();
    this.quillService.insertHtml(
      index,
      this.quillService.getSpanTagName(),
      `<span class="emoji-container">${imgHtml}</span>`
    );
    this.quillService.insertText(index + 1, " "); // so that the cursor becomes visible
    this.quillService.setCursorIndex(index + 2, 0);
    let editor = this.containerElement.querySelector(".ql-editor");
    editor.focus();
    this.closeEmoji();
  }

  render() {
    return (
      <div>
        <form id="messageForm" method="post">
          {this.state.isMentionOpen && (
            <div className="user-mention-container">
              <ContentEditorUserMention
                userList={this.state.mentionUserList}
                onMentionSelecting={this.onMentionSelecting}
                onMentionSelected={this.onMentionSelected}
                highlightItemIndex={this.state.highlightMentionUserIndex}
                filterName={this.mention.filterName}
              />
            </div>
          )}
          <div className="message-box">
            <div className="message-editor-container">
              <div id="messageEditor" onPaste={this.onEditorPaste} />
            </div>
            <div className="message-box-buttons">
              <div className="message-box-button">
                <span onClick={this.onMentionButtonClicked}>
                  <Popup
                    trigger={<Icon name="at" className="icon-mention" />}
                    content="Mention user"
                    inverted
                    position="top center"
                    size="tiny"
                  />
                </span>
              </div>
              <div className="message-box-button">
                <input
                  id="uploadFile"
                  type="file"
                  multiple
                  onChange={this.onFileUploaded}
                />
                <span onClick={this.onUploadFileButtonClicked}>
                  <Popup
                    trigger={
                      <Icon name="paperclip" className="icon-paperclip" />
                    }
                    content="Send file"
                    inverted
                    position="top center"
                    size="tiny"
                  />
                </span>
                {this.state.isUploadFileModalOpen && (
                  <Modal onClose={this.onCloseUploadFile}>
                    <UploadFileForm
                      files={Array.from(this.uploadFiles)}
                      onUploaded={this.onCloseUploadFile}
                      {...this.props}
                    />
                  </Modal>
                )}
              </div>
              <div className="message-box-button">
                <span onClick={this.onEmojiButtonClicked}>
                  <Popup
                    trigger={
                      <Icon name="smile outline" className="icon-emoji" />
                    }
                    content="Add emoji"
                    inverted
                    position="top center"
                    size="tiny"
                  />
                </span>
              </div>
            </div>
            {this.state.isEmojiOpen && (
              <div className="emoji-picker">
                <NimblePicker
                  set="google"
                  data={data}
                  onSelect={this.addEmoji}
                />
              </div>
            )}
          </div>
        </form>
      </div>
    );
  }
}

export default ContentEditor;
