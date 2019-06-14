import React from "react";
import AuthService from "services/AuthService";
import { toast } from "react-toastify";
import { Icon } from "semantic-ui-react";
import "./UploadFileForm.css";

class UploadFileForm extends React.Component {
  constructor(props) {
    super(props);

    this.onUploadFileFormSubmit = this.onUploadFileFormSubmit.bind(this);
    this.authService = new AuthService(props);
    let maxFileCount = 3;
    this.files = this.props.files;
    if (this.files.length > maxFileCount) {
      toast.warn(`Maximum ${maxFileCount} files can be uploaded at one time`);
      this.files = this.files.slice(0, maxFileCount);
    }
  }

  onUploadFileFormSubmit(event) {
    event.preventDefault();

    let formData = new FormData();
    if (this.files.length === 0) return;
    this.files.forEach(file => formData.append("files", file));

    let url = this.props.isChannel
      ? `/api/messages/channel/${this.props.id}/uploadFile`
      : `/api/messages/conversation/${this.props.id}/uploadFile`;
    this.authService
      .fetch(
        url,
        {
          method: "POST",
          body: formData
        },
        true //noContentType
      )
      .then(id => {
        this.props.onUploaded();
      })
      .catch(error => {
        toast.error(`Upload file failed: ${error}`);
      });
  }

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
            {this.files.map(file => (
              <div key={file.name + file.lastModified} className="file-item">
                <Icon name="file outline" size="large" />
                <span title={file.name}>{file.name}</span>
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
