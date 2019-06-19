import React from "react";
import "./SidebarHeader.css";
import { Icon, Popup } from "semantic-ui-react";
import Modal from "modals/Modal";
import DropdownModal from "modals/DropdownModal";
import InvitePeopleForm from "modalForms/InvitePeopleForm";
import SetStatusForm from "modalForms/SetStatusForm";
import AuthService from "services/AuthService";
import UserStatusService from "services/UserStatusService";
import { toast } from "react-toastify";
import ApiService from "services/ApiService";

class SidebarHeader extends React.Component {
  constructor(props) {
    super(props);

    this.onHeaderClick = this.onHeaderClick.bind(this);
    this.onCloseDropdown = this.onCloseDropdown.bind(this);
    this.onInvitePeople = this.onInvitePeople.bind(this);
    this.onCloseInvitePeople = this.onCloseInvitePeople.bind(this);
    this.onSetStatus = this.onSetStatus.bind(this);
    this.onClearStatus = this.onClearStatus.bind(this);
    this.onCloseSetStatus = this.onCloseSetStatus.bind(this);
    this.onLogout = this.onLogout.bind(this);
    this.authService = new AuthService(props);
    this.apiService = new ApiService(props);
    this.UserStatusService = new UserStatusService();

    this.state = {
      isInvitePeopleModalOpen: false,
      isSetStatusModalOpen: false,
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

  onSetStatus(event) {
    this.setState({
      isSetStatusModalOpen: true,
      isDropdownModalOpen: false
    });
  }

  onClearStatus(event) {
    this.apiService
      .fetch(`/api/users/clearStatus`, {
        method: "POST"
      })
      .then(id => {
        this.props.onUserSessionDataChange();
        this.setState({
          isDropdownModalOpen: false
        });
      })
      .catch(error => {
        toast.error(`Clear status failed: ${error}`);
      });
  }

  onCloseSetStatus(event) {
    this.setState({
      isSetStatusModalOpen: false
    });
  }

  onLogout() {
    this.authService.logout();
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
              <Icon name="chevron down" />
            </div>
            <div className="sidebar-header-user-name">
              {this.props.userProfile.displayName}
              {(() => {
                if (this.UserStatusService.isNotActive(this.props.userStatus)) {
                  let statusName = this.UserStatusService.getStatusName(
                    this.props.userStatus
                  );
                  return (
                    <Popup
                      trigger={<Icon name="flag" className="status-icon" />}
                      content={statusName}
                      inverted
                      position="right center"
                      size="tiny"
                    />
                  );
                }
              })()}
            </div>
          </div>
          {this.state.isDropdownModalOpen && (
            <DropdownModal onClose={this.onCloseDropdown}>
              <div className="sidebar-header-dropdown panel">
                <section className="sidebar-header-dropdown-section">
                  <div className="sidebar-header-dropdown-section-header">
                    <img
                      className="user-identicon"
                      src={this.props.userProfile.identiconPath}
                    />
                    {this.props.userProfile.displayName}
                  </div>
                  <ul>
                    {this.UserStatusService.isNotActive(
                      this.props.userStatus
                    ) ? (
                      <li onClick={this.onClearStatus}>Clear status</li>
                    ) : (
                      <li onClick={this.onSetStatus}>Set status</li>
                    )}

                    <li>Profile</li>
                  </ul>
                </section>
                <section className="sidebar-header-dropdown-section">
                  <div className="sidebar-header-dropdown-section-header">
                    {this.props.userProfile.workspaceName}
                  </div>
                  <ul>
                    <li onClick={this.onInvitePeople}>Invite people</li>
                    <li onClick={this.onLogout}>
                      Log out of {this.props.userProfile.workspaceName}
                    </li>
                  </ul>
                </section>
              </div>
            </DropdownModal>
          )}
        </div>
        <div>
          {this.state.isSetStatusModalOpen && (
            <Modal onClose={this.onCloseSetStatus}>
              <SetStatusForm
                onClose={this.onCloseSetStatus}
                onSelect={this.props.onUserSessionDataChange}
              />
            </Modal>
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
