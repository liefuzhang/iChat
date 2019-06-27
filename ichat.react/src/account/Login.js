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
    this.onAccountFormSubmit = this.onAccountFormSubmit.bind(this);
    this.onWorkspaceFormSubmit = this.onWorkspaceFormSubmit.bind(this);
    this.onForgotPasswordFormSubmit = this.onForgotPasswordFormSubmit.bind(
      this
    );
    this.onOwnerAccountFormSubmit = this.onOwnerAccountFormSubmit.bind(this);
    this.onPasswordChange = this.onPasswordChange.bind(this);

    this.createdWorkspaceName = "";
    this.createdWorkspaceOwnerEmail = "";
    this.createdWorkspaceOwnerPassword = "";
    this.resetEmail = "";

    this.state = {
      isLoginFormVisible: true,
      isForgotPasswordFormVisible: false,
      isForgotPasswordConfirmationFormVisible: false,
      isWorkspaceFormVisible: false,
      isOwnerLoginFormVisible: false
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

  onForgotPasswordFormSubmit(event) {
    event.preventDefault();
    let email = event.target.elements["email"].value;

    if (email) {
      this.apiService
        .fetch(
          `/api/users/forgotPassword`,
          {
            method: "POST",
            body: JSON.stringify(email)
          },
          false,
          true // no auth
        )
        .then(() => {
          this.resetEmail = email;
          this.setState({
            isForgotPasswordFormVisible: false,
            isForgotPasswordConfirmationFormVisible: true
          });
        })
        .catch(error => {
          toast.error(`Send reset link failed: ${error}`);
        });
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
          this.createdWorkspaceName = workspace;
          this.createdWorkspaceOwnerEmail = email;
          this.createdWorkspaceOwnerPassword = password;
          this.setState({
            isWorkspaceFormVisible: false,
            isOwnerLoginFormVisible: true
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
      this.createdWorkspaceOwnerEmail,
      this.createdWorkspaceOwnerPassword
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

  render() {
    return (
      <div className="login-page">
        {this.state.isLoginFormVisible && (
          <div id="loginContainer" className="login-container panel">
            <section>
              <div className="form-container">
                <h1>Login to your workspace</h1>
                <form method="post" onSubmit={this.onAccountFormSubmit}>
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
                  <div className="forgot-password">
                    <a
                      onClick={() =>
                        this.setState({
                          isLoginFormVisible: false,
                          isForgotPasswordFormVisible: true
                        })
                      }
                    >
                      Forgot your password?
                    </a>
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
                onClick={() =>
                  this.setState({
                    isLoginFormVisible: false,
                    isWorkspaceFormVisible: true
                  })
                }
              >
                Create New WorkSpace
              </button>
            </section>
          </div>
        )}
        {this.state.isForgotPasswordFormVisible && (
          <div className="login-container panel">
            <CloseButton
              onClose={() =>
                this.setState({
                  isLoginFormVisible: true,
                  isForgotPasswordFormVisible: false
                })
              }
            />
            <section>
              <div className="form-container">
                <h1>Reset password</h1>
                <form method="post" onSubmit={this.onForgotPasswordFormSubmit}>
                  <p className="form-description">
                    Enter your registered email address to reset password.
                  </p>
                  <input
                    className="form-control"
                    name="email"
                    type="email"
                    placeholder="Email"
                    required
                  />
                  <button type="submit" className="btn form-control">
                    Send reset link
                  </button>
                </form>
              </div>
            </section>
          </div>
        )}
        {this.state.isForgotPasswordConfirmationFormVisible && (
          <div className="login-container panel">
            <section>
              <div className="form-container">
                <h1>Email sent!</h1>
                <form>
                  <p className="form-description">
                    Check your <b>{this.resetEmail}</b> inbox to reset your
                    password.
                  </p>
                </form>
              </div>
            </section>
          </div>
        )}
        {this.state.isWorkspaceFormVisible && (
          <div className="login-container panel">
            <CloseButton
              onClose={() =>
                this.setState({
                  isLoginFormVisible: true,
                  isWorkspaceFormVisible: false
                })
              }
            />
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
        )}
        {this.state.isOwnerLoginFormVisible && (
          <div className="login-container panel">
            <section>
              <div className="form-container">
                <h1>
                  Workspace <b>{this.createdWorkspaceName}</b> Created!
                </h1>
                <form
                  method="post"
                  onSubmit={this.onOwnerAccountFormSubmit}
                >
                  <p className="form-description">
                    Click <b>Continue</b> to login as{" "}
                    <b>{this.createdWorkspaceOwnerEmail}</b>.
                  </p>
                  <button type="submit" className="btn form-control">
                    Continue
                  </button>
                </form>
              </div>
            </section>
          </div>
        )}
      </div>
    );
  }
}

export default Login;
