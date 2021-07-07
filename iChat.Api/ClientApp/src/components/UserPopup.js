import React from "react";
import "./UserPopup.css";
import { toast } from "react-toastify";
import ApiService from "services/ApiService";
import BlankModalWithPageOverlay from "modals/BlankModalWithPageOverlay";

class UserPopup extends React.Component {
  constructor(props) {
    super(props);

    this.startConversation = this.startConversation.bind(this);
    this.apiService = new ApiService(props);

    this.state = {
      userRealtimeStatusLoaded: false
    };
  }

  startConversation() {
    this.apiService
      .fetch(`/api/conversations/start`, {
        method: "POST",
        body: JSON.stringify([this.props.user.id])
      })
      .then(id => {
        this.props.onClose();
        this.props.history.push(`/conversation/${id}`);
      })
      .catch(error => {
        toast.error(`Start conversation failed: ${error}`);
      });
  }

  componentDidMount() {
    this.apiService
      .fetch(`/api/users/${this.props.user.id}/onlineStatus`)
      .then(isOnline => {
        this.isUserOnline = isOnline;
        this.setState({ userRealtimeStatusLoaded: true });
      });
  }

  render() {
    let user = this.props.user;
    return (
      <BlankModalWithPageOverlay
        clickedTarget={this.props.clickedTarget}
        onClose={this.props.onClose}
      >
        <div className="user-popup panel">
          <div className="user-popup-header">
            <img className="user-identicon" src={user.identiconPath} />
          </div>
          <div className="user-popup-body">
            <div className="user-popup-user-info">
              <div className="user-popup-user-name">
                <b>{user.displayName}</b>
                {this.state.userRealtimeStatusLoaded && this.isUserOnline && (
                  <span className="user-realtime-status" title="active">
                    <span className="user-realtime-status-circle" />
                  </span>
                )}
                {this.state.userRealtimeStatusLoaded && !this.isUserOnline && (
                  <span
                    className="user-realtime-status user-offline"
                    title="away"
                  >
                    <span className="user-realtime-status-circle" />
                  </span>
                )}
              </div>
              <div>Email: {user.email}</div>
              {user.phoneNumber && <div>Phone: {user.phoneNumber}</div>}
            </div>
            <div className="user-popup-list-section">
              <div className="dropdown-section">
                <ul>
                  <li onClick={this.startConversation}>Direct message</li>
                </ul>
              </div>
            </div>
          </div>
        </div>
      </BlankModalWithPageOverlay>
    );
  }
}

export default UserPopup;
