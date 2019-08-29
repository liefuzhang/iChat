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

    this.isPulseSignalJustSent = false;
    this.profileService = new ProfileService();
    this.apiService = new ApiService(props);

    this.connection = new signalR.HubConnectionBuilder()
      // TODO use config here?
      .withUrl(`http://ichat.liefuzhang.com:58314/chatHub/`, {
        accessTokenFactory: () => this.profileService.getToken()
      })
      .build();
    this.connection.start().catch(function(err) {
      return console.error(err.toString());
    });

    this.onUserSessionDataChange = this.onUserSessionDataChange.bind(this);
    this.onProfileUpdated = this.onProfileUpdated.bind(this);
    this.sendPulseSignal = this.sendPulseSignal.bind(this);

    this.state = {
      savedActiveSidebarItem: undefined,
      userStatus: undefined,
      userProfile: this.profileService.getProfile(),
      userSessionDataLoaded: false
    };

    this.sendPulseSignal();
  }

  onUserSessionDataChange() {
    this.apiService.fetch(`/api/app/userSessionData`).then(data => {
      if (data) {
        this.setState({
          savedActiveSidebarItem: data.activeSidebarItem,
          userStatus: data.userStatus,
          userSessionDataLoaded: true
        });
      }
    });
  }

  onProfileUpdated() {
    this.apiService.fetch(`/api/identity/userProfile`).then(profile => {
      if (profile) {
        this.profileService.setProfile(profile);
        this.setState({ userProfile: profile });
      }
    });
  }

  sendPulseSignal() {
    if (!this.isPulseSignalJustSent) {
      this.isPulseSignalJustSent = true;
      this.apiService.fetch("/api/app/userPulseSignal", {
        method: "POST"
      });
      setTimeout(() => {
        this.isPulseSignalJustSent = false;
      }, 30000); // throttle it to once in 30 secs
    }
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
        : this.state.userProfile.defaultChannelId);

    return (
      <div className="main-page-container" onClick={this.sendPulseSignal}>
        {this.state.userSessionDataLoaded && (
          <div id="mainPage">
            <Sidebar
              isChannel={isChannel}
              id={id}
              hubConnection={this.connection}
              userProfile={this.state.userProfile}
              {...this.props}
            />
            <Content
              isChannel={isChannel}
              id={id}
              section={section}
              hubConnection={this.connection}
              onProfileUpdated={this.onProfileUpdated}
              onUserSessionDataChange={this.onUserSessionDataChange}
              userProfile={this.state.userProfile}
              userStatus={this.state.userStatus}
              {...this.props}
            />
          </div>
        )}
      </div>
    );
  }
}

export default MainPage;
