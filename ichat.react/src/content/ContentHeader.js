import React from "react";
import "./ContentHeader.css";
import AuthService from "services/AuthService";

class ContentHeader extends React.Component {
  constructor(props) {
    super(props);

    this.fetchData = this.fetchData.bind(this);
    this.authService = new AuthService(props);

    this.state = {
      selectedChannel: {},
      selectedUser: {}
    };

    this.fetchData(this.props);
  }

  fetchData(props) {
    if (props.isChannel) {
      this.authService.fetch(`/api/channels/${props.id}`)
        .then(channel => this.setState({ selectedChannel: channel }));
    } else {
      this.authService.fetch(`/api/users/${props.id}`)
        .then(user => this.setState({ selectedUser: user }));
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
            : this.state.selectedUser.displayName}
        </div>
        <div className="content-header-topic">
          {this.props.isChannel ? this.state.selectedChannel.topic : ""}
        </div>
      </div>
    );
  }
}

export default ContentHeader;
