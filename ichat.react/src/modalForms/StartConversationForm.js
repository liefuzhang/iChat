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
