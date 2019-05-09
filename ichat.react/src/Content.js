import React from "react";
import "./Content.css";
import ContentHeader from "./ContentHeader";
import ContentMessages from "./ContentMessages";
import ContentFooter from "./ContentFooter";
import SignalRHubService from "./services/SignalRHubService";
import Sidebar from "./Sidebar";

class Content extends React.Component {
  constructor(props) {
    super(props);

    let hubService = new SignalRHubService();
    hubService.connect().then(() => {
      //todo change: add connection to all the users' channels and private messages
      if (props.match.params.section === "channel")
        hubService.addUserToChannelGroup(props.match.params.id);
    });
  }

  render() {
    let params = this.props.match.params;
    let isChannel = params.section === "channel";

    return (
      <div id="contentContainer">
        <div id="content">
          <ContentHeader isChannel={isChannel} id={params.id} />
          <ContentMessages section={params.section} id={params.id} />
          <ContentFooter isChannel={isChannel} id={params.id} />
        </div>
      </div>
    );
  }
}

export default Content;
