import { toast } from "react-toastify";
import ApiService from "services/ApiService";
import ProfileService from "services/ProfileService";

class AuthService {
  constructor(props) {
    this.localStorageKey = "ichat.user";
    this.props = props;
    this.apiService = new ApiService(props);
    this.profileService = new ProfileService();
  }

  login(email, password) {
    this.apiService
      .fetch(
        `/api/identity/authenticate`,
        {
          method: "POST",
          body: JSON.stringify({ email: email, password: password })
        },
        false,
        true // no auth
      )
      .then(profile => {
        this.profileService.setProfile(profile);
        this.props.history.push("/");
      })
      .catch(error => {
        toast.error(`Login failed: ${error}`);
      });
  }

  isLoggedIn() {
    return !!this.profileService.getProfile();
  }

  logout() {
    localStorage.removeItem(this.localStorageKey);
    this.props.history.push("/login");
  }
}

export default AuthService;
