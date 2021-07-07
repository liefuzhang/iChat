import React from "react";
import { Dropdown } from "semantic-ui-react";
import { toast } from "react-toastify";
import ApiService from "services/ApiService";

class JoinChannelForm extends React.Component {
  constructor(props) {
    super(props);

    this.onJoinChannelFormSubmit = this.onJoinChannelFormSubmit.bind(this);
    this.changeSelectedChannel = this.changeSelectedChannel.bind(this);
    this.apiService = new ApiService(props);

    this.state = {
      channelList: []
    };

    this.selectedChannelId = undefined;
  }

  onJoinChannelFormSubmit(event) {
    event.preventDefault();
    if (!this.selectedChannelId) return;

    let button = event.currentTarget.querySelector("button[type='submit']");
    button.classList.add("disabled-button");

    this.apiService
      .fetch(`/api/channels/join`, {
        method: "POST",
        body: JSON.stringify(this.selectedChannelId)
      })
      .then(() => {
        this.props.onChannelJoined();
        this.props.history.push(`/channel/${this.selectedChannelId}`);
      })
      .catch(error => {
        toast.error(`Join channel failed: ${error}`);
        button.classList.remove("disabled-button");
      });
  }

  changeSelectedChannel(event, item) {
    this.selectedChannelId = item.value;
  }

  componentDidMount() {
    this.apiService.fetch("/api/channels/allUnsubscribed").then(channels => {
      let channelList = channels.map(c => {
        return { key: c.id, text: c.name, value: c.id };
      });
      this.setState({ channelList: channelList });
    });
  }

  render() {
    return (
      <div className="form-container form-with-dropdown">
        <h1 style={{ textAlign: "center" }}>Join a channel</h1>

        <form
          id="JoinChannelForm"
          method="post"
          onSubmit={this.onJoinChannelFormSubmit}
        >
          <p className="form-description">Select a channel to join.</p>

          <Dropdown
            placeholder="Search channel"
            fluid
            search
            selection
            options={this.state.channelList}
            onChange={this.changeSelectedChannel}
          />
          <button type="submit" className="btn form-control">
            Join
          </button>
        </form>
      </div>
    );
  }
}

export default JoinChannelForm;
