import React from "react";
import "./StartConversationForm.css";
import { Dropdown } from "semantic-ui-react";
import { toast } from "react-toastify";
import ApiService from "services/ApiService";

class StartConversationForm extends React.Component {
  constructor(props) {
    super(props);

    this.onStartConversationFormSubmit = this.onStartConversationFormSubmit.bind(
      this
    );
    this.changeConversationUser = this.changeConversationUser.bind(this);
    this.apiService = new ApiService(props);

    this.state = {
      userList: []
    };

    this.conversationUserIds = [];
  }

  onStartConversationFormSubmit(event) {
    event.preventDefault();
    if (!this.conversationUserIds || this.conversationUserIds.length === 0)
      return;

    event.currentTarget
      .querySelector("button[type='submit']")
      .classList.add("disabled-button");

    this.apiService
      .fetch(`/api/conversations/start`, {
        method: "POST",
        body: JSON.stringify(this.conversationUserIds)
      })
      .then(id => {
        this.props.onConversationStarted();
        this.props.history.push(`/conversation/${id}`);
      })
      .catch(error => {
        toast.error(`Start conversation failed: ${error}`);
      });
  }

  changeConversationUser(event, item) {
    this.conversationUserIds = item.value;
  }

  componentDidMount() {
    this.apiService.fetch("/api/users").then(users => {
      let currentUserId = this.props.userProfile.id;
      let userList = users
        .filter(u => u.id !== currentUserId)
        .map(u => {
          return { key: u.id, text: u.displayName, value: u.id };
        });
      this.setState({ userList: userList });
    });
  }

  render() {
    return (
      <div className="form-container start-conversation-form">
        <h1 style={{ textAlign: "center" }}>Start conversation</h1>

        <form
          id="StartConversationForm"
          method="post"
          onSubmit={this.onStartConversationFormSubmit}
        >
          <p className="form-description">
            Select people you want to have a conversation with.
          </p>

          <Dropdown
            placeholder="Search user"
            fluid
            multiple
            search
            selection
            options={this.state.userList}
            onChange={this.changeConversationUser}
          />
          <button type="submit" className="btn form-control">
            Go
          </button>
        </form>
      </div>
    );
  }
}

export default StartConversationForm;
