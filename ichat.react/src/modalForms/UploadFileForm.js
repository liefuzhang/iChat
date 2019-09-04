import React from "react";
import { toast } from "react-toastify";
import { Icon, Progress } from "semantic-ui-react";
import "./UploadFileForm.css";
import ApiService from "services/ApiService";

class UploadFileForm extends React.Component {
  constructor(props) {
    super(props);

    this.onUploadFileFormSubmit = this.onUploadFileFormSubmit.bind(this);
    this.apiService = new ApiService(props);
    let maxFileCount = 3;
    this.files = this.props.files;
    if (this.files.length > maxFileCount) {
      toast.warn(`Maximum ${maxFileCount} files can be uploaded at one time`);
      this.files = this.files.slice(0, maxFileCount);
    }

    this.state = {
      showUploadingBar: false
    };
  }

  onUploadFileFormSubmit(event) {
    event.preventDefault();
    if (this.files.length === 0) return;

     let button = event.currentTarget.querySelector("button[type='submit']");
    button.classList.add("disabled-button");

    this.setState({
      showUploadingBar: true
    });

    let formData = new FormData();
    this.files.forEach(file => formData.append("files", file));

    let url = this.props.isChannel
      ? `/api/messages/channel/${this.props.id}/uploadFile`
      : `/api/messages/conversation/${this.props.id}/uploadFile`;
    this.apiService
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
        this.setState({
          showUploadingBar: false
        });
        button.classList.remove("disabled-button");
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
            There is some limitation on how many files you can upload. If you need to upload
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
          {this.state.showUploadingBar && (
            <Progress percent={100} active>
              Uploading file
            </Progress>
          )}
        </form>
      </div>
    );
  }
}

export default UploadFileForm;
