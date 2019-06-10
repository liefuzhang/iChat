import React from "react";
import "./Sidebar.css";
import SidebarItemConversation from "./SidebarItem.Conversation";
import SidebarItemChannel from "./SidebarItem.Channel";
import SidebarItemJoinChannel from "./SidebarItem.JoinChannel";
import SidebarHeader from "./SidebarHeader";
import AuthService from "services/AuthService";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Modal from "modals/Modal";
import CreateChannelForm from "modalForms/CreateChannelForm";
import StartConversationForm from "modalForms/StartConversationForm";
import SimpleBar from "simplebar-react";
import "lib/simplebar.css";
import { Loader, Image, Segment } from "semantic-ui-react";

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
      isCreateChannelModalOpen: false,
      isPageLoading: true
    };

    this.fecthDataOnLoad();
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
    return this.authService
      .fetch("/api/channels/forUser")
      .then(channels => this.setState({ channels }));
  }

  fetchConversations() {
    return this.authService
      .fetch("/api/conversations/recent")
      .then(conversations => this.setState({ conversations: conversations }));
  }

  fecthDataOnLoad() {
    Promise.all([this.fetchChannels(), this.fetchConversations()]).then(() => {
      setTimeout(() => {
        this.setState({ isPageLoading: false });
      }, 1000);
    });
  }

  render() {
    return (
      <div id="sidebar">
        {this.state.isPageLoading && (
          <Segment>
            <Loader active />
            <Image src="https://react.semantic-ui.com/images/wireframe/short-paragraph.png" />
            <Image src="https://react.semantic-ui.com/images/wireframe/short-paragraph.png" />
          </Segment>
        )}
        <section className="sidebar-header">
          <SidebarHeader {...this.props} />
        </section>
        <SimpleBar className="sidebar-scrollable">
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
                    onChannelCreated={this.onChannelCreated}
                    {...this.props}
                  />
                </Modal>
              )}
            </div>
          </section>

          <section>
            <SidebarItemJoinChannel
              onUpdateChannelList={this.onUpdateChannelList}
              {...this.props}
            />
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
              if (!this.props.isChannel && this.props.id === c.id)
                active = true;
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
                    onConversationStarted={this.onConversationStarted}
                    {...this.props}
                  />
                </Modal>
              )}
            </div>
          </section>
        </SimpleBar>
      </div>
    );
  }
}

export default Sidebar;
