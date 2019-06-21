import React from "react";
import { Icon, Popup, Confirm } from "semantic-ui-react";
import "./ContentMessageItem.css";
import ApiService from "services/ApiService";
import { toast } from "react-toastify";

class ContentMessageItem extends React.Component {
  constructor(props) {
    super(props);

    this.apiService = new ApiService(props);
    this.onDeleteMessageConfirmed = this.onDeleteMessageConfirmed.bind(this);

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

  onDownloadClick(id, name) {
    this.apiService
      .fetchFile(`/api/messages/downloadFile/${id}`, name)
      .catch(error => {
        toast.error(`Download file failed: ${error}`);
      });
  }

  onDeleteMessageConfirmed(messageId) {
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
      })
      .finally(() => {
        this.setState({
          isDeleteMessageConfirmOpen: false
        });
      });
  }

  render() {
    let message = this.props.message;
    return (
      <div
        className={
          "message-item-container" +
          (message.isConsecutiveMessage ? " following-message" : "")
        }
        key={message.id}
        onMouseOver={this.onHoverMessageItem}
        onMouseLeave={this.onLeaveMessageItem}
      >
        <img className="user-identicon" src={message.sender.identiconPath} />
        <div className="message-item">
          <div className="message-title">
            <b>{message.sender.displayName}</b>{" "}
            <span className="message-time">&nbsp;{message.timeString}</span>
          </div>
          <div className="message-content-container">
            <div className="message-content-time">
              <span className="message-time">{message.timeString}</span>
            </div>
            <div className="message-toolbar">
              <div className="message-toolbar-item">
                {message.sender.id === this.props.userProfile.id && (
                  <Popup
                    trigger={
                      <div
                        className="message-toolbar-item-content"
                        onClick={() =>
                          this.setState({ isDeleteMessageConfirmOpen: true })
                        }
                      >
                        <Icon name="trash alternate outline" />{" "}
                      </div>
                    }
                    content="Delete message"
                    inverted
                    position="top center"
                    size="tiny"
                  />
                )}
                <Confirm
                  open={this.state.isDeleteMessageConfirmOpen}
                  header="Delete message"
                  content="Are you sure you want to delete this message? This cannot be undone."
                  onCancel={() =>
                    this.setState({
                      isDeleteMessageConfirmOpen: false
                    })
                  }
                  onConfirm={() => this.onDeleteMessageConfirmed(message.id)}
                />
              </div>
            </div>

            <div className="message-content">
              {!message.hasFileAttachments && (
                <RawMessage content={message.content} />
              )}
              {message.hasFileAttachments && (
                <div className="file-container">
                  {message.fileAttachments.map(file => (
                    <Popup
                      trigger={
                        <div
                          className="file-item"
                          onClick={() =>
                            this.onDownloadClick(file.id, file.name)
                          }
                        >
                          <Icon name="file outline" size="large" />
                          <span title={file.name}>{file.name}</span>
                        </div>
                      }
                      key={file.name + file.id}
                      content="Click to download"
                      inverted
                      position="top center"
                      size="tiny"
                    />
                  ))}
                </div>
              )}
            </div>
          </div>
        </div>
      </div>
    );
  }
}

function RawMessage(props) {
  return <div dangerouslySetInnerHTML={{ __html: props.content }} />;
}

export default ContentMessageItem;
