import React from "react";
import "./Content.css";
import ContentHeader from "./ContentHeader";
import ContentMessages from "./ContentMessages";
import ContentFooter from "./ContentFooter";
import Sidebar from "./Sidebar";

function Content(props) {
  let params = props.match.params;
  let isChannel = params.section === "channel";

  return (
    <div>
      <Sidebar />
      <div id="content">
        <ContentHeader isChannel={isChannel} id={params.id} />
        <ContentMessages section={params.section} id={params.id} />
        <ContentFooter isChannel={isChannel} id={params.id} />
      </div>
    </div>
  );
}

export default Content;
