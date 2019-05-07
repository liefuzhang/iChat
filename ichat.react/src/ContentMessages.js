import React from "react";
import "./ContentMessages.css";
import "./simplebar.css";
import SimpleBar from "simplebar-react";

class ContentMessages extends React.Component {
  constructor(props) {
    super(props);

    this.fetchData = this.fetchData.bind(this);
    this.state = {
      messages: []
    };
  }

  fetchData(props) {
    return fetch(`/api/messages/${props.section}/${props.id}`)
      .then(response => response.json())
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
