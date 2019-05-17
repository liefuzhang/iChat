import React from "react";
import "./SidebarHeader.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Modal from "./Modal";

class SidebarHeader extends React.Component {
  constructor(props) {
    super(props);

    this.onInvitePeople = this.onInvitePeople.bind(this);
    this.onCloseInvitePeople = this.onCloseInvitePeople.bind(this);

    this.state = {
      isInvitePeopleModalOpen: false
    };
  }

  onHeaderClick(event) {
    document.querySelector(".sidebar-header-dropdown").classList.toggle("hide");
  }

  onInvitePeople(event) {
    this.onHeaderClick();
    this.setState({
      isInvitePeopleModalOpen: true
    });
  }

  onCloseInvitePeople(event) {
    this.setState({
      isInvitePeopleModalOpen: false
    });
  }

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
                {this.props.userProfile.workspaceName}
              </span>
              <FontAwesomeIcon icon="chevron-down"/>
            </div>
            <div className="sidebar-header-user-name">
              {this.props.userProfile.displayName}
            </div>
          </div>
          <div className="sidebar-header-dropdown panel hide">
            <section className="sidebar-header-dropdown-section">
              <div className="sidebar-header-dropdown-section-header">
                <img src={this.props.userProfile.identiconPath} />
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
        <div className="invite-people-modal hide">
          {this.state.isInvitePeopleModalOpen && (
            <Modal onClose={this.onCloseInvitePeople}>
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
            </Modal>
          )}
        </div>
      </div>
    );
  }
}

export default SidebarHeader;
