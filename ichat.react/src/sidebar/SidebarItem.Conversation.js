import React from "react";
import "./SidebarItem.css";
import { Link } from "react-router-dom";
import AuthService from "services/AuthService";
import { Popup } from "semantic-ui-react";

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
  
  let truncateNameThreshold = 18;
  return (
    <Link
      to={`/conversation/${props.conversation.id}`}
      onClick={setActiveSidebarItem}
    >
      <Popup
        trigger={
          <div
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
        }
        content={props.conversation.name}
        inverted
        position="top center"
        size="tiny"
        disabled={
          props.conversation.name.length > truncateNameThreshold ? false : true
        }
      />
    </Link>
  );
}

export default SidebarItemConversation;
