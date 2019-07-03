import React from "react";
import "./SidebarItem.css";
import { Link } from "react-router-dom";
import { Icon, Popup } from "semantic-ui-react";
import UserStatusService from "services/UserStatusService";

function SidebarItemConversation(props) {
  let truncateNameThreshold = 16;
  let conversation = props.conversation;
  let userStatusService = new UserStatusService();

  return (
    <Link to={`/conversation/${conversation.id}`}>
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
            <span className="sidebar-item-user-realtime-status" title="active">
              <span className="sidebar-item-user-realtime-status-circle" />
            </span>
          )}
        {conversation.userCount <= 2 &&
          !(
            conversation.isTheOtherUserOnline || conversation.userCount === 1
          ) && (
            <span
              className="sidebar-item-user-realtime-status user-offline"
              title="away"
            >
              <span className="sidebar-item-user-realtime-status-circle" />
            </span>
          )}
        {conversation.userCount > 2 && (
          <span className="sidebar-item-other-user-count">
            {conversation.userCount - 1}
          </span>
        )}
        <Popup
          trigger={
            <span className="sidebar-item-name">{conversation.name}</span>
          }
          content={conversation.name}
          inverted
          position="top center"
          size="tiny"
          disabled={
            conversation.name.length > truncateNameThreshold ? false : true
          }
        />
        {(() => {
          if (
            conversation.otherUserStatus &&
            userStatusService.isNotActive(conversation.otherUserStatus)
          ) {
            let statusName = userStatusService.getStatusName(
              conversation.otherUserStatus
            );
            return (
              <Popup
                trigger={
                  <Icon
                    name="flag"
                    className="status-icon sidebar-item-other-user-status"
                  />
                }
                content={statusName}
                inverted
                position="top center"
                size="mini"
              />
            );
          }
        })()}
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
    </Link>
  );
}

export default SidebarItemConversation;
