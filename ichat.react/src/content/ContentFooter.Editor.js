import React from "react";
import "./ContentFooter.Editor.css";
import QuillService from "services/QuillService";
import AuthService from "services/AuthService";
import ContentFooterEditorUserMention from "./ContentFooter.Editor.UserMention";
import { Icon } from "semantic-ui-react";
import UploadFileForm from "modalForms/UploadFileForm";
import Modal from "modals/Modal";

class ContentFooterEditor extends React.Component {
  constructor(props) {
    super(props);

    this.authService = new AuthService(props);
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
    this.userList = [];

    this.state = {
      isMentionOpen: false,
      mentionUserList: [],
      highlightMentionUserIndex: 0,
      isUploadFileModalOpen: false
    };
  }

  componentDidMount() {
    this.quillService = new QuillService({
      editorContainerSelector: "#messageEditor",
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
    this.authService.fetch("/api/users").then(users => {
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
      mentionRegex: /<span data-user-id="([0-9]+)" class="mentioned-user">@.+?<\/span>/g
    };
  }

  onSubmitMessage(message, pureText) {
    if (!pureText && !this.mention.mentionRegex.test(message)) return;
    var mentionUserIds = [];
    var groups;
    this.mention.mentionRegex.lastIndex = 0;
    while ((groups = this.mention.mentionRegex.exec(message)) !== null) {
      mentionUserIds.push(+groups[1]);
    }

    this.authService.fetch(
      `/api/messages/${this.props.section}/${this.props.id}`,
      {
        method: "POST",
        body: JSON.stringify({
          message: message,
          mentionUserIds: mentionUserIds
        })
      }
    );
  }

  onTextChange(event) {
    if (!this.mention.isSelecting) {
      let insertOp = event.ops.find(o => !!o.insert);
      let deleteOp = event.ops.find(o => !!o.delete);
      if (insertOp && insertOp.insert === "@" && !this.state.isMentionOpen) {
        this.setMentionList(this.userList.slice(0, 8));
        this.setState({
          isMentionOpen: true
        });
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
    this.authService.fetch(url, {
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
    if (this.state.isMentionOpen && !!this.mention.filterName) return;
    this.mention.isSelecting = true;
    let user = this.userList.find(u => u.id === id);
    this.quillService.deleteText(this.mention.mentionAtIndex, 1); // delete the previous @ or @userName
    this.quillService.insertHtml(
      this.mention.mentionAtIndex,
      this.quillService.getSpanTagName(),
      this.formatMentionUser(user)
    );
    this.quillService.setCursorIndex(this.mention.mentionAtIndex + 1, 0);
    this.mention.isSelecting = false;
    this.mention.isSelectionInserted = true;
  }

  onMentionSelected(id) {
    this.setState({ isMentionOpen: false });
    this.onMentionSelecting(id);
    if (this.mention.filterName)
      this.quillService.deleteText(
        this.mention.mentionAtIndex + 1,
        this.mention.filterName.length
      );
    this.onMentionFinish();
  }

  onMentionFinish() {
    this.initMention();
    this.setState({ isMentionOpen: false });
    let editor = document.querySelector(".ql-editor");
    editor.focus();
  }

  registerEventHandlers() {
    var editorContainer = document.querySelector("#messageEditor");
    editorContainer.addEventListener("keydown", this.keydownEventHandler, true); // true - event capturing phase
  }

  unregisterEventHandlers() {
    let editorContainer = document.querySelector("#messageEditor");
    editorContainer.removeEventListener(
      "keydown",
      this.keydownEventHandler,
      true
    ); // true - event capturing phase

    this.quillService.unregisterEventHandlers();
  }

  onToggleFocus() {
    let messageBox = document.querySelector(".message-box");
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

  onFileUploaded(event) {
    if (event.currentTarget.files && event.currentTarget.files[0]) {
      this.uploadFiles = event.currentTarget.files;
      this.setState({
        isUploadFileModalOpen: true
      });
    }
  }

  onCloseUploadFile() {
    document.querySelector("#uploadFile").value = "";
    this.setState({
      isUploadFileModalOpen: false
    });
  }

  onUploadFileButtonClicked() {
    document.querySelector("#uploadFile").click();
  }

  render() {
    return (
      <div>
        <form id="messageForm" method="post">
          {this.state.isMentionOpen && (
            <div className="user-mention-container">
              <ContentFooterEditorUserMention
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
              <div id="messageEditor" />
            </div>
            <div className="message-box-buttons">
              <input
                id="uploadFile"
                type="file"
                multiple
                onChange={this.onFileUploaded}
              />
              <span
                className="message-box-button"
                onClick={this.onUploadFileButtonClicked}
                title="Send file"
              >
                <Icon name="paperclip" className="icon-paperclip" />
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
          </div>
        </form>
      </div>
    );
  }
}

export default ContentFooterEditor;
