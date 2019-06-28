import React from "react";
import "./ContentMessages.css";
import "lib/simplebar.css";
import ContentMessageItem from "./ContentMessageItem";
import ContentMessageItemEditor from "./ContentMessageItemEditor";
import SimpleBar from "simplebar-react";
import ApiService from "services/ApiService";
import MessageChangeService from "services/MessageChangeService";

class ContentMessages extends React.Component {
  constructor(props) {
    super(props);

    this.fetchHistory = this.fetchHistory.bind(this);
    this.onChannelMessageItemChange = this.onChannelMessageItemChange.bind(
      this
    );
    this.onConversationMessageItemChange = this.onConversationMessageItemChange.bind(
      this
    );
    this.onEditMessageClicked = this.onEditMessageClicked.bind(this);
    this.onCloseEditingMessage = this.onCloseEditingMessage.bind(this);
    this.onLoadMoreHistory = this.onLoadMoreHistory.bind(this);

    this.apiService = new ApiService(props);
    this.mesageChangeService = new MessageChangeService();

    if (props.hubConnection) {
      props.hubConnection.on(
        "ChannelMessageItemChange",
        this.onChannelMessageItemChange
      );
      props.hubConnection.on(
        "ConversationMessageItemChange",
        this.onConversationMessageItemChange
      );
    }

    this.currentPage = 1;
    this.state = {
      messageGroups: [],
      editingMessageId: undefined
    };

    this.scrollDetector = {
      init: function() {
        this.calculateMessageGroupOffsetTops();
        this.scrollableElement = document.querySelector(
          ".message-container .simplebar-content-wrapper"
        );
        this.scrollableElement.onscroll = this.handler.bind(this);
        window.onresize = this.calculateMessageGroupOffsetTops;
        this.lastGroupIndex = -1;
        this.handler();
      },
      scrollableElement: undefined,
      lastGroupIndex: -1,
      offsetTops: [],
      messageGroupAnchors: [],
      calculateMessageGroupOffsetTops: function() {
        this.messageGroupAnchors = Array.from(
          document.querySelectorAll(".message-group-anchor")
        );
        this.offsetTops = this.messageGroupAnchors.map(function(a) {
          return a.offsetTop;
        });
      },
      handler: function() {
        var adjustHeight = this.messageGroupAnchors[0].offsetHeight / 2;
        var currentGroupIndex = -1;
        var startIndex = this.lastGroupIndex - 1;
        startIndex = startIndex < 0 ? 0 : startIndex;
        for (var i = startIndex; i < this.offsetTops.length; i++) {
          if (
            this.scrollableElement.scrollTop <
            this.offsetTops[i] + adjustHeight
          ) {
            break;
          }
          currentGroupIndex = i;
        }
        if (this.lastGroupIndex !== currentGroupIndex) {
          let currentAnchor = document.querySelector(
            ".message-group-anchor.sticky-on-top"
          );
          currentAnchor && currentAnchor.classList.remove("sticky-on-top");
          if (currentGroupIndex !== -1) {
            this.messageGroupAnchors[currentGroupIndex].classList.add(
              "sticky-on-top"
            );
          }
          this.lastGroupIndex = currentGroupIndex;
        }
      }
    };
  }

  scrollToBottom() {
    var scrollable = document.querySelector(
      ".message-container .simplebar-content-wrapper"
    );
    scrollable.scrollTop = scrollable.scrollHeight;
  }

  fetchHistory() {
    return this.apiService
      .fetch(
        `/api/messages/${this.props.section}/${this.props.id}/${
          this.currentPage
        }`
      )
      .then(messageLoad => {
        this.areAllPagesLoaded = messageLoad.totalPage === this.currentPage;
        let updatedMessageGroups = this.mesageChangeService.mergeMessageGroups(
          messageLoad.messageGroupDtos,
          this.state.messageGroups
        );
        this.setState({ messageGroups: updatedMessageGroups });
      });
  }

  fetchSingleMessage(messageId) {
    return this.apiService.fetch(
      `/api/messages/${this.props.section}/${
        this.props.id
      }/singleMessage/${messageId}`
    );
  }

