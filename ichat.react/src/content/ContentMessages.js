import React from "react";
import "./ContentMessages.css";
import "lib/simplebar.css";
import ContentMessageItem from "./ContentMessageItem";
import SimpleBar from "simplebar-react";
import AuthService from "services/AuthService";

class ContentMessages extends React.Component {
  constructor(props) {
    super(props);

    this.fetchData = this.fetchData.bind(this);
    this.onNewChannelMessage = this.onNewChannelMessage.bind(this);
    this.onNewConversationMessage = this.onNewConversationMessage.bind(this);
    this.authService = new AuthService(props);

    if (props.hubConnection) {
      props.hubConnection.on("NewChannelMessage", this.onNewChannelMessage);
      props.hubConnection.on(
        "NewConversationMessage",
        this.onNewConversationMessage
      );
    }

    this.state = {
      messageGroups: []
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

  fetchData(props) {
    return this.authService
      .fetch(`/api/messages/${props.section}/${props.id}`)
      .then(messageGroups => this.setState({ messageGroups }))
      .then(() => this.scrollToBottom());
  }

  componentDidMount() {
    this.fetchData(this.props).then(() => {
      this.props.onFinishLoading();
    });
  }

  componentDidUpdate(prevProps) {
    if (
      this.props.section !== prevProps.section ||
      this.props.id !== prevProps.id
    ) {
      this.fetchData(this.props);
    }

    if (this.state.messageGroups.length > 0) this.scrollDetector.init();
  }

  onNewChannelMessage(channelId) {
    if (this.props.section === "channel" && this.props.id === channelId) {
      this.fetchData(this.props);
    }
  }

  onNewConversationMessage(conversationId) {
    if (
      this.props.section === "conversation" &&
      this.props.id === conversationId
    ) {
      this.fetchData(this.props);
    }
  }

  render() {
    return (
      <div className="message-container">
        <SimpleBar className="message-scrollable">
          {this.state.messageGroups.map(g => (
            <div key={g.dateString} className="message-group">
              <div className="message-group-anchor" />
              <div className="message-group-header-container">
                <div className="message-group-header horizontal-divider">
                  <span>{g.dateString}</span>
                </div>
              </div>
              {g.messages.map(m => (
                <ContentMessageItem
                  key={m.id}
                  message={m}
                  userProfile={this.props.userProfile}
                />
              ))}
            </div>
          ))}
        </SimpleBar>
      </div>
    );
  }
}

export default ContentMessages;
