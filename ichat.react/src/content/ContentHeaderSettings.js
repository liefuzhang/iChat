import React from "react";
import "./ContentHeaderSettings.css";
import ApiService from "services/ApiService";
import { Icon, Popup } from "semantic-ui-react";
import DropdownModal from "modals/DropdownModal";
import Modal from "modals/Modal";
import { toast } from "react-toastify";
import InviteOtherMembersForm from "modalForms/InviteOtherMembersForm";
import ShowMemberListForm from "modalForms/ShowMemberListForm";

class ContentHeaderSettings extends React.Component {
  constructor(props) {
    super(props);

    this.apiService = new ApiService(props);
    this.onSettingButtonClicked = this.onSettingButtonClicked.bind(this);
    this.onCloseSettingDropdown = this.onCloseSettingDropdown.bind(this);
    this.onLeaveChannelClicked = this.onLeaveChannelClicked.bind(this);
    this.onInviteOtherMembers = this.onInviteOtherMembers.bind(this);
    this.onCloseInviteOtherMembers = this.onCloseInviteOtherMembers.bind(this);
    this.onShowMemberList = this.onShowMemberList.bind(this);
    this.onCloseShowMemberList = this.onCloseShowMemberList.bind(this);

    this.state = {
      isSettingDropdownModalOpen: false,
      isInviteOtherMembersModalOpen: false,
      isShowMemberListModalOpen: false
    };
  }

  onSettingButtonClicked() {
    this.setState({
      isSettingDropdownModalOpen: true
    });
  }

  onCloseSettingDropdown() {
    this.setState({
      isSettingDropdownModalOpen: false
    });
  }

  onLeaveChannelClicked() {
    let defaultChannelId = this.props.userProfile.defaultChannelId;
    this.apiService
      .fetch(`/api/channels/leave`, {
        method: "POST",
        body: JSON.stringify(this.props.id)
      })
      .then(id => {
        this.onCloseSettingDropdown();
        this.props.history.push(`/channel/${defaultChannelId}`);
      })
      .catch(error => {
        toast.error(`Leave channel failed: ${error}`);
      });
  }

  onInviteOtherMembers() {
    this.setState({
      isInviteOtherMembersModalOpen: true,
      isSettingDropdownModalOpen: false
    });
  }

  onCloseInviteOtherMembers() {
    this.setState({
      isInviteOtherMembersModalOpen: false
    });
  }

  onShowMemberList() {
    this.setState({
      isShowMemberListModalOpen: true,
      isSettingDropdownModalOpen: false
    });
  }

  onCloseShowMemberList() {
    this.setState({
      isShowMemberListModalOpen: false
    });
  }

  componentDidUpdate(prevProps) {
    if (
      this.props.isChannel !== prevProps.isChannel ||
      this.props.id !== prevProps.id
    ) {
      this.onCloseShowMemberList();
    }
  }

  render() {
    return (
      <div className="content-header-settings-container">
        <span onClick={this.onSettingButtonClicked}>
          <Popup
            trigger={<Icon name="setting" className="icon-setting" />}
            content={`${
              this.props.isChannel ? "Channel" : "Conversation"
            } settings`}
            inverted
            position="bottom center"
            size="tiny"
          />
        </span>
        {this.state.isSettingDropdownModalOpen && (
          <DropdownModal onClose={this.onCloseSettingDropdown}>
            <div className="setting-dropdown dropdown-container panel">
              <section className="dropdown-section">
                <ul>
                  <li onClick={this.onShowMemberList}>Show member list</li>
                  {(this.props.isChannel ||
                    (!this.props.selectedConversation.isPrivate &&
                      !this.props.selectedConversation.isSelfConversation)) && (
                    <li onClick={this.onInviteOtherMembers}>
                      {this.props.isChannel
                        ? `Invite people to ${this.props.selectedChannel.name}`
                        : `Invite another member`}
                    </li>
                  )}
                  {this.props.isChannel &&
                    this.props.id !==
                      this.props.userProfile.defaultChannelId && (
                      <li onClick={this.onLeaveChannelClicked}>{`Leave ${
                        this.props.selectedChannel.name
                      }`}</li>
                    )}
                </ul>
              </section>
            </div>
          </DropdownModal>
        )}
        <div>
          {this.state.isInviteOtherMembersModalOpen && (
            <Modal onClose={this.onCloseInviteOtherMembers}>
              <InviteOtherMembersForm
                onClose={this.onCloseInviteOtherMembers}
                {...this.props}
              />
            </Modal>
          )}
        </div>
        <div>
          {this.state.isShowMemberListModalOpen && (
            <Modal onClose={this.onCloseShowMemberList}>
              <ShowMemberListForm
                memberList={this.props.messageChannelUserList}
                onOpenUserPopup={this.props.onOpenUserPopup}
                onClose={this.onCloseShowMemberList}
                {...this.props}
              />
            </Modal>
          )}
        </div>
      </div>
    );
  }
}

export default ContentHeaderSettings;
