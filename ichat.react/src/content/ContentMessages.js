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
    this.onUpdateChannel = this.onUpdateChannel.bind(this);
    this.onUpdateConversation = this.onUpdateConversation.bind(this);
    this.authService = new AuthService(props);

    if (props.hubConnection) {
      props.hubConnection.on("UpdateChannel", this.onUpdateChannel);
      props.hubConnection.on("UpdateConversation", this.onUpdateConversation);
    }

    this.state = {
      messageGroups: []
    };

    this.scrollDetector = {
      init: function() {
        this.calculateMessageGroupOffsetTops();
        this.scrollableElement = document.querySelector(
          ".simplebar-content-wrapper"
        );
        this.scrollableElement.onscroll = this.handler.bind(this);
        window.onresize = this.calculateMessageGroupOffsetTops;
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
          this.lastSectionIndex = currentGroupIndex;
        }
      }
    };
  }

  fetchData(props) {
    return this.authService
      .fetch(`/api/messages/${props.section}/${props.id}`)
      .then(messageGroups => this.setState({ messageGroups }))
      .then(() => scrollToBottom());
  }

  componentDidMount() {
    this.fetchData(this.props).then(() => {
      var messageScrollable = document.querySelector(".message-scrollable");
      messageScrollable.style.visibility = "visible"; // show now to avoid flickering
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

  onUpdateChannel(channelId) {
    if (this.props.section === "channel" && this.props.id === channelId) {
      this.fetchData(this.props);
    }
  }

  onUpdateConversation(conversationId) {
    if (this.props.section === "conversation" && this.props.id === conversationId) {
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
                <ContentMessageItem key={m.id} message={m} userProfile={this.props.userProfile} />
              ))}
            </div>
          ))}
        </SimpleBar>
      </div>
    );
  }
}

function scrollToBottom() {
  var scrollable = document.querySelector(".simplebar-content-wrapper");
  scrollable.scrollTop = scrollable.scrollHeight;
}

export default ContentMessages;
