import React from "react";
import "./ContentMessages.css";
import "./simplebar.css";
import signalR from "@aspnet/signalr";
import SimpleBar from "simplebar-react";
import AuthService from "./services/AuthService";

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
      messages: []
    };
  }

  fetchData(props) {
    let section = props.section || "channel";
    let id = props.id || 0;
    return this.authService
      .fetch(`/api/messages/${section}/${id}`)
      .then(messages => this.setState({ messages }))
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

  onUpdateChannel(channelId){
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
          {this.state.messages.map(m => (
            <div key={m.id} className="message-item">
              <div className="message-title">
                {m.sender.displayName} &nbsp; {m.createdDate}
              </div>
              <RawMessage content={m.content} />
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
