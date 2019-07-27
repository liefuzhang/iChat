import React from "react";
import "./ContentHeader.css";
import ApiService from "services/ApiService";
import { Icon, Popup } from "semantic-ui-react";
import DropdownModal from "modals/DropdownModal";
import Modal from "modals/Modal";
import { toast } from "react-toastify";
import InviteOtherMembersForm from "modalForms/InviteOtherMembersForm";
import ShowMemberListForm from "modalForms/ShowMemberListForm";

class ContentHeader extends React.Component {
  constructor(props) {
    super(props);

    this.fetchData = this.fetchData.bind(this);
    this.apiService = new ApiService(props);
    this.onSettingButtonClicked = this.onSettingButtonClicked.bind(this);
    this.onCloseSettingDropdown = this.onCloseSettingDropdown.bind(this);
    this.onLeaveChannelClicked = this.onLeaveChannelClicked.bind(this);
    this.onInviteOtherMembers = this.onInviteOtherMembers.bind(this);
    this.onCloseInviteOtherMembers = this.onCloseInviteOtherMembers.bind(this);
    this.onShowMemberList = this.onShowMemberList.bind(this);
    this.onCloseShowMemberList = this.onCloseShowMemberList.bind(this);

    if (props.hubConnection) {
      props.hubConnection.on("ConversationUserListChanged", () =>
        this.fetchData(this.props)
      );
    }

    this.state = {
      selectedChannel: {},
      selectedConversation: {},
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

  fetchData(props) {
    if (props.isChannel) {
      this.apiService
        .fetch(`/api/channels/${props.id}`)
        .then(channel => this.setState({ selectedChannel: channel }));
    } else {
      this.apiService
        .fetch(`/api/conversations/${props.id}`)
        .then(conversation =>
          this.setState({ selectedConversation: conversation })
        );
    }
  }

  componentDidMount() {
    this.fetchData(this.props);
  }

  componentDidUpdate(prevProps) {
    if (
      this.props.isChannel !== prevProps.isChannel ||
      this.props.id !== prevProps.id
    ) {
      this.fetchData(this.props);
      this.onCloseShowMemberList();
    }
  }

  render() {
    return (
      <div className="content-header">
        <div className="content-header-details">
          <div className="content-header-name">
            {this.props.isChannel
              ? this.state.selectedChannel.name
              : this.state.selectedConversation.name}
          </div>
          <div className="content-header-topic">
            {this.props.isChannel ? this.state.selectedChannel.topic : ""}
          </div>
        </div>
        <div className="content-header-toolbar">
          <div className="content-header-toolbar-item">
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
                        (!this.state.selectedConversation.isPrivate &&
                          !this.state.selectedConversation
                            .isSelfConversation)) && (
                        <li onClick={this.onInviteOtherMembers}>
                          {this.props.isChannel
                            ? `Invite people to ${
                                this.state.selectedChannel.name
                              }`
                            : `Invite another member`}
                        </li>
                      )}
                      {this.props.isChannel &&
                        this.props.id !==
                          this.props.userProfile.defaultChannelId && (
                          <li onClick={this.onLeaveChannelClicked}>{`Leave ${
                            this.state.selectedChannel.name
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
        </div>
      </div>
    );
  }
}

export default ContentHeader;