  handleMessageItemChange(changeType, messageId) {
    switch (changeType) {
      case this.mesageChangeService.CHANGE_TYPE.ADDED:
        this.handleAddedMessageItem(messageId);
        break;
      case this.mesageChangeService.CHANGE_TYPE.EDITED:
        this.handleEditedMessageItem(messageId);
        break;
      case this.mesageChangeService.CHANGE_TYPE.DELETED:
        this.handleDeletedMessageItem(messageId);
        break;
    }
  }

  handleAddedMessageItem(messageId) {
    this.fetchSingleMessage(messageId).then(messageGroupDto => {
      let updatedMessageGroups = this.mesageChangeService.mergeMessageGroups(
        this.state.messageGroups,
        [messageGroupDto]
      );
      this.setState({ messageGroups: updatedMessageGroups }, () => {
        if (messageGroupDto.messages[0].senderId === this.props.userProfile.id)
          this.scrollToBottom();
      });
    });
  }

  handleEditedMessageItem(messageId) {
    this.fetchSingleMessage(messageId).then(messageGroupDto => {
      let updatedMessageGroups = this.mesageChangeService.handleEditedMessageItem(
        messageGroupDto,
        this.state.messageGroups
      );
      if (updatedMessageGroups)
        this.setState({ messageGroups: updatedMessageGroups });
    });
  }

  handleDeletedMessageItem(messageId) {
    let updatedMessageGroups = this.mesageChangeService.handleDeletedMessageItem(
      messageId,
      this.state.messageGroups
    );
    if (updatedMessageGroups)
      this.setState({ messageGroups: updatedMessageGroups });
  }

  onChannelMessageItemChange(channelId, changeType, messageId) {
    if (this.props.section === "channel" && this.props.id === channelId) {
      this.handleMessageItemChange(changeType, messageId);
    }
  }

  onConversationMessageItemChange(conversationId, changeType, messageId) {
    if (
      this.props.section === "conversation" &&
      this.props.id === conversationId
    ) {
      this.handleMessageItemChange(changeType, messageId);
    }
  }

  onEditMessageClicked(messageId) {
    this.setState({ editingMessageId: messageId });
  }

  onCloseEditingMessage() {
    this.setState({ editingMessageId: undefined });
  }

  onLoadMoreHistory() {
    this.currentPage++;
    this.fetchHistory();
  }

  componentDidMount() {
    this.fetchHistory()
      .then(() => {
        this.props.onFinishLoading();
      })
      .then(() => this.scrollToBottom());
  }

  componentDidUpdate(prevProps) {
    if (
      this.props.section !== prevProps.section ||
      this.props.id !== prevProps.id
    ) {
      this.currentPage = 1;
      this.setState({ messageGroups: [] }, () => this.fetchHistory());
    }

    if (this.state.messageGroups.length > 0) this.scrollDetector.init();
  }

  render() {
    return (
      <div className="message-container">
        <SimpleBar className="message-scrollable">
          {this.state.messageGroups.map((g, index) => (
            <div key={g.dateString} className="message-group">
              <div className="message-group-anchor" />
              <div className="message-group-header-container">
                <div className="message-group-header horizontal-divider">
                  <span>{g.dateString}</span>
                </div>
              </div>
              {index === 0 && !this.areAllPagesLoaded && (
                <div
                  className="message-load-history"
                  onClick={this.onLoadMoreHistory}
                >
                  Loading history...
                </div>
              )}
              {g.messages.map(m => (
                <div key={m.id}>
                  {this.state.editingMessageId !== m.id && (
                    <ContentMessageItem
                      message={m}
                      onEditMessageClicked={this.onEditMessageClicked}
                      {...this.props}
                    />
                  )}
                  {this.state.editingMessageId === m.id && (
                    <ContentMessageItemEditor
                      className="content-message-item-container"
                      message={m}
                      onClose={this.onCloseEditingMessage}
                      {...this.props}
                    />
                  )}
                </div>
              ))}
            </div>
          ))}
        </SimpleBar>
      </div>
    );
  }
}

export default ContentMessages;
