import React from "react";
import { toast } from "react-toastify";
import { Icon, Dropdown } from "semantic-ui-react";
import ApiService from "services/ApiService";
import "./ShareFileForm.css";

class ShareFileForm extends React.Component {
  constructor(props) {
    super(props);

    this.onShareFileFormSubmit = this.onShareFileFormSubmit.bind(this);
    this.changeSelectedShareWith = this.changeSelectedShareWith.bind(this);
    this.channelPrefix = "channel_";
    this.conversationPrefix = "conversation_";
    this.apiService = new ApiService(props);

    this.state = {
      shareWithList: []
    };

    this.selectedShareWith = undefined;
  }

  componentDidMount() {
    var channelsPromise = this.apiService.fetch("/api/channels/forUser");
    var conversationsPromise = this.apiService.fetch(
      "/api/conversations/recent"
    );

    Promise.all([channelsPromise, conversationsPromise]).then(
      ([channels, conversations]) => {
        let shareWithList = [];
        channels.forEach(c => {
          shareWithList.push({
            key: `${this.channelPrefix}${c.id}`,
            text: c.name,
            value: `${this.channelPrefix}${c.id}`
          });
        });
        conversations.forEach(c => {
          shareWithList.push({
            key: `${this.conversationPrefix}${c.id}`,
            text: c.name,
            value: `${this.conversationPrefix}${c.id}`
          });
        });
        this.setState({ shareWithList: shareWithList });
      }
    );
  }

  onShareFileFormSubmit(event) {
    event.preventDefault();
    let shareWith = this.selectedShareWith;
    if (!shareWith) return;

    let button = event.currentTarget.querySelector("button[type='submit']");
    button.classList.add("disabled-button");

    let targetName, value;
    if (shareWith.key.startsWith(`${this.channelPrefix}`)) {
      targetName = "channel";
      value = +shareWith.value.substring(this.channelPrefix.length);
    } else {
      targetName = "conversation";
      value = +shareWith.value.substring(this.conversationPrefix.length);
    }

    let url = `/api/messages/${targetName}/${value}/shareFile/${
      this.props.file.id
    }`;
    this.apiService
      .fetch(url, {
        method: "POST"
      })
      .then(() => {
        this.props.onClose();
        this.props.history.push(`/${targetName}/${shareWith.value}`);
      })
      .catch(error => {
        toast.error(`Share file failed: ${error}`);
        button.classList.remove("disabled-button");
      });
  }

  changeSelectedShareWith(event, item) {
    let selectedValue = item.value;
    let selectedItem = this.state.shareWithList.find(
      item => item.value === selectedValue
    );
    this.selectedShareWith = selectedItem;
  }

  render() {
    let file = this.props.file;
    return (
      <div className="form-container form-with-dropdown">
        <h1 style={{ textAlign: "center" }}>Share file</h1>
        <form
          id="shareFileForm"
          method="post"
          onSubmit={this.onShareFileFormSubmit}
        >
          <p className="form-description">Share with</p>
          <Dropdown
            placeholder="Search for channel or conversation"
            fluid
            search
            selection
            options={this.state.shareWithList}
            onChange={this.changeSelectedShareWith}
          />
          <div className="file-container">
            <div className="file-item">
              <Icon name="file outline" size="large" />
              <span title={file.name}>{file.name}</span>
            </div>
          </div>
          <button type="submit" className="btn form-control">
            Share
          </button>
        </form>
      </div>
    );
  }
}

export default ShareFileForm;
