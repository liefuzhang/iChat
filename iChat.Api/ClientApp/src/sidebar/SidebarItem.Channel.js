import React from "react";
import "./SidebarItem.css";
import { Link } from "react-router-dom";
import { Popup } from "semantic-ui-react";

function SidebarItemChannel(props) {
  let truncateNameThreshold = 18;
  return (
    <Link to={`/channel/${props.channel.id}`}>
      <Popup
        trigger={
          <div
            className={
              "sidebar-item list-item" +
              (props.active ? " sidebar-active-item" : "") +
              (props.channel.hasUnreadMessage && !props.active
                ? " unread-item"
                : "")
            }
          >
            <span className="sidebar-item-name">
              {props.channel.name}
            </span>
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
        }
        content={props.channel.name}
        inverted
        position="top center"
        size="tiny"
        disabled={props.channel.name.length > truncateNameThreshold ? false : true}
      />
    </Link>
  );
}

export default SidebarItemChannel;
