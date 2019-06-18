import { toast } from "react-toastify";

class AuthService {
  constructor(props) {
    this.localStorageKey = "ichat.user";
    this.props = props;
    this.fetch = this.fetch.bind(this);
    this.fetchFile = this.fetchFile.bind(this);
  }

  acceptInvitation(email, password, name, code) {
    fetch(`/api/users/acceptInvitation`, {
      method: "POST",
      body: JSON.stringify({
        email: email,
        password: password,
        name: name,
        code: code
      }),
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
        this.login(email, password);
      })
      .catch(error => {
        toast.error(`Create account failed: ${error}`);
        return Promise.reject();
      });
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
      .catch(error => {
        toast.error(`Login failed: ${error}`);
        return Promise.reject();
      });
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

  fetchErrorHandler(error) {
    console.error(error);
    return Promise.reject(error);
  }

  fetch(url, options, noContentType) {
    options || (options = {});
    this.fetchCommon(url, options, noContentType).catch(this.fetchErrorHandler);

    return fetch(url, options)
      .then(function(response) {
        if (!response.ok) {
          return response.text().then(r => Promise.reject(r));
        }
        return response.text();
      })
      .then(text => (text.length ? JSON.parse(text) : null))
      .then(json => Promise.resolve(json))
      .catch(this.fetchErrorHandler);
  }

  fetchFile(url, fileName) {
    let options = {};
    this.fetchCommon(url, options).catch(this.fetchErrorHandler);

    return fetch(url, options)
      .then(response => {
        return response.blob();
      })
      .then(blob => {
        return URL.createObjectURL(blob);
      })
      .then(url => {
        var link = document.createElement("a");
        link.setAttribute("href", url);
        link.setAttribute("download", fileName);
        link.style.display = "none";
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
      })
      .catch(this.fetchErrorHandler);
  }

  fetchCommon(url, options, noContentType) {
    if (!url) {
      return Promise.reject(new Error("Empty url"));
    }

    var token = this.getToken();
    if (!token || this.checkIfTokenExpired(token)) {
      this.props.history.push("/login");
      return Promise.reject(new Error("invalid token, taken to login")).catch(
        () => this.props.history.push("/login")
      );
    }

    var bearer = "Bearer " + token;
    options.headers || (options.headers = {});
    noContentType || (options.headers["Content-Type"] = "application/json");
    options.headers["Authorization"] = bearer;

    return Promise.resolve();
  }
}

export default AuthService;
