import React from "react";
import "./ContentHeaderUserInfo.css";
import { Icon, Popup } from "semantic-ui-react";
import Modal from "modals/Modal";
import DropdownModal from "modals/DropdownModal";
import InvitePeopleForm from "modalForms/InvitePeopleForm";
import SetStatusForm from "modalForms/SetStatusForm";
import EditProfileForm from "modalForms/EditProfileForm";
import AuthService from "services/AuthService";
import UserStatusService from "services/UserStatusService";
import { toast } from "react-toastify";
import ApiService from "services/ApiService";

class ContentHeaderUserInfo extends React.Component {
  constructor(props) {
    super(props);

    this.onHeaderClick = this.onHeaderClick.bind(this);
    this.onCloseDropdown = this.onCloseDropdown.bind(this);
    this.onInvitePeople = this.onInvitePeople.bind(this);
    this.onCloseInvitePeople = this.onCloseInvitePeople.bind(this);
    this.onCloseEditProfile = this.onCloseEditProfile.bind(this);
    this.onSetStatus = this.onSetStatus.bind(this);
    this.onEditProfile = this.onEditProfile.bind(this);
    this.onClearStatus = this.onClearStatus.bind(this);
    this.onCloseSetStatus = this.onCloseSetStatus.bind(this);
    this.onLogout = this.onLogout.bind(this);
    this.authService = new AuthService(props);
    this.apiService = new ApiService(props);
    this.userStatusService = new UserStatusService();

    this.state = {
      isInvitePeopleModalOpen: false,
      isSetStatusModalOpen: false,
      isEditProfileModalOpen: false,
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

  onEditProfile(event) {
    this.setState({
      isEditProfileModalOpen: true,
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

  onCloseEditProfile(event) {
    this.setState({
      isEditProfileModalOpen: false
    });
  }

  onLogout() {
    this.authService.logout();
  }

  render() {
    return (
      <div>
        <div className="header-user-info">
          <div
            className="header-user-info-container"
            onClick={this.onHeaderClick}
          >
            <div>
              <Icon name="chevron down" />
              <span className="header-user-info-workspace-name">
                {this.props.userProfile.workspaceName}
              </span>
            </div>
            <div className="header-user-info-user-name">
              {this.props.userProfile.displayName}
              {(() => {
                if (this.userStatusService.isNotActive(this.props.userStatus)) {
                  let statusName = this.userStatusService.getStatusName(
                    this.props.userStatus
                  );
                  return (
                    <Popup
                      trigger={<Icon name="flag" className="status-icon" />}
                      content={statusName}
                      inverted
                      position="top right"
                      size="tiny"
                    />
                  );
                }
              })()}
            </div>
          </div>
          {this.state.isDropdownModalOpen && (
            <DropdownModal onClose={this.onCloseDropdown}>
              <div className="header-user-info-dropdown dropdown-container panel">
                <section className="dropdown-section">
                  <div className="dropdown-section-header">
                    <img
                      className="user-identicon"
                      src={this.props.userProfile.identiconPath}
                    />
                    {this.props.userProfile.displayName}
                  </div>
                  <ul>
                    {this.userStatusService.isNotActive(
                      this.props.userStatus
                    ) ? (
                      <li onClick={this.onClearStatus}>Clear status</li>
                    ) : (
                      <li onClick={this.onSetStatus}>Set status</li>
                    )}

                    <li onClick={this.onEditProfile}>Profile</li>
                  </ul>
                </section>
                <section className="dropdown-section">
                  <div className="dropdown-section-header">
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
          {this.state.isEditProfileModalOpen && (
            <Modal onClose={this.onCloseEditProfile}>
              <EditProfileForm
                userProfile={this.props.userProfile}
                onClose={this.onCloseEditProfile}
                onProfileUpdated={this.props.onProfileUpdated}
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

export default ContentHeaderUserInfo;
