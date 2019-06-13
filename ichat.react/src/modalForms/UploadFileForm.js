import React from "react";
import AuthService from "services/AuthService";
import { toast } from "react-toastify";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Icon } from "semantic-ui-react";

class UploadFileForm extends React.Component {
  constructor(props) {
    super(props);

    this.onUploadFileFormSubmit = this.onUploadFileFormSubmit.bind(this);
    this.authService = new AuthService(props);
  }

  onUploadFileFormSubmit(event) {
    event.preventDefault();

    let formData = new FormData();
    let files = this.props.files;
    if (files.length === 0) return;
    files.forEach(file => formData.append("files", file));

    let url = this.props.isChannel
      ? `/api/messages/channel/${this.props.id}/uploadFile`
      : `/api/messages/conversation/${this.props.id}/uploadFile`;
    this.authService.fetch(
      url,
      {
        method: "POST",
        body: formData
      },
      true //noContentType
    );
    // .then(id => {
    //   this.props.onChannelCreated();
    //   this.props.history.push(`/channel/${id}`);
    // })
    // .catch(error => {
    //   toast.error(`Create channel failed: ${error}`);
  }
  // });

  render() {
    return (
      <div className="form-container">
        <h1 style={{ textAlign: "center" }}>Upload file</h1>
        <form
          id="uploadFileForm"
          method="post"
          onSubmit={this.onUploadFileFormSubmit}
        >
          <p className="form-description warning-text">
            Your workspace is limited to 1 GB of files. If you need to upload
            more files, please contact liefuzhangnz@gmail.com
          </p>
          <div className="file-container">
            {this.props.files.map(file => (
              <div key={file.name + file.lastModified} className="file-item">
                <Icon name="file outline" size="large" />
                <span>{file.name}</span>
              </div>
            ))}
          </div>
          <button type="submit" className="btn form-control">
            Upload
          </button>
        </form>
      </div>
    );
  }
}

export default UploadFileForm;
