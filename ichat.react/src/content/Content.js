import React from "react";
import "./Content.css";
import ContentHeader from "content/ContentHeader";
import ContentMessages from "content/ContentMessages";
import ContentFooter from "content/ContentFooter";

function Content(props) {
  return (
    <div id="content">
      <ContentHeader isChannel={props.isChannel} id={props.id} />
      <ContentMessages
        section={props.section}
        id={props.id}
        hubConnection={props.hubConnection}
        userProfile={props.userProfile}
      />
      <ContentFooter
        isChannel={props.isChannel}
        section={props.section}
        id={props.id}
        hubConnection={props.hubConnection}
      />
    </div>
  );
}

export default Content;
