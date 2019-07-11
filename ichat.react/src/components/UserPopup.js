import React from "react";
import "./UserPopup.css";
import BlankModal from "../modals/BlankModal";

class UserPopup extends React.Component {
  constructor(props) {
    super(props);

    this.closeUserPopup = this.closeUserPopup.bind(this);

    this.state = {};
  }

  closeUserPopup() {
    this.props.onClose();
  }

  componentDidMount() {
    this.calculatePostion();
  }

  calculatePostion() {
    let gap = 10;
    let popup = document.querySelector(".page-overlay-content");
    let popupRect = popup.getBoundingClientRect();
    let targetRect = this.props.clickedTarget.getBoundingClientRect();
    let top = targetRect.bottom - popupRect.height;
    let left = targetRect.right + gap;
    let contentHeaderHeight = document.querySelector(".content-header")
      .offsetHeight;
    let translateY = 0;
    let translateX = 0;
    if (top < contentHeaderHeight) translateY = contentHeaderHeight - top + gap;
    if (left + popupRect.right > window.innerWidth)
      translateX = window.innerWidth - (left + popupRect.right) - gap;

    popup.setAttribute(
      "style",
      `top:${top}px; left: ${left}px; visibility: visible; transform: translate(${translateX}px, ${translateY}px)`
    );
  }

  render() {
    let user = this.props.user;
    return (
      <BlankModal>
        <div className="user-popup-container">
          <div className="page-overlay" onClick={this.closeUserPopup} />
          <div className="user-popup panel page-overlay-content invisible">
            <div className="user-popup-header">
              <img className="user-identicon" src={user.identiconPath} />
            </div>
            <div className="user-popup-body">
              <div className="user-popup-user-info">
                <div className="user-popup-user-name">
                  <b>{user.displayName}</b>
                </div>
                <div>Email: {user.email}</div>
                {user.phoneNumber && <div>Phone: {user.phoneNumber}</div>}
              </div>
              <div className="user-popup-list-section">
                <div className="dropdown-section">
                  <ul>
                    <li>Direct message</li>
                  </ul>
                </div>
              </div>
            </div>
          </div>
        </div>
      </BlankModal>
    );
  }
}

export default UserPopup;
