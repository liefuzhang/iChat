import React from "react";
import "./SidebarItem.css";
import { Link } from "react-router-dom";
import { Popup } from "semantic-ui-react";

function SidebarItemConversation(props) {
  let truncateNameThreshold = 16;
  let conversation = props.conversation;
  return (
    <Link to={`/conversation/${conversation.id}`}>
      <Popup
        trigger={
          <div
            className={
              "sidebar-item list-item" +
              (props.active ? " sidebar-active-item" : "") +
              (conversation.unreadMessageCount && !props.active
                ? " unread-item"
                : "")
            }
          >
            {conversation.userCount <= 2 &&
              (conversation.isTheOtherUserOnline ||
                conversation.userCount === 1) && (
                <span
                  className="sidebar-item-user-realtime-status"
                  title="active"
                >
                  <span className="sidebar-item-user-realtime-status-circle" />
                </span>
              )}
            {conversation.userCount <= 2 &&
              !(
                conversation.isTheOtherUserOnline ||
                conversation.userCount === 1
              ) && (
                <span
                  className="sidebar-item-user-realtime-status user-offline"
                  title="inactive"
                >
                  <span className="sidebar-item-user-realtime-status-circle" />
                </span>
              )}
            {conversation.userCount > 2 && (
              <span className="sidebar-item-other-user-count">
                {conversation.userCount - 1}
              </span>
            )}
            <span className="sidebar-item-name">{conversation.name}</span>
            <span
              className={
                "unread-badge" +
                (!conversation.unreadMessageCount || props.active
                  ? " invisible"
                  : "")
              }
            >
              {conversation.unreadMessageCount}
            </span>
          </div>
        }
        content={conversation.name}
        inverted
        position="top center"
        size="tiny"
        disabled={
          conversation.name.length > truncateNameThreshold ? false : true
        }
      />
    </Link>
  );
}

export default SidebarItemConversation;
