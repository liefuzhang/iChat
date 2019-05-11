import React from "react";
import "./Login.css";
import AuthService from "../services/AuthService";
import CloseButton from "../components/CloseButton";

class Login extends React.Component {
  constructor(props) {
    super(props);

    this.authService = new AuthService(props);
    this.showCreateWorkSpace = this.showCreateWorkSpace.bind(this);
  }

  componentDidMount() {
    document.querySelector("#account").addEventListener("submit", event => {
      event.preventDefault();
      let email = event.target.elements["email"].value;
      let password = event.target.elements["password"].value;
      if (email && password) {
        this.authService.login(email, password);
      }
    });
  }

  showCreateWorkSpace() {
    document.querySelector("#loginContainer").style.display = "none";
    document.querySelector("#workspaceContainer").classList.remove("hide-container");
  }

  hideCreateWorkspace() {
    document.querySelector("#loginContainer").style.display = "flex";
    document.querySelector("#workspaceContainer").classList.add("hide-container");
  }

  render() {
    return (
      <div className="login-page">
        <div id="loginContainer" className="login-container panel">
          <section>
            <h1 style={{ textAlign: "center" }}>Login to your workspace</h1>
            <div className="login-form">
              <form id="account" method="post">
                <p>Enter email address and password to log in.</p>
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
          <div className="horizontal-divider">or</div>
          <section>
            <button className="btn white-btn" onClick={this.showCreateWorkSpace}>
              Create New WorkSpace
            </button>
          </section>
        </div>
        <div id="workspaceContainer" className="login-container panel hide-container">
          <CloseButton onClose={this.hideCreateWorkspace}/>
          <section>
            <h1 style={{ textAlign: "center" }}>Create a new workspace</h1>
            <div className="login-form">
              <form id="workspace" method="post">
                <p>Enter details to create workspace.</p>
                <input
                  className="form-control"
                  name="workspace"
                  type="text"
                  placeholder="workspace name"
                  required
                />
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
                  required
                />
                <button type="submit" className="btn form-control">
                  Create
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
