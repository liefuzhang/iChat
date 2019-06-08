import React from "react";
import "./Sidebar.css";
import SidebarItemConversation from "./SidebarItem.Conversation";
import SidebarItemChannel from "./SidebarItem.Channel";
import SidebarHeader from "./SidebarHeader";
import AuthService from "services/AuthService";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Modal from "modals/Modal";
import CreateChannelForm from "modalForms/CreateChannelForm";
import StartConversationForm from "modalForms/StartConversationForm";

class Sidebar extends React.Component {
  constructor(props) {
    super(props);

    this.authService = new AuthService(props);
    this.onCreateChannel = this.onCreateChannel.bind(this);
    this.onCloseCreateChannel = this.onCloseCreateChannel.bind(this);
    this.onChannelCreated = this.onChannelCreated.bind(this);
    this.onStartConversation = this.onStartConversation.bind(this);
    this.onCloseStartConversation = this.onCloseStartConversation.bind(this);
    this.onConversationStarted = this.onConversationStarted.bind(this);
    this.onUpdateChannelList = this.onUpdateChannelList.bind(this);
    this.onUpdateConversationList = this.onUpdateConversationList.bind(this);

    if (props.hubConnection) {
      props.hubConnection.on("NewChannelMessage", this.onUpdateChannelList);
      props.hubConnection.on("UpdateChannelList", this.onUpdateChannelList);
      props.hubConnection.on(
        "NewConversationMessage",
        this.onUpdateConversationList
      );
      props.hubConnection.on(
        "UpdateConversationList",
        this.onUpdateConversationList
      );
    }

    this.state = {
      channels: [],
      conversations: [],
      isCreateChannelModalOpen: false
    };

    this.fecthData();
  }

  onCreateChannel(event) {
    this.setState({
      isCreateChannelModalOpen: true
    });
  }

  onCloseCreateChannel() {
    this.setState({
      isCreateChannelModalOpen: false
    });
  }

  onChannelCreated() {
    this.onCloseCreateChannel();
    this.fetchChannels();
  }

  onUpdateChannelList() {
    this.fetchChannels();
  }

  onStartConversation(event) {
    this.setState({
      isStartConversationModalOpen: true
    });
  }

  onCloseStartConversation() {
    this.setState({
      isStartConversationModalOpen: false
    });
  }

  onConversationStarted() {
    this.onCloseStartConversation();
    this.fetchConversations();
  }

  onUpdateConversationList() {
    this.fetchConversations();
  }

  fetchChannels() {
    this.authService
      .fetch("/api/channels")
      .then(channels => this.setState({ channels }));
  }

  fetchConversations() {
    this.authService
      .fetch("/api/conversations")
      .then(conversations => this.setState({ conversations: conversations }));
  }

  fecthData() {
    this.fetchChannels();
    this.fetchConversations();
  }

  render() {
    return (
      <div id="sidebar">
        <section>
          <SidebarHeader {...this.props} />
        </section>
        <section>
          <div className="section-title">
            <span>CHANNELS</span>
            <FontAwesomeIcon
              icon="plus-circle"
              title="Create a channel"
              className="icon-circle"
              onClick={this.onCreateChannel}
            />
          </div>
          {this.state.channels.map(c => {
            let active = false;
            if (this.props.isChannel && this.props.id === c.id) active = true;
            if (
              this.props.isChannel &&
              this.props.id === 0 &&
              c.name === "general"
            )
              active = true;
            return (
              <SidebarItemChannel key={c.id} channel={c} active={active} />
            );
          })}
          <div>
            {this.state.isCreateChannelModalOpen && (
              <Modal onClose={this.onCloseCreateChannel}>
                <CreateChannelForm
                  onClose={this.onCloseCreateChannel}
                  onChannelCreated={this.onChannelCreated}
                  {...this.props}
                />
              </Modal>
            )}
          </div>
        </section>

        <section>
          <div className="section-title">
            <span>DIRECT MESSAGES</span>
            <FontAwesomeIcon
              icon="plus-circle"
              title="Start a conversation"
              className="icon-circle"
              onClick={this.onStartConversation}
            />
          </div>
          {this.state.conversations.map(c => {
            let active = false;
            if (!this.props.isChannel && this.props.id === c.id) active = true;
            return (
              <SidebarItemConversation
                key={c.id}
                conversation={c}
                active={active}
              />
            );
          })}
          <div>
            {this.state.isStartConversationModalOpen && (
              <Modal onClose={this.onCloseStartConversation}>
                <StartConversationForm
                  onClose={this.onCloseStartConversation}
                  onConversationStarted={this.onConversationStarted}
                  {...this.props}
                />
              </Modal>
            )}
          </div>
        </section>
      </div>
    );
  }
}

export default Sidebar;
