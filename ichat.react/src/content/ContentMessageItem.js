import React from "react";
import { Icon, Popup } from "semantic-ui-react";
import "./ContentMessageItem.css";
import AuthService from "services/AuthService";
import { toast } from "react-toastify";

class ContentMessageItem extends React.Component {
  constructor(props) {
    super(props);

    this.authService = new AuthService(props);
  }

  onHoverMessageItem(event) {
    event.currentTarget.classList.add("message-hover");
  }

  onLeaveMessageItem(event) {
    event.currentTarget.classList.remove("message-hover");
  }

  onDownloadClick(id, name) {
    this.authService
      .fetchFile(`/api/messages/downloadFile/${id}`, name)
      .catch(error => {
        toast.error(`Download file failed: ${error}`);
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
                      key={file.name + file.lastModified}
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
