import React from "react";
import "./ContentMessageItem.FileItemImage.css";
import BlankModal from "modals/BlankModal";
import CloseButton from "components/CloseButton";

class ContentMessageItemFileItemImage extends React.Component {
  constructor(props) {
    super(props);

    this.onImageFileClick = this.onImageFileClick.bind(this);
    this.onImageFileFullScreenClose = this.onImageFileFullScreenClose.bind(
      this
    );

    this.state = {
      isFullScreenModalOpen: false
    };
  }

  onImageFileClick() {
    this.setState({ isFullScreenModalOpen: true });
  }

  onImageFileFullScreenClose() {
    this.setState({ isFullScreenModalOpen: false });
  }

  render() {
    return (
      <div className="file-item-image-container">
        <img
          className="file-item-image-file"
          src={this.props.imageUrl}
          onLoad={this.props.onImageLoadingFinished}
          onError={this.props.onImageLoadingFinished}
          alt={this.props.imageName}
          onClick={this.onImageFileClick}
        />
        {this.state.isFullScreenModalOpen && (
          <BlankModal onClose={this.onImageFileFullScreenClose}>
            <div className="full-screen-image-container">
              <img
                className="full-screen-image"
                src={this.props.imageUrl}
                alt={this.props.imageName}
              />
              <CloseButton onClose={this.onImageFileFullScreenClose} />
            </div>
          </BlankModal>
        )}
      </div>
    );
  }
}

export default ContentMessageItemFileItemImage;
