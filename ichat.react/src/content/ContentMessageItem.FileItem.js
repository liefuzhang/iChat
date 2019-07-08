import React from "react";
import "./ContentMessageItem.FileItem.css";
import ContentMessageItemFileItemToolbar from "./ContentMessageItem.FileItemToolbar";
import { Icon } from "semantic-ui-react";
import ApiService from "services/ApiService";
import { toast } from "react-toastify";
import BlankModal from "modals/BlankModal";
import CloseButton from "components/CloseButton";

class ContentMessageItemFileItem extends React.Component {
  constructor(props) {
    super(props);

    this.onShareFileClick = this.onShareFileClick.bind(this);
    this.onCloseShareFile = this.onCloseShareFile.bind(this);
    this.onImageFileClick = this.onImageFileClick.bind(this);
    this.onImageFileFullScreenClose = this.onImageFileFullScreenClose.bind(
      this
    );
    this.apiService = new ApiService(props);
    this.isImage = props.file.contentType.startsWith("image");

    this.state = {
      isShareFileModalOpen: false,
      isFullScreenModalOpen: false
    };
  }

  onDownloadClick(id, name) {
    this.apiService
      .fetchFile(`/api/messages/downloadFile/${id}`, name)
      .then(objectUrl => {
        var link = document.createElement("a");
        link.setAttribute("href", objectUrl);
        link.setAttribute("download", name);
        link.style.display = "none";
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
      })
      .catch(error => {
        toast.error(`Download file failed: ${error}`);
      });
  }

  onShareFileClick() {
    this.setState({
      isShareFileModalOpen: true
    });
  }

  onCloseShareFile() {
    this.setState({
      isShareFileModalOpen: false
    });
  }

  onImageFileClick() {
    this.setState({ isFullScreenModalOpen: true });
  }

  onImageFileFullScreenClose() {
    this.setState({ isFullScreenModalOpen: false });
  }

  componentDidMount() {
    if (this.isImage) {
      this.apiService
        .fetchFile(
          `/api/messages/downloadFile/${this.props.file.id}`,
          this.props.file.name
        )
        .then(objectUrl => {
          this.setState({ imageUrl: objectUrl });
        });
    }
  }

  render() {
    let file = this.props.file;
    return (
      <div className="content-message-file-item">
        {this.isImage && (
          <div className="file-image-item">
            <div className="file-image-item-name">
              {file.name}
            </div>
            <div className="file-item-image-container">
              <img
                className="file-item-image-file"
                src={this.state.imageUrl}
                onLoad={this.props.onImageLoadingFinished}
                onError={this.props.onImageLoadingFinished}
                alt={file.name}
                onClick={this.onImageFileClick}
              />
              {this.state.isFullScreenModalOpen && (
                <BlankModal onClose={this.onImageFileFullScreenClose}>
                  <div className="full-screen-image-container">
                    <img
                      className="full-screen-image"
                      src={this.state.imageUrl}
                      alt={file.name}
                    />
                    <CloseButton onClose={this.onImageFileFullScreenClose} />
                  </div>
                </BlankModal>
              )}
            </div>
            <div className="file-item-toolbar-container">
              <ContentMessageItemFileItemToolbar file={file} {...this.props} />
            </div>
          </div>
        )}
        {!this.isImage && (
          <div className="file-item">
            <Icon name="file outline" size="large" />
            <span title={file.name}>{file.name}</span>
            <div className="file-item-toolbar-container">
              <ContentMessageItemFileItemToolbar file={file} {...this.props} />
            </div>
          </div>
        )}
      </div>
    );
  }
}

export default ContentMessageItemFileItem;
