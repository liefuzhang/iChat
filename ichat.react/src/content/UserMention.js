import React from "react";
import "./UserMention.css";

class UserMention extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <div className="user-mention panel">
        {this.props.userList.map(u => (
          <div
            key={u.id}
            className="user-mention-item list-item"
            onClick={() => {
              this.props.onUserSelected(u.id);
            }}
          >
            <img className="user-identicon" src={u.identiconPath} />
            <span className="user-mention-name">{u.displayName}</span>
            <span className="user-mention-email">{u.email}</span>
          </div>
        ))}
      </div>
    );
  }
}

export default UserMention;
