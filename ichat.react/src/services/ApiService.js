import ProfileService from "services/ProfileService";

class ApiService {
  constructor(props) {
    this.props = props;
    this.profileService = new ProfileService();
  }

  fetchErrorHandler(error) {
    console.error(error);
    return Promise.reject(error);
  }

  fetch(url, options, noContentType, noAuth) {
    return this.fetchCommon(url, options, noContentType, noAuth)
      .then(response => {
        if (!response.ok) {
          return response.text().then(r => Promise.reject(r));
        }
        if (
          response.headers.get("content-type") &&
          response.headers.get("content-type").includes("application/json")
        ) {
          return response.json();
        } else return response.text();
      })
      .then(result => Promise.resolve(result))
      .catch(this.fetchErrorHandler);
  }

  fetchFile(url) {
    return this.fetchCommon(url)
      .then(response => {
        if (!response.ok) {
          return response.text().then(r => Promise.reject(r));
        }
        return response.blob();
      })
      .then(blob => URL.createObjectURL(blob))
      .catch(this.fetchErrorHandler);
  }

  fetchCommon(url, options, noContentType, noAuth) {
    if (!url) {
      return Promise.reject("Empty url");
    }

    if (!noAuth) {
      var token = this.profileService.getToken();
      if (!token) {
        this.props.history.push("/login");
        return Promise.reject("invalid token, taken to login");
      }
    }

    var bearer = "Bearer " + token;
    options || (options = {});
    options.headers || (options.headers = {});
    noContentType || (options.headers["Content-Type"] = "application/json");
    noAuth || (options.headers["Authorization"] = bearer);

    return fetch(`https://ichat-apis.herokuapp.com${url}`, options);
  }
}

export default ApiService;
