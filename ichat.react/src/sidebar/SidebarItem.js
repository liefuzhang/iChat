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
      <div
        title={props.name}
        className={
          "sidebar-item" +
          (props.active ? " active-item" : "") +
          (props.unreadMessageCount && !props.active ? " unread-item" : "")
        }
      >
        <span>
          {props.isChannel && <span># </span>}
          {props.name}
        </span>
        {props.unreadMessageCount && !props.active && (
          <span className="unread-badge">{props.unreadMessageCount}</span>
        )}
      </div>
    </Link>
  );
}

export default SidebarItem;
