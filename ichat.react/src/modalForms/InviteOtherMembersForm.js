import React from "react";
import { Dropdown } from "semantic-ui-react";
import { toast } from "react-toastify";
import ApiService from "services/ApiService";

class InviteOtherMembersForm extends React.Component {
  constructor(props) {
    super(props);

    this.onInviteOtherMembersFormSubmit = this.onInviteOtherMembersFormSubmit.bind(
      this
    );
    this.changeInviteMembers = this.changeInviteMembers.bind(this);
    this.apiService = new ApiService(props);

    this.state = {
      userList: []
    };

    this.inviteMembersUserIds = [];
  }

  onInviteOtherMembersFormSubmit(event) {
    event.preventDefault();
    if (!this.inviteMembersUserIds || this.inviteMembersUserIds.length === 0)
      return;

    let button = event.currentTarget.querySelector("button[type='submit']");
    button.classList.add("disabled-button");

    let url = `/api/${this.props.isChannel ? "channels" : "conversations"}/${
      this.props.id
    }/inviteOtherMembers`;

    this.apiService
      .fetch(url, {
        method: "POST",
        body: JSON.stringify(this.inviteMembersUserIds)
      })
      .then(id => {
        this.props.onClose();
        if (!this.props.isChannel && id !== this.props.id) {
          // when conversation already exists, redirect to it
          this.props.history.push(`/conversation/${id}`);
        }
      })
      .catch(error => {
        toast.error(`Invite other members failed: ${error}`);
        button.classList.remove("disabled-button");
      });
  }

  changeInviteMembers(event, item) {
    this.inviteMembersUserIds = item.value;
  }

  componentDidMount() {
    let allUsersPromise = this.apiService.fetch("/api/users");
    let memberIdsPromise = this.apiService.fetch(
      `/api/${this.props.isChannel ? "channels" : "conversations"}/${
        this.props.id
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
      <div className="form-container form-with-dropdown">
        <h1 style={{ textAlign: "center" }}>Invite other members</h1>

        <form
          id="InviteOtherMembersForm"
          method="post"
          onSubmit={this.onInviteOtherMembersFormSubmit}
        >
          <p className="form-description">
            Select people you want to invite to this
            {this.props.isChannel ? " channel" : " conversation"}.
          </p>

          <Dropdown
            placeholder="Search user"
            fluid
            multiple
            search
            selection
            options={this.state.userList}
            onChange={this.changeInviteMembers}
          />
          <button type="submit" className="btn form-control">
            Go
          </button>
        </form>
      </div>
    );
  }
}

export default InviteOtherMembersForm;
