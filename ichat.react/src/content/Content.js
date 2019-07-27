import React from "react";
import "./Content.css";
import ApiService from "services/ApiService";
import ContentHeader from "content/ContentHeader";
import ContentMessages from "content/ContentMessages";
import ContentFooter from "content/ContentFooter";
import { Loader, Image, Segment } from "semantic-ui-react";
import UserPopup from "../components/UserPopup";

class Content extends React.Component {
  constructor(props) {
    super(props);

    this.onFinishLoading = this.onFinishLoading.bind(this);
    this.onOpenUserPopup = this.onOpenUserPopup.bind(this);
    this.onUserPopupClose = this.onUserPopupClose.bind(this);
    this.apiService = new ApiService(props);

    if (props.hubConnection) {
      props.hubConnection.on("ConversationUserListChanged", () =>
        this.fetchUsers()
      );
    }

    this.state = {
      isPageLoading: true,
      messageChannelUserList: []
    };
  }

  fetchUsers() {
    let url = this.props.isChannel
      ? `/api/channels/${this.props.id}/users`
      : `/api/conversations/${this.props.id}/users`;

    this.apiService
      .fetch(url)
      .then(users => this.setState({ messageChannelUserList: users }));
  }
  
  onUserPopupClose() {
    this.popupClickedTarget = undefined;
    this.popupUser = undefined;
    this.setState({ isUserPopupOpen: false });
  }

  onOpenUserPopup(clickedTarget, userId) {
    let user = this.state.messageChannelUserList.find(
      user => user.id === userId
    );
    if (user) this.openUserPopup(clickedTarget, user);
    else {
      this.apiService.fetch(`/api/users/${userId}`).then(user => {
        if (user) this.openUserPopup(clickedTarget, user);
      });
    }
  }

  openUserPopup(clickedTarget, user) {
    this.popupClickedTarget = clickedTarget;
    this.popupUser = user;
    this.setState({ isUserPopupOpen: true });
  }

  componentDidMount() {
    this.fetchUsers();
  }

  componentDidUpdate(prevProps) {
    if (
      this.props.isChannel !== prevProps.isChannel ||
      this.props.id !== prevProps.id
    ) {
      this.fetchUsers();
    }
  }

  onFinishLoading() {
    this.setState({ isPageLoading: false });
  }

  render() {
    return (
      <div id="content">
        {this.state.isPageLoading && (
          <Segment>
            <Loader active>Loading</Loader>
            <Image src="https://react.semantic-ui.com/images/wireframe/short-paragraph.png" />
            <Image src="https://react.semantic-ui.com/images/wireframe/short-paragraph.png" />
          </Segment>
        )}
        <ContentHeader {...this.props} />
        <ContentMessages
          messageChannelUserList={this.state.messageChannelUserList}
          onFinishLoading={this.onFinishLoading}
          onOpenUserPopup={this.onOpenUserPopup}
          {...this.props}
        />
        <ContentFooter
          isChannel={this.props.isChannel}
          section={this.props.section}
          id={this.props.id}
          hubConnection={this.props.hubConnection}
          userProfile={this.props.userProfile}
        />
        {this.state.isUserPopupOpen && (
          <UserPopup
            user={this.popupUser}
            clickedTarget={this.popupClickedTarget}
            onClose={this.onUserPopupClose}
            {...this.props}
          />
        )}
      </div>
    );
  }
}

export default Content;
