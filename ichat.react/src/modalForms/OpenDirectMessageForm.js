import React from "react";
import "./OpenDirectMessageForm.css";
import AuthService from "services/AuthService";

class OpenDirectMessageForm extends React.Component {
  constructor(props) {
    super(props);

    this.onOpenDirectMessageFormSubmit = this.onOpenDirectMessageFormSubmit.bind(this);
    this.authService = new AuthService(props);
  }

  onOpenDirectMessageFormSubmit(event) {
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

  render() {
    return (
      <div className="form-container">
        <h1 style={{ textAlign: "center" }}>Direct messages</h1>
        {/* <form
          id="OpenDirectMessageForm"
          method="post"
          onSubmit={this.onOpenDirectMessageFormSubmit}
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

export default OpenDirectMessageForm;
