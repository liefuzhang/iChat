import React from "react";
import "./SidebarItem.css";
import { Link } from "react-router-dom";
import AuthService from "services/AuthService";

function SidebarItem(props) {
  function setActiveSidebarItem() {
    var authService = new AuthService(props);

    authService.fetch(`/api/app/activeSidebarItem`, {
      method: "POST",
      body: JSON.stringify({
        isChannel: props.isChannel,
        itemId: props.id
      })
    });
  }

  return (
    <Link to={`/${props.section}/${props.id}`} onClick={setActiveSidebarItem}>
      <div className={"sidebar-item " + (props.active ? "active-item" : "")}>
        #&nbsp;
        {props.name}
      </div>
    </Link>
  );
}

export default SidebarItem;
