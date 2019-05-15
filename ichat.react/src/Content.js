import React from "react";
import "./Content.css";
import ContentHeader from "./ContentHeader";
import ContentMessages from "./ContentMessages";
import ContentFooter from "./ContentFooter";
import Sidebar from "./Sidebar";
import * as signalR from "@aspnet/signalr";
import AuthService from "./services/AuthService";

class Content extends React.Component {
  constructor(props) {
    super(props);

    this.authService = new AuthService(props);

    this.connection = new signalR.HubConnectionBuilder()
      // TODO use config here?
      .withUrl("https://localhost:44389/chatHub/", {
        accessTokenFactory: () => new AuthService().getToken()
      })
      .build();
    this.connection
      .start()
      .then(() => {
        //todo change: add connection to all the users' channels and private messages
        if (props.match.params.section === "channel")
          this.connection
            .invoke("AddToChannelGroup", props.match.params.id)
            .catch(function(err) {
              return console.error(err.toString());
            });
      })
      .catch(function(err) {
        return console.error(err.toString());
      });

    this.userProfile = this.authService.getProfile();
  }

  render() {
    let params = this.props.match.params;
    let section = params.section || "channel";
    let isChannel = section === "channel";
    let id = params.id ? +params.id : 0;
    return (
      <div id="contentContainer">
        <Sidebar isChannel={isChannel} id={id} userProfile={this.userProfile} />
        <div id="content">
          <ContentHeader isChannel={isChannel} id={id} />
          <ContentMessages
            section={section}
            id={id}
            hubConnection={this.connection}
          />
          <ContentFooter isChannel={isChannel} id={id} />
        </div>
      </div>
    );
  }
}

export default Content;
