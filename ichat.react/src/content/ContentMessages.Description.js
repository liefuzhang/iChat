import React from "react";
import "./ContentMessages.Description.css";

class ContentMessagesDescription extends React.Component {
  constructor(props) {
    super(props);

    this.dto = props.messageChannelDescriptionDto;
    this.createdBy =
      this.dto.createdByUser.id === props.userProfile.id
        ? "you"
        : this.dto.createdByUser.displayName;
  }

  render() {
    return (
      <div className="content-message-description">
        <div className="message-description-name">
          {this.dto.messageChannelName}
        </div>
        <p>
          {this.props.isChannel && (
            <span>
              This channel has been created by {this.createdBy} on{" "}
              {this.dto.createdDateString}. This is the very beginning of the{" "}
              <b>{this.dto.messageChannelName}</b> channel.
            </span>
          )}
        </p>
      </div>
    );
  }
}

export default ContentMessagesDescription;
