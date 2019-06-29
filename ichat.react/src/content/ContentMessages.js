import React from "react";
import "./ContentMessages.css";
import "lib/simplebar.css";
import ContentMessageItem from "./ContentMessageItem";
import ContentMessageItemEditor from "./ContentMessageItemEditor";
import SimpleBar from "simplebar-react";
import ApiService from "services/ApiService";
import MessageChangeService from "services/MessageChangeService";
import MessageScrollService from "../services/MessageScrollService";

class ContentMessages extends React.Component {
  constructor(props) {
    super(props);

    this.fetchHistory = this.fetchHistory.bind(this);
    this.loadMoreHistory = this.loadMoreHistory.bind(this);
    this.onChannelMessageItemChange = this.onChannelMessageItemChange.bind(
      this
    );
    this.onConversationMessageItemChange = this.onConversationMessageItemChange.bind(
      this
    );
    this.onEditMessageClicked = this.onEditMessageClicked.bind(this);
    this.onCloseEditingMessage = this.onCloseEditingMessage.bind(this);
    this.onScrollToTop = this.onScrollToTop.bind(this);

    this.apiService = new ApiService(props);
    this.mesageChangeService = new MessageChangeService();
    this.messageScrollService = new MessageScrollService({
      scrollableElementSelector:
        ".message-container .simplebar-content-wrapper",
      anchorSelector: ".message-group-anchor",
      stickyItemClassName: "sticky-on-top",
      onScrollToTop: this.onScrollToTop
    });

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

    this.resetMessage();
    this.state = {
      messageGroups: [],
      editingMessageId: undefined
    };

    this.scrollDetector = {};
  }

  onScrollToTop() {
    if (!this.areAllPagesLoaded && !this.isFetchingHistory)
      this.loadMoreHistory();
  }

  resetMessage() {
    this.currentPage = 0;
    this.isFetchingHistory = false;
    this.isFetchingSingleMessage = false;
  }

  loadMoreHistory() {
    this.fetchHistory(true);
  }

  fetchHistory(isLoadingMore) {
    this.isFetchingHistory = true;
    this.currentPage++;
    let section = this.props.section;
    let id = this.props.id;

    return this.apiService
      .fetch(`/api/messages/${section}/${id}/${this.currentPage}`)
      .then(messageLoad => {
        if (
          this.isFetchingHistory &&
          section === this.props.section &&
          id === this.props.id
        ) {
          this.areAllPagesLoaded = messageLoad.totalPage === this.currentPage;
          let updatedMessageGroups = this.mesageChangeService.mergeMessageGroups(
            messageLoad.messageGroupDtos,
            this.state.messageGroups
          );
          this.setState({ messageGroups: updatedMessageGroups }, () => {
            if (isLoadingMore) this.messageScrollService.resumeScrollPosition();
            else this.messageScrollService.scrollToBottom();
          });
        }
      })
      .catch(() => {
        this.currentPage--;
      })
      .finally(() => {
        this.isFetchingHistory = false;
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
    this.isFetchingSingleMessage = true;
    this.fetchSingleMessage(messageId)
      .then(messageGroupDto => {
        if (this.isFetchingSingleMessage) {
          let updatedMessageGroups = this.mesageChangeService.mergeMessageGroups(
            this.state.messageGroups,
            [messageGroupDto]
          );
          this.setState({ messageGroups: updatedMessageGroups }, () => {
            if (
              messageGroupDto.messages[0].senderId === this.props.userProfile.id
            )
              this.messageScrollService.scrollToBottom();
          });
        }
      })
      .finally(() => {
        this.isFetchingSingleMessage = false;
      });
  }

  handleEditedMessageItem(messageId) {
    this.isFetchingSingleMessage = true;
    this.fetchSingleMessage(messageId)
      .then(messageGroupDto => {
        if (this.isFetchingSingleMessage) {
          let updatedMessageGroups = this.mesageChangeService.handleEditedMessageItem(
            messageGroupDto,
            this.state.messageGroups
          );
          if (updatedMessageGroups)
            this.setState({ messageGroups: updatedMessageGroups });
        }
      })
      .finally(() => {
        this.isFetchingSingleMessage = false;
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

  componentDidMount() {
    this.fetchHistory(false).then(() => {
      this.props.onFinishLoading();
    });
  }

  componentDidUpdate(prevProps) {
    if (
      this.props.section !== prevProps.section ||
      this.props.id !== prevProps.id
    ) {
      this.resetMessage();
      this.setState({ messageGroups: [] }, () => this.fetchHistory(false));
    }

    if (this.state.messageGroups.length > 0) this.messageScrollService.reset();
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
                <div className="message-load-history">Loading history...</div>
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
