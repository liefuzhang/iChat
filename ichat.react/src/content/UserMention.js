import React from "react";
import "./UserMention.css";

class UserMention extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <div className="user-mention panel">
        <div className="user-mention-title">
          <div className="mention-filter-name">
            People matching "@<b>{this.props.filterName}</b>"
          </div>
          <div className="mention-prompt">
          <span><b>&uarr;</b> <b>&darr;</b> to navigate</span>
          <span><b>&crarr;</b> to select</span>
          <span><b>esc</b> to dismiss</span>
          </div>
        </div>
        {this.props.userList.map((u, index) => {
          let highlighting = index === this.props.highlightItemIndex;
          return (
            <div
              key={u.id}
              className={
                "user-mention-item list-item" +
                (highlighting ? " active-item" : "")
              }
              onMouseOver={() => {
                this.props.onMentionSelecting(u.id);
              }}
              onClick={event => {
                event.preventDefault();
                this.props.onMentionSelected(u.id);
              }}
            >
              <img className="user-identicon" src={u.identiconPath} />
              <span className="user-mention-name">{u.displayName}</span>
              <span className="user-mention-email">{u.email}</span>
            </div>
          );
        })}
      </div>
    );
  }
}

export default UserMention;
