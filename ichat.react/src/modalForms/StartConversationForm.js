import React from "react";
import "./StartConversationForm.css";
import AuthService from "services/AuthService";
import { Dropdown } from "semantic-ui-react";

class StartConversationForm extends React.Component {
  constructor(props) {
    super(props);

    this.onStartConversationFormSubmit = this.onStartConversationFormSubmit.bind(
      this
    );
    this.changeConversationUser = this.changeConversationUser.bind(this);
    this.authService = new AuthService(props);

    this.state = {
      userList: undefined
    };

    this.conversationUserIds = [];
  }

  onStartConversationFormSubmit(event) {
    event.preventDefault();
    if (!this.conversationUserIds || this.conversationUserIds.length === 0)
      return;

    this.authService
      .fetch(`/api/conversations/start`, {
        method: "POST",
        body: JSON.stringify(this.conversationUserIds)
      })
      .then(id => {
        this.props.onConversationStarted();
        this.props.history.push(`/conversation/${id}`);
      });
  }

  changeConversationUser(event, item) {
    this.conversationUserIds = item.value;
  }

  componentDidMount() {
    this.authService.fetch("/api/users").then(users => {
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

          {this.state.userList && (
            <Dropdown
              placeholder="Search user"
              fluid
              multiple
              search
              selection
              options={this.state.userList}
              onChange={this.changeConversationUser}
            />
          )}
          <button type="submit" className="btn form-control">
            Go
          </button>
        </form>
      </div>
    );
  }
}

export default StartConversationForm;
