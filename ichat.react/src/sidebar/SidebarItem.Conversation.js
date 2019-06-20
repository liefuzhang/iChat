import React from "react";
import "./SidebarItem.css";
import { Link } from "react-router-dom";
import { Popup } from "semantic-ui-react";

function SidebarItemConversation(props) {
  let truncateNameThreshold = 18;
  return (
    <Link to={`/conversation/${props.conversation.id}`}>
      <Popup
        trigger={
          <div
            className={
              "sidebar-item list-item" +
              (props.active ? " sidebar-active-item" : "") +
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
