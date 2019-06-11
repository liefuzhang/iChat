import React from "react";
import "./SidebarItem.css";
import { Link } from "react-router-dom";
import AuthService from "services/AuthService";

function SidebarItemConversation(props) {
  function setActiveSidebarItem() {
    var authService = new AuthService(props);
    authService.fetch(`/api/app/activeSidebarItem`, {
      method: "POST",
      body: JSON.stringify({
        isChannel: false,
        itemId: props.conversation.id
      })
    });
  }

  return (
    <Link
      to={`/conversation/${props.conversation.id}`}
      onClick={setActiveSidebarItem}
    >
      <div
        title={props.conversation.name}
        className={
          "sidebar-item list-item" +
          (props.active ? " active-item" : "") +
          (props.conversation.unreadMessageCount && !props.active
            ? " unread-item"
            : "")
        }
      >
        <span className="sidebar-item-name">{props.conversation.name}</span>
        <span
          className={
            "unread-badge" +
            (!props.conversation.unreadMessageCount || props.active
              ? " invisible"
              : "")
          }
        >
          {props.conversation.unreadMessageCount}
        </span>
      </div>
    </Link>
  );
}

export default SidebarItemConversation;
