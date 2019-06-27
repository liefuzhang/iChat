import React from "react";
import "./AccountCommon.css";
import ApiService from "services/ApiService";
import AuthService from "services/AuthService";
import { toast } from "react-toastify";

class ResetPassword extends React.Component {
  constructor(props) {
    super(props);

    this.apiService = new ApiService(props);
    this.authService = new AuthService(props);
    this.onResetPasswordFormSubmit = this.onResetPasswordFormSubmit.bind(this);

    let searchParams = new URLSearchParams(props.location.search);
    this.email = searchParams.get("email");
    this.code = searchParams.get("code");
    this.onPasswordChange = this.onPasswordChange.bind(this);
  }

  onResetPasswordFormSubmit(event) {
    event.preventDefault();

    let password = event.target.elements["password"].value;

    if (this.email && password && this.code) {
      let button = event.currentTarget.querySelector("button[type='submit']");
      button.classList.add("disabled-button");

      this.apiService
        .fetch(
          `/api/users/resetPassword`,
          {
            method: "POST",
            body: JSON.stringify({
              email: this.email,
              password: password,
              code: this.code
            })
          },
          false,
          true // no auth
        )
        .then(() => {
          this.authService.login(this.email, password);
        })
        .catch(error => {
          toast.error(`Reset password failed: ${error}`);
          button.classList.remove("disabled-button");
        });
    }
  }

  onPasswordChange() {
    let password = document.querySelector(
      "#resetPassword input[name='password']"
    );
    let confirmPassword = document.querySelector(
      "#resetPassword input[name='confirmPassword']"
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
        <div className="login-container panel">
          <div className="form-container">
            <h1>Reset password</h1>
            <form
              id="resetPassword"
              method="post"
              onSubmit={this.onResetPasswordFormSubmit}
            >
              <p className="form-description">Enter your new password.</p>
              <input
                className="form-control"
                name="password"
                type="password"
                placeholder="Password"
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
              <button type="submit" className="btn form-control">
                Reset &amp; Login
              </button>
            </form>
          </div>
        </div>
      </div>
    );
  }
}

export default ResetPassword;
