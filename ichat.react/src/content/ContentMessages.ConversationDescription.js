import React from "react";
import "./ContentMessages.Description.css";

class ContentMessagesConversationDescription extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <div className="content-message-description">
        <div className="user-identicons">
          {this.props.userList
            .filter(user => user.id !== this.props.userProfile.id)
            .map(u => (
              <img
                key={u.id}
                className="user-identicon"
                src={u.identiconPath}
              />
            ))}
        </div>
        <p>
          <span>
            This is the very beginnning of the direct message history between
            you and{" "}
            {this.props.userList
              .filter(user => user.id !== this.props.userProfile.id)
              .map(user => user.displayName)
              .join(", ")}
            . Let's chat!
          </span>
        </p>
      </div>
    );
  }
}

export default ContentMessagesConversationDescription;
