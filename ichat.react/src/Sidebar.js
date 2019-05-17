import React from "react";
import "./Sidebar.css";
import SidebarItem from "./SidebarItem";
import SidebarHeader from "./SidebarHeader";
import AuthService from "./services/AuthService";

class Sidebar extends React.Component {
  constructor(props) {
    super(props);

    this.authService = new AuthService(props);

    this.state = {
      channels: [],
      directMessageUsers: []
    };

    this.fecthData();
  }

  fecthData() {
    this.authService
      .fetch("/api/channels")
      .then(channels => this.setState({ channels }));

    this.authService
      .fetch("/api/users")
      .then(directMessageUsers => this.setState({ directMessageUsers }));
  }

  render() {
    return (
      <div id="sidebar">
        <section>
          <SidebarHeader userProfile={this.props.userProfile}></SidebarHeader>
        </section>
        <section>
          <div className="section-title">CHANNELS</div>
          {this.state.channels.map(c => {
            let active = false;
            if (this.props.isChannel && this.props.id === c.id) active = true;
            if (
              this.props.isChannel &&
              this.props.id === 0 &&
              c.name === "general"
            )
              active = true;
            return (
              <SidebarItem
                key={c.id}
                section="channel"
                name={c.name}
                id={c.id}
                active={active}
              />
            );
          })}
        </section>

        <section>
          <div className="section-title">DIRECT MESSAGES</div>
          {this.state.directMessageUsers.map(c => {
            let active = false;
            if (!this.props.isChannel && this.props.id === c.id) active = true;
            return (
              <SidebarItem
                key={c.id}
                section="user"
                name={c.displayName}
                id={c.id}
                active={active}
              />
            );
          })}
        </section>
      </div>
    );
  }
}

export default Sidebar;
