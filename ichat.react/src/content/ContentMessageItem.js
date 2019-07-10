import React from "react";
import { Icon, Popup, Confirm } from "semantic-ui-react";
import "./ContentMessageItem.css";
import ApiService from "services/ApiService";
import { toast } from "react-toastify";
import EmojiPicker from "components/EmojiPicker";
import ContentMessageItemUserReactions from "./ContentMessageItem.UserReactions";
import ContentMessageItemUrlPreview from "./ContentMessageItem.UrlPreview";
import ContentMessageItemFileItem from "./ContentMessageItem.FileItem";

class ContentMessageItem extends React.Component {
  constructor(props) {
    super(props);

    this.apiService = new ApiService(props);
    this.onDeleteMessageConfirmed = this.onDeleteMessageConfirmed.bind(this);
    this.onEmojiColonsAdded = this.onEmojiColonsAdded.bind(this);
    this.onEmojiColonsRemoved = this.onEmojiColonsRemoved.bind(this);
    this.onEmojiClose = this.onEmojiClose.bind(this);

    this.state = {
      isDeleteMessageConfirmOpen: false
    };
  }

  onHoverMessageItem(event) {
    event.currentTarget.classList.add("message-hover");
  }

  onLeaveMessageItem(event) {
    event.currentTarget.classList.remove("message-hover");
  }

  onDeleteMessageConfirmed(messageId) {
    this.setState({
      isDeleteMessageConfirmOpen: false
    });

    this.apiService
      .fetch(
        `/api/messages/${this.props.section}/${this.props.id}/deleteMessage`,
        {
          method: "POST",
          body: JSON.stringify(messageId)
        }
      )
      .catch(error => {
        toast.error(`Delete message failed: ${error}`);
      });
  }

  onEmojiColonsAdded(colons) {
    this.apiService
      .fetch(
        `/api/messages/${this.props.section}/${this.props.id}/addReaction/${
          this.props.message.id
        }`,
        {
          method: "POST",
          body: JSON.stringify(colons)
        }
      )
      .catch(error => {
        toast.error(`Add reaction failed: ${error}`);
      });
  }

  onEmojiColonsRemoved(colons) {
    this.apiService
      .fetch(
        `/api/messages/${this.props.section}/${this.props.id}/removeReaction/${
          this.props.message.id
        }`,
        {
          method: "POST",
          body: JSON.stringify(colons)
        }
      )
      .catch(error => {
        toast.error(`Remove reaction failed: ${error}`);
      });
  }

  onEmojiClose() {
    let currentMessage = document.querySelector(".message-hover");
    currentMessage && currentMessage.classList.remove("message-hover");
  }

  render() {
    let message = this.props.message;

    return (
      <div
        className={
          "message-item-common-container message-item-container" +
          (message.isConsecutiveMessage ? " following-message" : "") +
          (message.isSystemMessage ? " system-message" : "")
        }
        key={message.id}
        onMouseOver={this.onHoverMessageItem}
        onMouseLeave={this.onLeaveMessageItem}
      >
        <img className="user-identicon" src={message.sender.identiconPath} />
        <div className="message-item">
          <div className="message-title">
            <b>{message.sender.displayName}</b>
            <span className="message-time">{message.timeString}</span>
          </div>
          <div className="message-content-container">
            <div className="message-content-time">
              <span className="message-time">{message.timeString}</span>
            </div>
            <div className="message-toolbar">
              <div className="message-toolbar-item">
                <div className="message-toolbar-item-content">
                  <EmojiPicker
                    onEmojiColonsAdded={this.onEmojiColonsAdded}
                    onClose={this.onEmojiClose}
                    tooltipText="Add reaction"
                  />
                </div>
              </div>
              {message.sender.id === this.props.userProfile.id &&
                !message.hasFileAttachments &&
                !message.isSystemMessage && (
                  <div className="message-toolbar-item">
                    <Popup
                      trigger={
                        <div
                          className="message-toolbar-item-content"
                          onClick={() =>
                            this.props.onEditMessageClicked(message.id)
                          }
                        >
                          <Icon name="edit outline" />
                        </div>
                      }
                      content="Edit message"
                      inverted
                      position="top center"
                      size="tiny"
                    />
                  </div>
                )}
              {message.sender.id === this.props.userProfile.id &&
                !message.isSystemMessage && (
                  <div className="message-toolbar-item">
                    <Popup
                      trigger={
                        <div
                          className="message-toolbar-item-content"
                          onClick={() =>
                            this.setState({ isDeleteMessageConfirmOpen: true })
                          }
                        >
                          <Icon name="trash alternate outline" />
                        </div>
                      }
                      content="Delete message"
                      inverted
                      position="top center"
                      size="tiny"
                    />
                    <Confirm
                      open={this.state.isDeleteMessageConfirmOpen}
                      header="Delete message"
                      content="Are you sure you want to delete this message? This cannot be undone."
                      onCancel={() =>
                        this.setState({
                          isDeleteMessageConfirmOpen: false
                        })
                      }
                      onConfirm={() =>
                        this.onDeleteMessageConfirmed(message.id)
                      }
                    />
                  </div>
                )}
            </div>
            <div className="message-content">
              {!message.hasFileAttachments && (
                <div>
                  <RawMessage
                    content={message.content}
                    edited={message.contentEdited}
                  />
                  {message.containsOpenGraphObjects && (
                    <ContentMessageItemUrlPreview
                      openGraphObjects={message.openGraphDtos}
                      {...this.props}
                    />
                  )}
                </div>
              )}
              {message.hasFileAttachments && (
                <div className="file-container">
                  {message.fileAttachments.map(file => (
                    <ContentMessageItemFileItem
                      key={file.id}
                      file={file}
                      {...this.props}
                    />
                  ))}
                </div>
              )}
            </div>
          </div>
          {message.messageReactions && message.messageReactions.length > 0 && (
            <div className="message-reactions-container">
              <ContentMessageItemUserReactions
                reactions={message.messageReactions}
                onEmojiColonsAdded={this.onEmojiColonsAdded}
                onEmojiColonsRemoved={this.onEmojiColonsRemoved}
                {...this.props}
              />
            </div>
          )}
        </div>
      </div>
    );
  }
}

function RawMessage(props) {
  var content = props.content;
  if (props.edited) {
    var template = document.createElement("template");
    template.innerHTML = content;
    var editedNode = document.createElement("span");
    var textNode = document.createTextNode("(edited)");
    editedNode.appendChild(textNode);
    editedNode.setAttribute("class", "content-edited-text");
    template.content.lastChild.appendChild(editedNode);
    content = template.innerHTML;
  }
  return (
    <div
      dangerouslySetInnerHTML={{
        __html: content
      }}
    />
  );
}

export default ContentMessageItem;
