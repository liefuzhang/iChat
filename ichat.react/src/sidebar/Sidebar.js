import React from "react";
import "./Sidebar.css";
import SidebarItemConversation from "./SidebarItem.Conversation";
import SidebarItemChannel from "./SidebarItem.Channel";
import SidebarItemJoinChannel from "./SidebarItem.JoinChannel";
import Modal from "modals/Modal";
import CreateChannelForm from "modalForms/CreateChannelForm";
import StartConversationForm from "modalForms/StartConversationForm";
import SimpleBar from "simplebar-react";
import "lib/simplebar.css";
import { Loader, Image, Segment } from "semantic-ui-react";
import { Icon, Popup } from "semantic-ui-react";
import ApiService from "services/ApiService";

class Sidebar extends React.Component {
  constructor(props) {
    super(props);

    this.apiService = new ApiService(props);
    this.onCreateChannel = this.onCreateChannel.bind(this);
    this.onCloseCreateChannel = this.onCloseCreateChannel.bind(this);
    this.onChannelCreated = this.onChannelCreated.bind(this);
    this.onStartConversation = this.onStartConversation.bind(this);
    this.onCloseStartConversation = this.onCloseStartConversation.bind(this);
    this.onConversationStarted = this.onConversationStarted.bind(this);
    this.onUpdateChannelList = this.onUpdateChannelList.bind(this);
    this.onUpdateConversationList = this.onUpdateConversationList.bind(this);

    if (props.hubConnection) {
      props.hubConnection.on(
        "ChannelMessageItemChanged",
        this.onUpdateChannelList
      );
      props.hubConnection.on(
        "ConversationMessageItemChanged",
        this.onUpdateConversationList
      );
      props.hubConnection.on("UnreadChannelMessageCleared", this.onUpdateChannelList);
      props.hubConnection.on(
        "UnreadConversationMessageCleared",
        this.onUpdateConversationList
      );
      props.hubConnection.on(
        "ConversationUserListChanged",
        this.onUpdateConversationList
      );
      props.hubConnection.on("UserWentOnline", this.onUpdateConversationList);
      props.hubConnection.on("UserStatusChanged", this.onUpdateConversationList);
    }

    this.state = {
      channels: [],
      conversations: [],
      isCreateChannelModalOpen: false,
      isPageLoading: true,
      unreadMessage: 0,
      selectedChannel:{},
      selectedConversation: {}
    };
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

  updatePageTitle() {
    var unreadMessageText = "";
    this.setState({unreadMessage:this.getUnreadMessageCount()});
    if(this.state.unreadMessage > 0 &&this.state.unreadMessage != null)
    {
      unreadMessageText = " | " + this.state.unreadMessage + " new items"
    }
    this.fetchData(this.props)
      .then(()=>
      {document.title = "iChat | "+ (this.props.isChannel? this.state.selectedChannel.name + unreadMessageText :this.state.selectedConversation.name + unreadMessageText)});
    
  }

  fetchChannels() {
    return this.apiService
      .fetch("/api/channels/forUser")
      .then(channels => {
        this.setState({ channels: channels });
        this.updatePageTitle();
      });
  }

  fetchConversations() {
    return this.apiService
      .fetch("/api/conversations/recent")
      .then(conversations => {
        this.setState({ conversations: conversations })
        this.updatePageTitle();
      });
  }

  fetchData(props) {
    if (props.isChannel) {
      return this.apiService
        .fetch(`/api/channels/${props.id}`)
        .then(channel => this.setState({ selectedChannel: channel }));
    } else {
      return this.apiService
        .fetch(`/api/conversations/${props.id}`)
        .then(conversation =>
          this.setState({ selectedConversation: conversation })
        );
    }
  }

  componentDidMount() {
    Promise.all([this.fetchChannels(), this.fetchConversations()]).then(() => {
      this.setState({ isPageLoading: false });
    });
  }

  getUnreadMessageCount() {
    let unreadMessage = 0;
    this.state.conversations.forEach(c=>unreadMessage+=c.unreadMessageCount)
    this.state.channels.forEach(c=>unreadMessage+=c.unreadMentionCount)
    return unreadMessage
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
        <SimpleBar className="sidebar-scrollable">
          <section>
            <div className="section-title">
              <span>CHANNELS</span>
              <Popup
                trigger={
                  <Icon
                    name="plus circle"
                    className="icon-circle"
                    onClick={this.onCreateChannel}
                  />
                }
                content="Create a channel"
                inverted
                position="bottom center"
                size="tiny"
              />
            </div>
            {this.state.channels.map(c => {
              let active = false;
              if (this.props.isChannel && this.props.id === c.id) active = true;
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
              <Popup
                trigger={
                  <Icon
                    name="plus circle"
                    className="icon-circle"
                    onClick={this.onStartConversation}
                  />
                }
                content="Start a conversation"
                inverted
                position="top center"
                size="tiny"
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
