import React from "react";
import "./ContentMessages.css";
import "./simplebar.css";
import SimpleBar from 'simplebar';

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
      .then(messages => this.setState({ messages }));
  }

  componentDidMount() {
    this.fetchData(this.props).then(()=>
    initSimpleBar());
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
        <div className="message-scrollable">
          {this.state.messages.map(m => (
            <div key={m.id} className="message-item">
              <div className="message-title">
                {m.sender.displayName} &nbsp; {m.createdDate}
              </div>
              <RawMessage content={m.content} />
            </div>
          ))}
        </div>
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

function initSimpleBar() {
  var messageScrollable = document.querySelector(".message-scrollable");

  // init simplebar
  new SimpleBar(messageScrollable);

  var scrollable = document.querySelector(".simplebar-content");
  scrollable.scrollTop = scrollable.scrollHeight;
  messageScrollable.style.visibility = "visible"; // show now to avoid flickering
}

export default ContentMessages;
