import React from "react";
import "./SidebarHeader.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

class SidebarHeader extends React.Component {
  constructor(props) {
    super(props);
  }

  onHeaderClick(event) {
    document.querySelector(".sidebar-header-dropdown").classList.toggle("hide");
  }

  onInvitePeople(event) {}

  componentDidMount() {
    document
      .querySelector(".sidebar-header-container")
      .addEventListener("click", this.onHeaderClick);
  }

  componentWillUnmount() {
    document
      .querySelector(".sidebar-header-container")
      .removeEventListener("click", this.onHeaderClick);
  }

  render() {
    return (
      <div>
        <div className="sidebar-header">
          <div className="sidebar-header-container">
            <div>
              <span className="sidebar-header-workspace-name">
                {this.props.userProfile.workspaceName}{" "}
              </span>
              <FontAwesomeIcon icon="chevron-down" />
            </div>
            <div className="sidebar-header-user-name">
              {this.props.userProfile.displayName}
            </div>
          </div>
          <div className="sidebar-header-dropdown panel hide">
            <section className="sidebar-header-dropdown-section">
              <div className="sidebar-header-dropdown-section-header">
                {this.props.userProfile.displayName}
              </div>
              <ul>
                <li>Set status</li>
                <li>Profile</li>
              </ul>
            </section>
            <section className="sidebar-header-dropdown-section">
              <div className="sidebar-header-dropdown-section-header">
                {this.props.userProfile.workspaceName}
              </div>
              <ul>
                <li onClick={this.onInvitePeople}>Invite people</li>
                <li>Log out of {this.props.userProfile.workspaceName}</li>
              </ul>
            </section>
          </div>
        </div>
        <div className="modal-panel-container">
          <div className="modal-panel-overlay">
            <div className="modal-panel panel">
              <h1 style={{ textAlign: "center" }}>
                Invite People to {this.props.userProfile.workspaceName}
              </h1>
              <div className="invite-people">
                <input
                  className="form-control"
                  type="email"
                  placeholder="Email"
                  required
                />
                <button className="btn form-control">Send Invitation</button>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default SidebarHeader;
