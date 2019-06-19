import React from "react";
import "./InvitePeopleForm.css";
import AuthService from "services/AuthService";
import { toast } from "react-toastify";
import { Icon, Popup } from "semantic-ui-react";

class InvitePeopleForm extends React.Component {
  constructor(props) {
    super(props);

    this.addAnotherEmail = this.addAnotherEmail.bind(this);
    this.removeEmail = this.removeEmail.bind(this);
    this.onEmailFormSubmit = this.onEmailFormSubmit.bind(this);
    this.authService = new AuthService(props);

    this.state = {
      emailsToInvite: ["", "", ""]
    };
  }

  addAnotherEmail() {
    let emails = this.state.emailsToInvite;
    emails.push("");
    this.setState({
      emailsToInvite: emails
    });
  }

  removeEmail(index) {
    let emails = this.state.emailsToInvite;
    emails.splice(index, 1);
    this.setState({
      emailsToInvite: emails
    });
  }

  onEmailFormSubmit(event) {
    event.preventDefault();
    let emails = this.state.emailsToInvite.filter(e => e && !!e.trim());
    if (emails.length === 0) return;

    event.currentTarget
      .querySelector("button[type='submit']")
      .classList.add("disabled-button");

    this.authService
      .fetch(`/api/users/invite`, {
        method: "POST",
        body: JSON.stringify(emails)
      })
      .then(() => {
        toast.success("Invitation email sent!");
        this.props.onClose();
      })
      .catch(error => {
        toast.error(`Send Invitation failed: ${error}`);
      });
  }

  handleChange(event, index) {
    let emails = this.state.emailsToInvite;
    emails[index] = event.target.value;
    this.setState({
      emailsToInvite: emails
    });
  }

  render() {
    return (
      <div className="form-container">
        <h1 style={{ textAlign: "center" }}>
          Invite People to {this.props.userProfile.workspaceName}
        </h1>
        <form id="emailForm" method="post" onSubmit={this.onEmailFormSubmit}>
          <p className="form-description">Enter email addresses to invite.</p>
          {this.state.emailsToInvite.map((e, index) => {
            return (
              <div key={index} className="email-row">
                <input
                  className="form-control"
                  type="email"
                  placeholder="Email"
                  value={e}
                  onChange={event => this.handleChange(event, index)}
                />
                {this.state.emailsToInvite.length > 1 && (
                  <Popup
                    trigger={
                      <Icon
                        name="times"
                        className="icon-times"
                        onClick={() => this.removeEmail(index)}
                      />
                    }
                    content="Remove this email"
                    inverted
                    position="right center"
                    size="tiny"
                  />
                )}
              </div>
            );
          })}
          <div onClick={this.addAnotherEmail} className="add-email">
            <Icon name="plus circle" className="icon-circle" />
            <span>Add another</span>
          </div>
          <button type="submit" className="btn form-control">
            Send Invitation
          </button>
        </form>
      </div>
    );
  }
}

export default InvitePeopleForm;
