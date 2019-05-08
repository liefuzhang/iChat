import decode from "jwt-decode";

class AuthService {
  constructor(props) {
    this.localStorageKey = "ichat.user";
    this.props = props;
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
          throw Error("Incorrect email or password");
        }
        return response;
      })
      .then(response => response.json())
      .then(response => {
        localStorage.setItem("ichat.user", JSON.stringify(response));
        this.props.history.push("/");
      })
      .catch(error => alert(error));
  }

  setToken() {}

  checkIfTokenExpired(){}
  
  logout(){}

  fetch(){}
}

export default AuthService;