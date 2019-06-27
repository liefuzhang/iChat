import React from "react";
import "./ContentMessageItem.FileItem.css";
import { Icon, Popup } from "semantic-ui-react";
import ApiService from "services/ApiService";
import { toast } from "react-toastify";
import ShareFileForm from "modalForms/ShareFileForm";
import Modal from "modals/Modal";

class ContentMessageItemFileItem extends React.Component {
  constructor(props) {
    super(props);

    this.onShareFileClick = this.onShareFileClick.bind(this);
    this.onCloseShareFile = this.onCloseShareFile.bind(this);
    this.apiService = new ApiService(props);

    this.state = {
      isShareFileModalOpen: false
    };
  }

  onDownloadClick(id, name) {
    this.apiService
      .fetchFile(`/api/messages/downloadFile/${id}`, name)
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

  render() {
    let file = this.props.file;
    return (
      <div className="file-item-container">
        <div className="file-item">
          <Icon name="file outline" size="large" />
          <span title={file.name}>{file.name}</span>
          <div className="file-item-toolbar">
            <Popup
              trigger={
                <div
                  className="file-item-toolbar-item"
                  onClick={() => this.onDownloadClick(file.id, file.name)}
                >
                  <Icon name="cloud download" />
                </div>
              }
              content="Download"
              inverted
              position="top center"
              size="tiny"
            />
            <Popup
              trigger={
                <div
                  className="file-item-toolbar-item"
                  onClick={this.onShareFileClick}
                >
                  <Icon name="share alternate" />
                </div>
              }
              content="Share"
              inverted
              position="top center"
              size="tiny"
            />
            <div>
              {this.state.isShareFileModalOpen && (
                <Modal onClose={this.onCloseShareFile}>
                  <ShareFileForm
                    file={file}
                    onClose={this.onCloseShareFile}
                    {...this.props}
                  />
                </Modal>
              )}
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default ContentMessageItemFileItem;
