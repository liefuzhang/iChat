import React from "react";
import "./Login.css";
import "./AccountCommon.css";
import AuthService from "services/AuthService";
import CloseButton from "components/CloseButton";
import ApiService from "services/ApiService";
import { toast } from "react-toastify";

class Login extends React.Component {
  constructor(props) {
    super(props);

    this.authService = new AuthService(props);
    this.apiService = new ApiService(props);
    this.showCreateWorkSpace = this.showCreateWorkSpace.bind(this);
    this.onAccountFormSubmit = this.onAccountFormSubmit.bind(this);
    this.onWorkspaceFormSubmit = this.onWorkspaceFormSubmit.bind(this);
    this.onOwnerAccountFormSubmit = this.onOwnerAccountFormSubmit.bind(this);
    this.onPasswordChange = this.onPasswordChange.bind(this);

    this.state = {
      createdWorkspaceName: "",
      createdWorkspaceOwnerEmail: "",
      createdWorkspaceOwnerPassword: ""
    };
  }

  onAccountFormSubmit(event) {
    event.preventDefault();
    let email = event.target.elements["email"].value;
    let password = event.target.elements["password"].value;
    if (email && password) {
      this.authService.login(email, password);
    }
  }

  onWorkspaceFormSubmit(event) {
    event.preventDefault();
    let email = event.target.elements["email"].value;
    let password = event.target.elements["password"].value;
    let workspace = event.target.elements["workspace"].value;
    let displayName = event.target.elements["displayName"].value;

    if (email && password && workspace) {
      this.apiService
        .fetch(
          `/api/workspaces/register`,
          {
            method: "POST",
            body: JSON.stringify({
              email: email,
              password: password,
              workspaceName: workspace,
              displayName: displayName
            })
          },
          false,
          true // no auth
        )
        .then(() => {
          document
            .querySelector("#workspaceContainer")
            .classList.add("hide-container");
          document
            .querySelector("#ownerLoginContainer")
            .classList.remove("hide-container");

          this.setState({
            createdWorkspaceName: workspace,
            createdWorkspaceOwnerEmail: email,
            createdWorkspaceOwnerPassword: password
          });
        })
        .catch(error => {
          toast.error(`Create workspace failed: ${error}`);
        });
    }
  }

  onOwnerAccountFormSubmit(event) {
    event.preventDefault();

    this.authService.login(
      this.state.createdWorkspaceOwnerEmail,
      this.state.createdWorkspaceOwnerPassword
    );
  }

  onPasswordChange() {
    let password = document.querySelector("#workspace input[name='password']");
    let confirmPassword = document.querySelector(
      "#workspace input[name='confirmPassword']"
    );
    if (password.value !== confirmPassword.value) {
      confirmPassword.setCustomValidity("Passwords don't match");
    } else {
      confirmPassword.setCustomValidity("");
    }
  }

  showCreateWorkSpace() {
    document.querySelector("#loginContainer").style.display = "none";
    document
      .querySelector("#workspaceContainer")
      .classList.remove("hide-container");
  }

  hideCreateWorkspace() {
    document.querySelector("#loginContainer").style.display = "flex";
    document
      .querySelector("#workspaceContainer")
      .classList.add("hide-container");
  }

  render() {
    return (
      <div className="login-page">
        <div id="loginContainer" className="login-container panel">
          <section>
            <div className="form-container">
              <h1>Login to your workspace</h1>
              <form
                id="account"
                method="post"
                onSubmit={this.onAccountFormSubmit}
              >
                <p className="form-description">
                  Enter email address and password to log in.
                </p>
                <input
                  className="form-control"
                  name="email"
                  type="email"
                  placeholder="Email"
                  required
                />
                <input
                  className="form-control"
                  name="password"
                  type="password"
                  placeholder="Password"
                  minLength="6"
                  required
                />
                <button type="submit" className="btn form-control">
                  Log in
                </button>
                <div className="remember-me">
                  <input name="rememberMe" type="checkbox" />
                  <label>Remember me</label>
                </div>
                <div className="forgot-password">
                  <a>Forgot your password?</a>
                </div>
              </form>
            </div>
          </section>
          <div className="horizontal-divider">
            <span>or</span>
          </div>
          <section>
            <button
              className="btn white-btn"
              onClick={this.showCreateWorkSpace}
            >
              Create New WorkSpace
            </button>
          </section>
        </div>
        <div
          id="workspaceContainer"
          className="login-container panel hide-container"
        >
          <CloseButton onClose={this.hideCreateWorkspace} />
          <section>
            <div className="form-container">
              <h1>Create a new workspace</h1>
              <form
                id="workspace"
                method="post"
                onSubmit={this.onWorkspaceFormSubmit}
              >
                <p className="form-description">
                  Enter details to create workspace.
                </p>
                <input
                  className="form-control"
                  name="workspace"
                  type="text"
                  placeholder="Workspace Name"
                  pattern="[0-9A-Za-z_]*"
                  title="Workspace name can only contain numbers, letters and underscore."
                  required
                />
                <input
                  className="form-control"
                  name="email"
                  type="email"
                  placeholder="Your Email"
                  required
                />
                <input
                  className="form-control"
                  name="password"
                  type="password"
                  placeholder="Your Password"
                  minLength="6"
                  onInput={this.onPasswordChange}
                  required
                />
                <input
                  className="form-control"
                  name="confirmPassword"
                  type="password"
                  placeholder="Confirm Password"
                  minLength="6"
                  onInput={this.onPasswordChange}
                  required
                />
                <input
                  className="form-control"
                  name="displayName"
                  type="text"
                  placeholder="Your Display Name"
                />
                <button type="submit" className="btn form-control">
                  Create
                </button>
              </form>
            </div>
          </section>
        </div>
        <div
          id="ownerLoginContainer"
          className="login-container panel hide-container"
        >
          <section>
            <div className="form-container">
              <h1>
                Workspace <b>{this.state.createdWorkspaceName}</b> Created!
              </h1>
              <form
                id="ownerAccount"
                method="post"
                onSubmit={this.onOwnerAccountFormSubmit}
              >
                <p className="form-description">
                  Click <b>Continue</b> to login as{" "}
                  <b>{this.state.createdWorkspaceOwnerEmail}</b>.
                </p>
                <button type="submit" className="btn form-control">
                  Continue
                </button>
              </form>
            </div>
          </section>
        </div>
      </div>
    );
  }
}

export default Login;
