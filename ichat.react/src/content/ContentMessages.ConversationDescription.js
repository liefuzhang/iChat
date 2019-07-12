import React from "react";
import "./ContentMessages.Description.css";

class ContentMessagesConversationDescription extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    let userList = this.props.isSelfConversation
      ? this.props.userList
      : this.props.userList.filter(
          user => user.id !== this.props.userProfile.id
        );
    return (
      <div className="content-message-description">
        <div className="user-identicons">
          {userList.map(u => (
            <img key={u.id} className="user-identicon" src={u.identiconPath} />
          ))}
        </div>
        <p>
          {this.props.isSelfConversation && (
            <span>
              <b>This is your own space!</b> Feel free to play around, draft or
              simply keep things handy.
            </span>
          )}
          {!this.props.isSelfConversation && (
            <span>
              This is the very beginnning of the direct message history between
              you and{" "}
              {this.props.userList
                .filter(user => user.id !== this.props.userProfile.id)
                .map(user => user.displayName)
                .join(", ")}
              . Let's chat!
            </span>
          )}
        </p>
      </div>
    );
  }
}

export default ContentMessagesConversationDescription;
