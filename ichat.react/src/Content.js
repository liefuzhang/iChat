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

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("/chatHub", {
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
  }

  render() {
    let params = this.props.match.params;
    let isChannel = params.section ? params.section === "channel" : true;
    let id = params.id ? +params.id : 0;
    return (
      <div id="contentContainer">
        <Sidebar isChannel={isChannel} id={id} />
        <div id="content">
          <ContentHeader isChannel={isChannel} id={id} />
          <ContentMessages
            section={params.section}
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
