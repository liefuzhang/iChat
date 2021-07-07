import React from "react";
import "./SidebarItem.css";
import JoinChannelForm from "modalForms/JoinChannelForm";
import Modal from "modals/Modal";
import AuthService from "services/AuthService";

class SidebarItemJoinChannel extends React.Component {
  constructor(props) {
    super(props);

    this.authService = new AuthService(props);
    this.onJoinChannel = this.onJoinChannel.bind(this);
    this.onChannelJoined = this.onChannelJoined.bind(this);
    this.onCloseJoinChannelModal = this.onCloseJoinChannelModal.bind(this);

    this.state = {
      isJoinChannelModalOpen: false
    };
  }

  onJoinChannel(event) {
    event.stopPropagation();
    this.setState({
      isJoinChannelModalOpen: true
    });
  }

  onChannelJoined() {
    this.onCloseJoinChannelModal();
    this.props.onUpdateChannelList();
  }

  onCloseJoinChannelModal() {
    this.setState({
      isJoinChannelModalOpen: false
    });
  }

  render() {
    return (
      <div className="sidebar-item list-item">
        <span onClick={this.onJoinChannel}>+&nbsp;Join a channel</span>
        <div>
          {this.state.isJoinChannelModalOpen && (
            <Modal onClose={this.onCloseJoinChannelModal}>
              <JoinChannelForm
                onChannelJoined={this.onChannelJoined}
                {...this.props}
              />
            </Modal>
          )}
        </div>
      </div>
    );
  }
}

export default SidebarItemJoinChannel;
