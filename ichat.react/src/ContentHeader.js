import React from "react";
import "./ContentHeader.css";

class ContentHeader extends React.Component {
  constructor(props) {
    super(props);

    this.fetchData = this.fetchData.bind(this);
    this.isChannel = this.props.isChannel;

    this.state = {
      selectedChannel: {},
      selectedUser: {}
    };
  }

  fetchData(props) {
    if (props.isChannel) {
      fetch(`/api/channels/${props.id}`)
        .then(response => response.json())
        .then(channel => this.setState({ selectedChannel: channel }));
    } else {
      fetch(`/api/users/${props.id}`)
        .then(response => response.json())
        .then(user => this.setState({ selectedUser: user }));
    }
  }

  componentWillMount() {
    this.fetchData(this.props);
  }

  componentWillReceiveProps(nextProps) {
    if (
      this.props.isChannel !== nextProps.isChannel ||
      this.props.id !== nextProps.id
    ) {
      this.fetchData(nextProps);
      this.isChannel = nextProps.isChannel;
    }
  }

  render() {
    return (
      <div className="content-header">
        <div className="content-header-name">
          {this.isChannel
            ? "#" + this.state.selectedChannel.name
            : this.state.selectedUser.name}
        </div>
        <div className="content-header-topic">
          {this.isChannel ? this.state.selectedChannel.topic : ""}
        </div>
      </div>
    );
  }
}

export default ContentHeader;
