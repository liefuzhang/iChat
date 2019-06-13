import React from "react";
import AuthService from "services/AuthService";
import { toast } from "react-toastify";

class UploadFileForm extends React.Component {
  constructor(props) {
    super(props);

    this.onUploadFileFormSubmit = this.onUploadFileFormSubmit.bind(this);
    this.authService = new AuthService(props);
  }

  onUploadFileFormSubmit(event) {
    event.preventDefault();

    let formData = new FormData();
    let file = this.props.file;
    formData.append("files", file);

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
          <div className="file-container">{this.props.file.name}</div>
          <button type="submit" className="btn form-control">
            Upload
          </button>
        </form>
      </div>
    );
  }
}

export default UploadFileForm;
