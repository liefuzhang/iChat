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
      .fetch(`/api/conversation/start`, {
        method: "POST",
        body: JSON.stringify({
          userIds: this.conversationUserIds
        })
      })
      .then(id => {
        this.props.onConversationStarted();
        // this.props.history.push(`/channel/${id}`);
      });
  }

  changeConversationUser(event, item) {
    this.conversationUserIds = item.value;
  }

  componentDidMount() {
    this.authService.fetch("/api/users").then(users => {
      let userList = users.map(u => {
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
