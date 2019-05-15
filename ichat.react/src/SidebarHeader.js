import React from "react";
import "./SidebarHeader.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

class SidebarHeader extends React.Component {
  constructor(props) {
    super(props);
  }

  onHeaderClick(event) {
    document.querySelector(".sidebar-header-dropdown").classList.toggle("hide");
  }

  componentDidMount() {
    document
      .querySelector(".sidebar-header-container")
      .addEventListener("click", this.onHeaderClick);
  }

  componentWillUnmount() {
    document
      .querySelector(".sidebar-header-container")
      .removeEventListener("click", this.onHeaderClick);
  }

  render() {
    return (
      <div className="sidebar-header">
        <div className="sidebar-header-container">
          <div>
            <span className="sidebar-header-workspace-name">
              {this.props.userProfile.workspaceName}{" "}
            </span>
            <FontAwesomeIcon icon="chevron-down" />
          </div>
          <div className="sidebar-header-user-name">
            {this.props.userProfile.displayName}
          </div>
        </div>
        <div className="sidebar-header-dropdown panel hide">
          <section className="sidebar-header-dropdown-section">
            <div className="sidebar-header-dropdown-section-header">
              {this.props.userProfile.displayName}
            </div>
            <ul>
              <li>Set status</li>
              <li>Profile</li>
            </ul>
          </section>
          <section className="sidebar-header-dropdown-section">
            <div className="sidebar-header-dropdown-section-header">
              {this.props.userProfile.workspaceName}
            </div>
            <ul>
              <li>Invite people</li>
              <li>Log out of {this.props.userProfile.workspaceName}</li>
            </ul>
          </section>
        </div>
      </div>
    );
  }
}

export default SidebarHeader;
