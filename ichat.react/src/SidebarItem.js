import React from "react";
import "./SidebarItem.css";
import { Link } from "react-router-dom";

function SidebarItem(props) {
  return (
    <Link to={`/${props.section}/${props.id}`}>
      <div className={"sidebar-item " + (props.active ? "active-item":"")}>
        #&nbsp;
        {props.name}
      </div>
    </Link>
  );
}

export default SidebarItem;
