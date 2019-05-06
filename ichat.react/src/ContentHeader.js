import React from "react";
import "./Sidebar.css";

class ContentHeader extends React.Component {
  constructor(props) {
    super(props);

    this.fetchData = this.fetchData.bind(this);

    this.state = {
      selectedChannel: {},
      selectedUser: {}
    };
  }

  fetchData() {
    fetch(`/api/messages/${this.params.section}/${this.params.id}`)
      .then(response => response.json())
      .then(messages => this.setState({ messages }));
  }

  componentWillMount() {
    this.fetchData();
  }

  componentWillReceiveProps(nextProps) {
    if (
      this.props.isChannel !== nextProps.isChannel ||
      this.props.id !== nextProps.id
    ) {
      if (nextProps.isChannel) {
        fetch(`/api/channels/${nextProps.id}`)
          .then(response => response.json())
          .then(channel => this.setState({ selectedChannel: channel }));
      } else {
        fetch(`/api/users/${nextProps.id}`)
          .then(response => response.json())
          .then(user => this.setState({ selectedUser: user }));
      }
    }
  }

  componentDidMount() {
    if (this.props.isChannel) {
      fetch(`/api/channels/${this.props.id}`)
        .then(response => response.json())
        .then(channel => this.setState({ selectedChannel: channel }));
    } else {
      fetch(`/api/users/${this.props.id}`)
        .then(response => response.json())
        .then(user => this.setState({ selectedUser: user }));
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
