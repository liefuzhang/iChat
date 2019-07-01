import React from "react";
import "./ContentMessages.Description.css";

class ContentMessagesConversationDescription extends React.Component {
  constructor(props) {
    super(props);

    this.dto = props.messageChannelDescriptionDto;
    this.otherUserList = this.userNames = props.messageChannelDescriptionDto.userList.filter(
      user => user.id !== props.userProfile.id
    );
    this.userNames = this.otherUserList
      .map(user => user.displayName)
      .join(", ");
  }

  render() {
    return (
      <div className="content-message-description">
        <div className="user-identicons">
          {this.otherUserList.map(u => (
            <img key={u.id} className="user-identicon" src={u.identiconPath} />
          ))}
        </div>
        <p>
          <span>
            This is the very beginnning of the direct message history between
            you and {this.userNames}. Let's chat!
          </span>
        </p>
      </div>
    );
  }
}

export default ContentMessagesConversationDescription;
