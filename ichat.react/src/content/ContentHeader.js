import React from "react";
import "./ContentHeader.css";
import ApiService from "services/ApiService";
import { Icon, Popup } from "semantic-ui-react";
import DropdownModal from "modals/DropdownModal";

class ContentHeader extends React.Component {
  constructor(props) {
    super(props);

    this.fetchData = this.fetchData.bind(this);
    this.apiService = new ApiService(props);
    this.onSettingButtonClicked = this.onSettingButtonClicked.bind(this);
    this.onCloseSettingDropdown = this.onCloseSettingDropdown.bind(this);

    this.state = {
      selectedChannel: {},
      selectedConversation: {},
      isSettingDropdownModalOpen: false
    };

    this.fetchData(this.props);
  }

  fetchData(props) {
    if (props.isChannel) {
      this.apiService
        .fetch(`/api/channels/${props.id}`)
        .then(channel => this.setState({ selectedChannel: channel }));
    } else {
      this.apiService
        .fetch(`/api/conversations/${props.id}`)
        .then(conversation =>
          this.setState({ selectedConversation: conversation })
        );
    }
  }

  componentDidUpdate(prevProps) {
    if (
      this.props.isChannel !== prevProps.isChannel ||
      this.props.id !== prevProps.id
    ) {
      this.fetchData(this.props);
    }
  }

  onSettingButtonClicked() {
    this.setState({
      isSettingDropdownModalOpen: true
    });
  }

  onCloseSettingDropdown(event) {
    this.setState({
      isSettingDropdownModalOpen: false
    });
  }

  render() {
    return (
      <div className="content-header">
        <div className="content-header-details">
          <div className="content-header-name">
            {this.props.isChannel
              ? "#" + this.state.selectedChannel.name
              : this.state.selectedConversation.name}
          </div>
          <div className="content-header-topic">
            {this.props.isChannel ? this.state.selectedChannel.topic : ""}
          </div>
        </div>
        <div className="content-header-toolbar">
          <div className="content-header-toolbar-item">
            <span onClick={this.onSettingButtonClicked}>
              <Popup
                trigger={<Icon name="setting" className="icon-setting" />}
                content={`${
                  this.props.isChannel ? "Channel" : "Conversation"
                } settings`}
                inverted
                position="bottom center"
                size="tiny"
              />
            </span>
            {this.state.isSettingDropdownModalOpen && (
              <DropdownModal onClose={this.onCloseSettingDropdown}>
                <div className="dropdown-container panel">
                  <section className="dropdown-section">
                    <ul>
                      {this.props.isChannel && (
                        <li>{`Leave #${this.state.selectedChannel.name}`}</li>
                      )}
                    </ul>
                  </section>
                </div>
              </DropdownModal>
            )}
          </div>
        </div>
      </div>
    );
  }
}

export default ContentHeader;
