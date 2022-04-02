import React from "react";
import { Dropdown } from "semantic-ui-react";
import { toast } from "react-toastify";
import ApiService from "services/ApiService";

class CreateChannelForm extends React.Component {
  constructor(props) {
    super(props);
    this.onCreateChannelFormSubmit = this.onCreateChannelFormSubmit.bind(this);
    this.changeInviteMembers = this.changeInviteMembers.bind(this);
    this.apiService = new ApiService(props);
    this.state = {
      userList: []
    };
    this.inviteMembersUserIds = [];
  }

  onCreateChannelFormSubmit(event) {
    event.preventDefault();
    if (!this.inviteMembersUserIds || this.inviteMembersUserIds.length === 0)
      return;

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
        let url = `/api/channels/${id}/inviteOtherMembers`;
        this.props.history.push(`/channel/${id}`);
        this.apiService
          .fetch(url, {
            method: "POST",
            body: JSON.stringify(this.inviteMembersUserIds)
          })
          .catch(error => {
            toast.error(`Invite other members failed: ${error}`);
            button.classList.remove("disabled-button");
          });
      })
      .catch(error => {
        toast.error(`Create channel failed: ${error}`);
        button.classList.remove("disabled-button");
      });

  }


  changeInviteMembers(event, item) {
    this.inviteMembersUserIds = item.value;
  }

  componentDidMount() {
    let allUsersPromise = this.apiService.fetch("/api/users");
    let memberIdsPromise = this.apiService.fetch(
      `/api/${this.props.isChannel ? "channels" : "conversations"}/${this.props.id
      }/userIds`
    );

    Promise.all([allUsersPromise, memberIdsPromise]).then(
      ([users, memberIds]) => {
        let userList = users
          .filter(u => memberIds.every(id => id !== u.id))
          .map(u => {
            return { key: u.id, text: u.displayName, value: u.id };
          });
        this.setState({ userList: userList });
      }
    );
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
          <div className="form-group">
            <label htmlFor="createChannelTopic">
              Topic<span className="secondary-text">&nbsp;Select people you want to invite to this</span>
            </label>
            <Dropdown
              placeholder="Search user"
              fluid
              multiple
              search
              selection
              options={this.state.userList}
              onChange={this.changeInviteMembers}
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
