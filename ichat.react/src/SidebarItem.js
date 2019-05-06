import React from "react";
import { Link } from "react-router-dom";

function SidebarItem(props) {
  return (
    <Link to={`/${props.section}/${props.id}`}>
      <div className="sidebar-item">
        #&nbsp;
        {props.name}
      </div>
    </Link>
  );
}

export default SidebarItem;
