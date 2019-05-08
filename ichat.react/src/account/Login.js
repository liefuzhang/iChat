import React from "react";
import "./Login.css";

class Login extends React.Component {
  constructor(props) {
    super(props);
  }

  componentDidMount() {
    document.querySelector("#account").addEventListener("submit", event => {
      event.preventDefault();
      let email = event.target.elements["email"].value;
      let password = event.target.elements["password"].value;
      if (email && password) {
        fetch(`/api/identity/authenticate`, {
          method: "POST",
          body: JSON.stringify({ email: email, password: password }),
          headers: {
            "Content-Type": "application/json"
          }
        })
          .then(function(response) {
            if (!response.ok) {
              throw Error("Incorrect email or password");
            }
            return response;
          })
          .then(response => response.json())
          .then(response => {
            localStorage.setItem("ichat.user", JSON.stringify(response));
          })
          .catch(error => alert(error));
      }
    });
  }

  render() {
    return (
      <div className="login-page">
        <div className="login-container panel">
          <h1>Login</h1>
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
        </div>
      </div>
    );
  }
}

export default Login;
