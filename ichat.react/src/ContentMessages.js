import React from "react";
import "./ContentMessages.css";

class ContentMessages extends React.Component {
  constructor(props) {
    super(props);

    this.fetchData = this.fetchData.bind(this);
    this.state = {
      messages: []
    };
  }

  fetchData(props) {
    fetch(`/api/messages/${props.section}/${props.id}`)
      .then(response => response.json())
      .then(messages => this.setState({ messages }));
  }

  componentWillMount() {
    this.fetchData(this.props);
  }

  componentWillReceiveProps(nextProps) {
    if (
      this.propssection !== nextProps.section ||
      this.props.id !== nextProps.id
    ) {
      this.fetchData(nextProps);
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

export default ContentMessages;