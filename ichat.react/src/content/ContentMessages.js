import React from "react";
import "./ContentMessages.css";
import "lib/simplebar.css";
import ContentMessageItem from "./ContentMessageItem";
import ContentMessageItemEditor from "./ContentMessageItemEditor";
import ContentMessagesChannelDescription from "./ContentMessages.ChannelDescription";
import ContentMessagesConversationDescription from "./ContentMessages.ConversationDescription";
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
    this.onImageLoadingFinished = this.onImageLoadingFinished.bind(this);

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
        "ChannelMessageItemChanged",
        this.onChannelMessageItemChange
      );
      props.hubConnection.on(
        "ConversationMessageItemChanged",
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
    this.areAllPagesLoaded = false;
    this.isFetchingHistory = false;
    this.isFetchingSingleMessage = false;
    this.resetLoadImage();
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
          this.setNewImageFileCount(messageLoad.messageGroupDtos);
          let updatedMessageGroups = this.mesageChangeService.mergeMessageGroups(
            messageLoad.messageGroupDtos,
            this.state.messageGroups
          );
          if (!isLoadingMore)
            this.messageChannelDescriptionDto =
              messageLoad.messageChannelDescriptionDto;
          this.updateMessageGroups(updatedMessageGroups, () => {
            if (isLoadingMore) this.messageScrollService.resumeScrollPosition();
            else {
              this.messageScrollService.scrollToBottom();
              this.props.onFinishLoading();
            }
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
          this.setNewImageFileCount([messageGroupDto]);
          let newMessage = messageGroupDto.messages[0];
          let updatedMessageGroups = this.mesageChangeService.mergeMessageGroups(
            this.state.messageGroups,
            [messageGroupDto]
          );
          this.updateMessageGroups(updatedMessageGroups, () => {
            if (newMessage.senderId === this.props.userProfile.id)
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
            this.updateMessageGroups(updatedMessageGroups);
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
    if (updatedMessageGroups) this.updateMessageGroups(updatedMessageGroups);
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

  updateMessageGroups(messageGroups, callback) {
    this.setState({ messageGroups: messageGroups }, () => {
      if (callback) this.loadImage.imagesLoadedCallback = callback;
      if (this.loadImage.loadedImageCount === this.loadImage.imageFileCount) {
        this.finishLoading();
      } else {
        let timeoutInSeconds = 5;
        this.imageLoadTimeout = setTimeout(() => {
          this.finishLoading();
        }, timeoutInSeconds * 1000);
      }
    });
  }

  setNewImageFileCount(newMessageGroups) {
    newMessageGroups &&
      newMessageGroups.forEach(group => {
        let fileMessages = group.messages.filter(m => m.hasFileAttachments);
        fileMessages.forEach(fileMessage => {
          fileMessage.fileAttachments.forEach(file => {
            if (file.contentType.startsWith("image"))
              this.loadImage.imageFileCount++;
          });
        });

        let messagesContainingUrlLinks = group.messages.filter(
          m => m.containsOpenGraphObjects
        );
        messagesContainingUrlLinks.forEach(message => {
          this.loadImage.imageFileCount += message.openGraphImageCount;
        });
      });
  }

  onImageLoadingFinished() {
    this.loadImage.loadedImageCount++;
    if (this.loadImage.loadedImageCount === this.loadImage.imageFileCount) {
      this.imageLoadTimeout && clearTimeout(this.imageLoadTimeout);
      this.finishLoading();
    }
  }

  finishLoading() {
    if (this.state.messageGroups.length > 0) this.messageScrollService.reset();
    if (this.loadImage.imagesLoadedCallback)
      this.loadImage.imagesLoadedCallback();
    this.resetLoadImage();
  }

  resetLoadImage() {
    this.loadImage = {
      imageFileCount: 0,
      loadedImageCount: 0,
      imagesLoadedCallback: undefined
    };
  }

  componentDidMount() {
    this.fetchHistory(false);
  }

  componentDidUpdate(prevProps) {
    if (
      this.props.section !== prevProps.section ||
      this.props.id !== prevProps.id
    ) {
      this.resetMessage();
      this.updateMessageGroups([], () => this.fetchHistory(false));
    }
  }

  render() {
    return (
      <div className="message-container">
        <SimpleBar className="message-scrollable">
          {this.areAllPagesLoaded && (
            <div className="message-group message-content-description">
              <div className="message-group-anchor" />
              {this.props.isChannel ? (
                <ContentMessagesChannelDescription
                  messageChannelDescriptionDto={
                    this.messageChannelDescriptionDto
                  }
                  userProfile={this.props.userProfile}
                />
              ) : (
                this.props.messageChannelUserList &&
                this.props.messageChannelUserList.length > 0 && (
                  <ContentMessagesConversationDescription
                    userList={this.props.messageChannelUserList}
                    userProfile={this.props.userProfile}
                  />
                )
              )}
            </div>
          )}
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
                      onImageLoadingFinished={this.onImageLoadingFinished}
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
