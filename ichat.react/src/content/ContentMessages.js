import React from "react";
import "./ContentMessages.css";
import "lib/simplebar.css";
import SimpleBar from "simplebar-react";
import AuthService from "services/AuthService";

class ContentMessages extends React.Component {
  constructor(props) {
    super(props);

    this.fetchData = this.fetchData.bind(this);
    this.onUpdateChannel = this.onUpdateChannel.bind(this);
    this.onReceiveMessage = this.onReceiveMessage.bind(this);
    this.authService = new AuthService(props);

    if (props.hubConnection) {
      props.hubConnection.on("UpdateChannel", this.onUpdateChannel);
      props.hubConnection.on("ReceiveMessage", this.onReceiveMessage);
    }

    this.state = {
      messageGroups: []
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
  }

  onUpdateChannel(channelId) {
    if (this.props.section === "channel" && this.props.id === channelId) {
      this.fetchData(this.props);
    }
  }

  onReceiveMessage() {
    if (this.props.section === "user") {
      this.fetchData(this.props);
    }
  }

  render() {
    return (
      <div className="message-container">
        <SimpleBar className="message-scrollable">
          {this.state.messageGroups.map(g => (
            <div key={g.dateString} className="message-group">
              <div className="message-group-header horizontal-divider">
                <span>{g.dateString}</span>
              </div>
              {g.messages.map(m => (
                <div key={m.id} className="message-item">
                  <div className="message-title">
                    {m.sender.displayName} &nbsp; {m.timeString}
                  </div>
                  <RawMessage content={m.content} />
                </div>
              ))}
            </div>
          ))}
        </SimpleBar>
      </div>
    );
  }
}

function RawMessage(props) {
  return (
    <div
      className="message-content"
      dangerouslySetInnerHTML={{ __html: props.content }}
    />
  );
}

function scrollToBottom() {
  var scrollable = document.querySelector(".simplebar-content-wrapper");
  scrollable.scrollTop = scrollable.scrollHeight;
}

export default ContentMessages;
