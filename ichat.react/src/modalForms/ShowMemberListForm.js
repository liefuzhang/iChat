import React from "react";
import "./ShowMemberListForm.css";

class ShowMemberListForm extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <div className="form-container form-with-dropdown">
        <h1 style={{ textAlign: "center" }}>Member List</h1>

        <form id="showMemberListForm">
          {this.props.memberList.map(u => (
            <div
              className="member-list-item"
              key={u.id}
              onClick={event =>
                this.props.onOpenUserPopup(event.currentTarget, u.id)
              }
            >
              <img className="user-identicon" src={u.identiconPath} />
              <b>{u.displayName}</b>
            </div>
          ))}
        </form>
      </div>
    );
  }
}

export default ShowMemberListForm;
