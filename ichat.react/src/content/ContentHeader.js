import React from "react";
import "./ContentHeader.css";
import ApiService from "services/ApiService";
import { Icon, Popup } from "semantic-ui-react";
import DropdownModal from "modals/DropdownModal";
import Modal from "modals/Modal";
import { toast } from "react-toastify";
import InviteOtherMembersForm from "modalForms/InviteOtherMembersForm";
import ShowMemberListForm from "modalForms/ShowMemberListForm";
import ContentHeaderUserInfo from "./ContentHeaderUserInfo";
import ContentHeaderSettings from "./ContentHeaderSettings";

class ContentHeader extends React.Component {
  constructor(props) {
    super(props);

    this.fetchData = this.fetchData.bind(this);
    this.apiService = new ApiService(props);

    if (props.hubConnection) {
      props.hubConnection.on("ConversationUserListChanged", () =>
        this.fetchData(this.props)
      );
    }

    this.state = {
      selectedChannel: {},
      selectedConversation: {}
    };
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

  componentDidMount() {
    this.fetchData(this.props);
  }

  componentDidUpdate(prevProps) {
    if (
      this.props.isChannel !== prevProps.isChannel ||
      this.props.id !== prevProps.id
    ) {
      this.fetchData(this.props);
    }
  }

  render() {
    return (
      <div className="content-header">
        <div className="content-header-details">
          <div className="content-header-name">
            {this.props.isChannel
              ? this.state.selectedChannel.name
              : this.state.selectedConversation.name}
            <div className="content-header-settings">
              <ContentHeaderSettings
                selectedChannel={this.state.selectedChannel}
                selectedConversation={this.state.selectedConversation}
                {...this.props}
              />
            </div>
          </div>
          <div className="content-header-topic">
            {this.props.isChannel ? this.state.selectedChannel.topic : ""}
          </div>
        </div>
        <div>
          <ContentHeaderUserInfo {...this.props} />
        </div>
      </div>
    );
  }
}

export default ContentHeader;
