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

  render() {
    return (
      <BlankModal>
        <div className="user-popup-container">
          <div className="page-overlay" onClick={this.closeUserPopup} />
          <div className="user-popup panel page-overlay-content">
            <div className="user-popup-header">header</div>
            <div className="user-popup-body">send direct message</div>
          </div>
        </div>
      </BlankModal>
    );
  }
}

export default UserPopup;
