import React from "react";
import "./SidebarHeader.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Modal from "modals/Modal";
import DropdownModal from "modals/DropdownModal";
import InvitePeopleForm from "modalForms/InvitePeopleForm";

class SidebarHeader extends React.Component {
  constructor(props) {
    super(props);

    this.onHeaderClick = this.onHeaderClick.bind(this);
    this.onCloseDropdown = this.onCloseDropdown.bind(this);
    this.onInvitePeople = this.onInvitePeople.bind(this);
    this.onCloseInvitePeople = this.onCloseInvitePeople.bind(this);

    this.state = {
      isInvitePeopleModalOpen: false,
      isDropdownModalOpen: false
    };
  }

  onHeaderClick(event) {
    this.setState({
      isDropdownModalOpen: true
    });
  }

  onCloseDropdown(event) {
    this.setState({
      isDropdownModalOpen: false
    });
  }

  onInvitePeople(event) {
    this.setState({
      isInvitePeopleModalOpen: true,
      isDropdownModalOpen: false
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
              <FontAwesomeIcon icon="chevron-down" />
            </div>
            <div className="sidebar-header-user-name">
              {this.props.userProfile.displayName}
            </div>
          </div>
          {this.state.isDropdownModalOpen && (
            <DropdownModal onClose={this.onCloseDropdown}>
              <div className="sidebar-header-dropdown panel">
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
            </DropdownModal>
          )}
        </div>
        <div>
          {this.state.isInvitePeopleModalOpen && (
            <Modal onClose={this.onCloseInvitePeople}>
              <InvitePeopleForm
                userProfile={this.props.userProfile}
                onClose={this.onCloseInvitePeople}
              />
            </Modal>
          )}
        </div>
      </div>
    );
  }
}

export default SidebarHeader;
