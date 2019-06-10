import React from "react";
import "./UserMention.css";
import AuthService from "services/AuthService";

class UserMention extends React.Component {
  constructor(props) {
    super(props);

    this.authService = new AuthService(props);
  }

  render() {
    return (
      <div className="user-mention panel">
        {this.props.userList.map(u => (
          <div key={u.id} className="user-mention-item" data-user-id={u.id}>
            {u.displayName}
          </div>
        ))}
      </div>
    );
  }
}

export default UserMention;
