import React from "react";
import "./CreateChannelForm.css";
import AuthService from "services/AuthService";

class CreateChannelForm extends React.Component {
  constructor(props) {
    super(props);

    this.onCreateChannelFormSubmit = this.onCreateChannelFormSubmit.bind(this);
    this.authService = new AuthService(props);
  }

  onCreateChannelFormSubmit(event) {
    event.preventDefault();
    let name = event.target.elements["name"].value;
    let topic = event.target.elements["topic"].value;

    this.authService
      .fetch(`/api/channels`, {
        method: "POST",
        body: JSON.stringify({
          name: name,
          topic: topic
        })
      })
      .then((id) => {
        this.props.onClose();
        this.props.history.push(`/channel/${id}`);
      });
  }

  render() {
    return (
      <div className="form-container">
        <h1 style={{ textAlign: "center" }}>Create a channel</h1>
        <form
          id="createChannelForm"
          method="post"
          onSubmit={this.ononCreateChannelFormSubmit}
        >
          <p>Channels are where specific topics can be talked about.</p>
          <input
            className="form-control"
            type="text"
            name="name"
            placeholder="Channel Name, e.g. meetings"
            required
          />
          <input
            className="form-control"
            type="text"
            name="topic"
            placeholder="What this channel is about?"
          />
          <button type="submit" className="btn form-control">
            Create Channel
          </button>
        </form>
      </div>
    );
  }
}

export default CreateChannelForm;
