import React from "react";
import "./ContentMessages.css";
import "lib/simplebar.css";
import ContentMessageItem from "./ContentMessageItem";
import ContentMessageItemEditor from "./ContentMessageItemEditor";
import SimpleBar from "simplebar-react";
import ApiService from "services/ApiService";
import moment from "moment";

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

    this.resetMessages();

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
        this.currentMessageGroups = this.mergeMessageGroups(
          messageLoad.messageGroupDtos,
          this.currentMessageGroups
        );
        this.setState({ messageGroups: this.currentMessageGroups });
      });
  }

  fetchSingleMessage(messageId) {
    return this.apiService.fetch(
      `/api/messages/${this.props.section}/${
        this.props.id
      }/singleMessage/${messageId}`
    );
  }

  resetMessages() {
    this.currentPage = 1;
    this.currentMessageGroups = [];
  }

  setIsConsecutiveMessage(newerMessage, olderMessage) {
    let newerMessageDate = moment(newerMessage.timeString, "h:mm tt");
    let olderMessageDate = moment(olderMessage.timeString, "h:mm tt");
    if (newerMessageDate - olderMessageDate <= 3 * 60 * 1000)
      newerMessage.isConsecutiveMessage = true;
  }

  mergeMessageGroups(olderMessageGroups, newerMessageGroups) {
    let mergedMessageGroups = [];
    if (
      newerMessageGroups.length > 0 &&
      olderMessageGroups[olderMessageGroups.length - 1].dateString ===
        newerMessageGroups[0].dateString
    ) {
      // when message group has overlapping date
      let firstNewerMessageGroupMessages = newerMessageGroups[0].messages;
      let lastOlderMessageGroupMessage =
        olderMessageGroups[olderMessageGroups.length - 1].messages;
      this.setIsConsecutiveMessage(
        firstNewerMessageGroupMessages[0],
        lastOlderMessageGroupMessage[lastOlderMessageGroupMessage.length - 1]
      );
      newerMessageGroups[0].messages = lastOlderMessageGroupMessage.concat(
        firstNewerMessageGroupMessages
      );
      mergedMessageGroups = olderMessageGroups
        .slice(0, olderMessageGroups.length - 1)
        .concat(newerMessageGroups);
    } else {
      mergedMessageGroups = olderMessageGroups.concat(newerMessageGroups);
    }

    return mergedMessageGroups;
  }

  onChannelMessageItemChange(channelId, changeType, messageId) {
    if (this.props.section === "channel" && this.props.id === channelId) {
      switch (changeType) {
        case 1:
          this.fetchSingleMessage(messageId).then(messageGroupDto => {
            this.currentMessageGroups = this.mergeMessageGroups(
              this.currentMessageGroups,
              [messageGroupDto]
            );
            this.setState({ messageGroups: this.currentMessageGroups }, () => {
              if (
                messageGroupDto.messages[0].senderId ===
                this.props.userProfile.id
              )
                this.scrollToBottom();
            });
          });
      }
    }
  }

  onConversationMessageItemChange(conversationId, changeType, messageId) {
    if (
      this.props.section === "conversation" &&
      this.props.id === conversationId
    ) {
      this.fetchHistory();
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
      this.resetMessages();
      this.fetchHistory();
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
