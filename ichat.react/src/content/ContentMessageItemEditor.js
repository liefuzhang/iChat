import React from "react";
import { Icon } from "semantic-ui-react";
import "./ContentMessageItemEditor.css";
import ContentEditor from "./ContentEditor";
import ApiService from "services/ApiService";

class ContentMessageItemEditor extends React.Component {
  constructor(props) {
    super(props);

    this.apiService = new ApiService(props);

    this.state = {};
  }

  render() {
    return (
      <div className="message-item-common-container message-item-edit-container">
        <img
          className="user-identicon"
          src={this.props.userProfile.identiconPath}
        />
        <div className="message-item-editor-container">
          <div id="messageItemEditor">
            <ContentEditor
              containerId="messageItemEditor"
              isEditing={true}
              currentContent={this.props.message.content}
              {...this.props}
            />
          </div>
          <div className="message-item-editor-buttons">
            <button className="btn white-btn" onClick={this.props.onCancel}>Cancel</button>
            <button className="btn">Save Changes</button>
          </div>
        </div>
      </div>
    );
  }
}

export default ContentMessageItemEditor;
