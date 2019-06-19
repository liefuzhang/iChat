import React from "react";
import "./MainPage.css";
import Sidebar from "sidebar/Sidebar";
import * as signalR from "@aspnet/signalr";
import Content from "content/Content";
import ApiService from "services/ApiService";
import ProfileService from "services/ProfileService";

class MainPage extends React.Component {
  constructor(props) {
    super(props);

    this.profileService = new ProfileService();
    this.apiService = new ApiService(props);

    this.connection = new signalR.HubConnectionBuilder()
      // TODO use config here?
      .withUrl("https://localhost:44389/chatHub/", {
        accessTokenFactory: () => this.profileService.getToken()
      })
      .build();
    this.connection.start().catch(function(err) {
      return console.error(err.toString());
    });

    this.userProfile = this.profileService.getProfile();

    this.onUserSessionDataChange = this.onUserSessionDataChange.bind(this);

    this.state = {
      savedActiveSidebarItem: undefined,
      userStatus: undefined,
      isPageLoading: true
    };
  }

  onUserSessionDataChange() {
    this.apiService.fetch(`/api/app/userSessionData`).then(data => {
      if (data) {
        this.setState({ savedActiveSidebarItem: data.activeSidebarItem });
        this.setState({ userStatus: data.userStatus });
      }
    });
  }

  componentDidMount() {
    this.onUserSessionDataChange();
  }

  render() {
    let params = this.props.match.params;
    let section =
      params.section ||
      (this.state.savedActiveSidebarItem &&
      !this.state.savedActiveSidebarItem.isChannel
        ? "conversation"
        : "channel");
    let isChannel = section === "channel";
    let id =
      +params.id ||
      (this.state.savedActiveSidebarItem &&
      this.state.savedActiveSidebarItem.itemId
        ? this.state.savedActiveSidebarItem.itemId
        : 0);
    return (
      <div id="mainPage">
        <Sidebar
          isChannel={isChannel}
          id={id}
          userProfile={this.userProfile}
          userStatus={this.state.userStatus}
          onUserSessionDataChange={this.onUserSessionDataChange}
          hubConnection={this.connection}
          {...this.props}
        />
        <Content
          isChannel={isChannel}
          id={id}
          section={section}
          hubConnection={this.connection}
          userProfile={this.userProfile}
        />
      </div>
    );
  }
}

export default MainPage;
