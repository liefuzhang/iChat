import decode from "jwt-decode";

class AuthService {
  constructor(props) {
    this.localStorageKey = "ichat.user";
    this.props = props;
    this.fetch = this.fetch.bind(this);
  }

  login(email, password) {
    fetch(`/api/identity/authenticate`, {
      method: "POST",
      body: JSON.stringify({ email: email, password: password }),
      headers: {
        "Content-Type": "application/json"
      }
    })
      .then(function(response) {
        if (!response.ok) {
          return response.text().then(r => Promise.reject(r));
        }
        return response.text();
      })
      .then(text => {
        this.setProfile(text);
        this.props.history.push("/");
      })
      .catch(error => alert(error));
  }

  isLoggedIn() {
    return !!this.getProfile();
  }

  setProfile(profile) {
    localStorage.setItem(this.localStorageKey, profile);
  }

  getProfile() {
    if (!localStorage.getItem(this.localStorageKey)) return null;
    return JSON.parse(localStorage.getItem(this.localStorageKey));
  }

  getToken() {
    let profile = this.getProfile();
    return profile && profile.token;
  }

  checkIfTokenExpired() {}

  logout() {
    localStorage.removeItem(this.localStorageKey);
    this.props.history.push("/login");
  }

  fetch(url, options) {
    if (!url) {
      return Promise.reject(new Error("Empty url"));
    }

    var token = this.getToken();
    if (!token || this.checkIfTokenExpired(token)) {
      this.props.history.push("/login");
      return Promise.reject(new Error("invalid token, taken to login")).then(
        () => this.props.history.push("/login")
      );
    }

    var bearer = "Bearer " + token;
    options || (options = {});
    options.headers = {
      "Content-Type": "application/json",
      Authorization: bearer
    };

    return fetch(url, options)
      .then(function(response) {
        if (!response.ok) {
          return response.text().then(r => Promise.reject(r));
        }
        return response.text();
      })
      .then(text => (text.length ? JSON.parse(text) : {}))
      .then(json => Promise.resolve(json))
      .catch(error => alert(error));
  }
}

export default AuthService;
