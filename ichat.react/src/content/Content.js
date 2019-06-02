import React from "react";
import "./Content.css";
import ContentHeader from "./ContentHeader";
import ContentMessages from "./ContentMessages";
import ContentFooter from "./ContentFooter";
import Sidebar from "sidebar/Sidebar";
import * as signalR from "@aspnet/signalr";
import AuthService from "services/AuthService";

class Content extends React.Component {
  constructor(props) {
    super(props);

    this.authService = new AuthService(props);

    this.connection = new signalR.HubConnectionBuilder()
      // TODO use config here?
      .withUrl("https://localhost:44389/chatHub/", {
        accessTokenFactory: () => this.authService.getToken()
      })
      .build();
    this.connection.start().catch(function(err) {
      return console.error(err.toString());
    });

    this.userProfile = this.authService.getProfile();

    this.state = {
      savedActiveSidebarItem: undefined
    };
  }

  componentDidMount() {
    this.authService.fetch(`/api/app/activeSidebarItem`).then(item => {
      if (item) {
        this.setState({ savedActiveSidebarItem: item });
      }
    });
  }

  render() {
    let params = this.props.match.params;
    let section =
      params.section ||
      (this.state.savedActiveSidebarItem && !this.state.savedActiveSidebarItem.isChannel
        ? "user"
        : "channel");
    let isChannel = section === "channel";
    let id =
      +params.id ||
      (this.state.savedActiveSidebarItem && this.state.savedActiveSidebarItem.itemId
        ? this.state.savedActiveSidebarItem.itemId
        : 0);
    return (
      <div id="contentContainer">
        <Sidebar
          isChannel={isChannel}
          id={id}
          userProfile={this.userProfile}
          {...this.props}
        />
        <div id="content">
          <ContentHeader isChannel={isChannel} id={id} />
          <ContentMessages
            section={section}
            id={id}
            hubConnection={this.connection}
            userProfile={this.userProfile}
          />
          <ContentFooter isChannel={isChannel} id={id} />
        </div>
      </div>
    );
  }
}

export default Content;
