import React from "react";
import "./AccountCommon.css";
import AuthService from "../services/AuthService";
import { toast } from "react-toastify";

class AcceptInvitation extends React.Component {
  constructor(props) {
    super(props);

    this.authService = new AuthService(props);
    this.onAcceptInvitationFormSubmit = this.onAcceptInvitationFormSubmit.bind(
      this
    );

    let searchParams = new URLSearchParams(props.location.search);
    this.workspaceName = searchParams.get("workspaceName");
    this.email = searchParams.get("email");
    this.code = searchParams.get("code");
  }

  onAcceptInvitationFormSubmit(event) {
    event.preventDefault();
    let name = event.target.elements["name"].value;
    let password = event.target.elements["password"].value;
    if (this.email && password && this.code) {
      this.apiService
        .fetch(
          `/api/users/acceptInvitation`,
          {
            method: "POST",
            body: JSON.stringify({
              email: this.email,
              password: password,
              name: name,
              code: this.code
            })
          },
          false,
          true // no auth
        )
        .then(() => {
          this.authService.login(this.email, this.password);
        })
        .catch(error => {
          toast.error(`Create account failed: ${error}`);
          return Promise.reject();
        });
    }
  }

  render() {
    return (
      <div className="login-page">
        <div className="login-container panel">
          <div className="form-container">
            <h1>Join {this.workspaceName} on iChat</h1>
            <form
              id="acceptInvitation"
              method="post"
              onSubmit={this.onAcceptInvitationFormSubmit}
            >
              <p>Enter your name and password to create account.</p>
              <input
                className="form-control"
                name="name"
                type="text"
                placeholder="Display name"
              />
              <input
                className="form-control"
                name="password"
                type="password"
                placeholder="Password"
                required
              />
              <button type="submit" className="btn form-control">
                Create Account
              </button>
            </form>
          </div>
        </div>
      </div>
    );
  }
}

export default AcceptInvitation;
