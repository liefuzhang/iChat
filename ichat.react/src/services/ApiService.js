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
        return response.text();
      })
      .then(text => (text.length ? JSON.parse(text) : null))
      .then(json => Promise.resolve(json))
      .catch(this.fetchErrorHandler);
  }

  fetchFile(url, fileName) {
    return this.fetchCommon(url)
      .then(response => {
        if (!response.ok) {
          return response.text().then(r => Promise.reject(r));
        }
        return response.blob();
      })
      .then(blob => URL.createObjectURL(blob))
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

    return fetch(url, options);
  }
}

export default ApiService;
