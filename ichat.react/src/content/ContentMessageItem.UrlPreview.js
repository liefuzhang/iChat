import React from "react";
import "./ContentMessageItem.UrlPreview.css";
import ContentMessageItemFileItemImage from "./ContentMessageItem.FileItemImage";

function ContentMessageItemUrlPreview(props) {
  let fileItem = {
    contentType: "image"
  };

  return (
    <div className="message-item-url-preview-container">
      {props.openGraphObjects.map((object, index) => (
        <div key={index} className="url-preview-item">
          <div className="url-preview-item-site-name">{object.siteName}</div>
          <a
            href={object.url}
            target="_blank"
            className="url-preview-item-title"
          >
            {object.title}
          </a>
          <div className="url-preview-item-description">
            {object.description}
          </div>
          <div className="url-preview-item-image">
            <ContentMessageItemFileItemImage
              imageUrl={object.imageUrl}
              imageName={object.title}
              {...props}
            />
          </div>
        </div>
      ))}
    </div>
  );
}

export default ContentMessageItemUrlPreview;
