import React from "react";
import "./SidebarItem.css";
import { Link } from "react-router-dom";
import AuthService from "services/AuthService";

function SidebarItemChannel(props) {
  function setActiveSidebarItem() {
    var authService = new AuthService(props);
    authService.fetch(`/api/app/activeSidebarItem`, {
      method: "POST",
      body: JSON.stringify({
        isChannel: true,
        itemId: props.channel.id
      })
    });
  }

  return (
    <Link to={`/channel/${props.channel.id}`} onClick={setActiveSidebarItem}>
      <div
        title={props.channel.name}
        className={
          "sidebar-item list-item" +
          (props.active ? " active-item" : "") +
          (props.channel.hasUnreadMessage && !props.active
            ? " unread-item"
            : "")
        }
      >
        <span className="sidebar-item-name">#&nbsp;{props.channel.name}</span>
        <span
          className={
            "unread-badge" +
            (!props.channel.unreadMentionCount || props.active
              ? " invisible"
              : "")
          }
        >
          {props.channel.unreadMentionCount}
        </span>
      </div>
    </Link>
  );
}

export default SidebarItemChannel;
