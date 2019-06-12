import React from "react";
import "./ContentMessageItem.css";

class ContentMessageItem extends React.Component {
  onHoverMessageItem(event) {
    event.currentTarget.classList.add("message-hover");
  }

  onLeaveMessageItem(event) {
    event.currentTarget.classList.remove("message-hover");
  }

  render() {
    return (
      <div
        className={
          "message-item-container" +
          (this.props.message.isConsecutiveMessage ? " following-message" : "")
        }
        key={this.props.message.id}
        onMouseOver={this.onHoverMessageItem}
        onMouseLeave={this.onLeaveMessageItem}
      >
        <img
          className="user-identicon"
          src={this.props.message.sender.identiconPath}
        />
        <div className="message-item">
          <div className="message-title">
            <b>{this.props.message.sender.displayName}</b>{" "}
            <span className="message-time">
              &nbsp;{this.props.message.timeString}
            </span>
          </div>
          <div className="message-content-container">
            <div className="message-content-time">
              <span className="message-time">
                {this.props.message.timeString}
              </span>
            </div>
            <RawMessage content={this.props.message.content} />
          </div>
        </div>
      </div>
    );
  }
}

function RawMessage(props) {
  return (
    <div
      className="message-content"
      dangerouslySetInnerHTML={{ __html: props.content }}
    />
  );
}

export default ContentMessageItem;
