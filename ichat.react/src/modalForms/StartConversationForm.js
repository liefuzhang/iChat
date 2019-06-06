import React from "react";
import "./StartConversationForm.css";
import AuthService from "services/AuthService";

class StartConversationForm extends React.Component {
  constructor(props) {
    super(props);

    this.onStartConversationFormSubmit = this.onStartConversationFormSubmit.bind(
      this
    );
    this.authService = new AuthService(props);
  }

  onStartConversationFormSubmit(event) {
    event.preventDefault();
    // let name = event.target.elements["name"].value;
    // let topic = event.target.elements["topic"].value;

    // this.authService
    //   .fetch(`/api/channels`, {
    //     method: "POST",
    //     body: JSON.stringify({
    //       name: name,
    //       topic: topic
    //     })
    //   })
    //   .then(id => {
    //     this.props.onChannelCreated();
    //     this.props.history.push(`/channel/${id}`);
    //   });
  }

  componentDidMount() {
  }

  render() {
    return (
      <div className="form-container">
        <h1 style={{ textAlign: "center" }}>Start conversation</h1>

        {/* <form
          id="StartConversationForm"
          method="post"
          onSubmit={this.onStartConversationFormSubmit}
        >
          <p>Channels are where specific topics can be talked about.</p>
          <div className="form-group">
            <label htmlFor="OpenDirectMessageName">Name</label>
            <input
              className="form-control"
              type="text"
              id="OpenDirectMessageName"
              name="name"
              placeholder="e.g. meetings"
              required
            />
          </div>
          <div className="form-group">
            <label htmlFor="OpenDirectMessageTopic">
              Topic<span className="secondary-text">&nbsp;(optional)</span>
            </label>
            <input
              className="form-control"
              type="text"
              id="OpenDirectMessageTopic"
              name="topic"
              placeholder="What is this channel about?"
            />
          </div>
          <button type="submit" className="btn form-control">
            Create Channel
          </button>
        </form> */}
      </div>
    );
  }
}

export default StartConversationForm;
