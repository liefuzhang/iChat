class ProfileService {
  constructor() {
    this.localStorageKey = "ichat.user";
  }

  setProfile(profile) {
    localStorage.setItem(this.localStorageKey, JSON.stringify(profile));
  }

  getProfile() {
    if (!localStorage.getItem(this.localStorageKey)) return null;
    return JSON.parse(localStorage.getItem(this.localStorageKey));
  }

  getToken() {
    let profile = this.getProfile();
    return profile && profile.token;
  }
}

export default ProfileService;
