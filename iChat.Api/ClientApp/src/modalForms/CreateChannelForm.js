import React from "react";
import { toast } from "react-toastify";
import ApiService from "services/ApiService";

class CreateChannelForm extends React.Component {
  constructor(props) {
    super(props);

    this.onCreateChannelFormSubmit = this.onCreateChannelFormSubmit.bind(this);
    this.apiService = new ApiService(props);
  }

  onCreateChannelFormSubmit(event) {
    event.preventDefault();

    let button = event.currentTarget.querySelector("button[type='submit']");
    button.classList.add("disabled-button");

    let name = event.target.elements["name"].value;
    let topic = event.target.elements["topic"].value;

    this.apiService
      .fetch(`/api/channels`, {
        method: "POST",
        body: JSON.stringify({
          name: name,
          topic: topic
        })
      })
      .then(id => {
        this.props.onChannelCreated();
        this.props.history.push(`/channel/${id}`);
      })
      .catch(error => {
        toast.error(`Create channel failed: ${error}`);
        button.classList.remove("disabled-button");
      });
  }

  render() {
    return (
      <div className="form-container">
        <h1 style={{ textAlign: "center" }}>Create a channel</h1>
        <form
          id="createChannelForm"
          method="post"
          onSubmit={this.onCreateChannelFormSubmit}
        >
          <p className="form-description">
            Channels are where specific topics can be talked about.
          </p>
          <div className="form-group">
            <label htmlFor="createChannelName">Name</label>
            <input
              className="form-control"
              type="text"
              id="createChannelName"
              name="name"
              placeholder="e.g. meetings"
              required
            />
          </div>
          <div className="form-group">
            <label htmlFor="createChannelTopic">
              Topic<span className="secondary-text">&nbsp;(optional)</span>
            </label>
            <input
              className="form-control"
              type="text"
              id="createChannelTopic"
              name="topic"
              placeholder="What is this channel about?"
            />
          </div>
          <button type="submit" className="btn form-control">
            Create Channel
          </button>
        </form>
      </div>
    );
  }
}

export default CreateChannelForm;
