import React from "react";
import "./SidebarItem.css";
import { Link } from "react-router-dom";
import { Popup } from "semantic-ui-react";
import ApiService from "services/ApiService";

function SidebarItemChannel(props) {
  function setActiveSidebarItem() {
    var apiService = new ApiService(props);
    apiService.fetch(`/api/app/activeSidebarItem`, {
      method: "POST",
      body: JSON.stringify({
        isChannel: true,
        itemId: props.channel.id
      })
    });
  }

  let truncateNameThreshold = 18;
  return (
    <Link to={`/channel/${props.channel.id}`} onClick={setActiveSidebarItem}>
      <Popup
        trigger={
          <div
            className={
              "sidebar-item list-item" +
              (props.active ? " active-item" : "") +
              (props.channel.hasUnreadMessage && !props.active
                ? " unread-item"
                : "")
            }
          >
            <span className="sidebar-item-name">
              #&nbsp;{props.channel.name}
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
