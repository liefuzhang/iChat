import React from "react";
import "./ContentHeader.css";
import ApiService from "services/ApiService";

class ContentHeader extends React.Component {
  constructor(props) {
    super(props);

    this.fetchData = this.fetchData.bind(this);
    this.apiService = new ApiService(props);

    this.state = {
      selectedChannel: {},
      selectedConversation: {}
    };

    this.fetchData(this.props);
  }

  fetchData(props) {
    if (props.isChannel) {
      this.apiService.fetch(`/api/channels/${props.id}`)
        .then(channel => this.setState({ selectedChannel: channel }));
    } else {
      this.apiService.fetch(`/api/conversations/${props.id}`)
        .then(conversation => this.setState({ selectedConversation: conversation }));
    }
  }
 
  componentDidUpdate(prevProps) {
    if (this.props.isChannel !== prevProps.isChannel ||
      this.props.id !== prevProps.id) {
      this.fetchData(this.props);
    }
  }

  render() {
    return (
      <div className="content-header">
        <div className="content-header-name">
          {this.props.isChannel
            ? "#" + this.state.selectedChannel.name
            : this.state.selectedConversation.name}
        </div>
        <div className="content-header-topic">
          {this.props.isChannel ? this.state.selectedChannel.topic : ""}
        </div>
      </div>
    );
  }
}

export default ContentHeader;
