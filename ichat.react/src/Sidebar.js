import React from "react";
import "./Sidebar.css";
import SidebarItem from "./SidebarItem";
import AuthService from "./services/AuthService";

class Sidebar extends React.Component {
  constructor(props) {
    super(props);

    this.authService = new AuthService(props);

    this.state = {
      channels: [],
      directMessageUsers: []
    };
  }

  componentDidMount() {
    this.authService
      .fetch("/api/channels")
      .then(channels => this.setState({ channels }));

    this.authService
      .fetch("/api/users")
      .then(directMessageUsers => this.setState({ directMessageUsers }));
  }

  render() {
    return (
      <div id="sideBar">
        <section>
          <div className="section-title">CHANNELS</div>
          {this.state.channels.map(c => (
            <SidebarItem key={c.id} section="channel" name={c.name} id={c.id} />
          ))}
        </section>

        <section>
          <div className="section-title">DIRECT MESSAGES</div>
          {this.state.directMessageUsers.map(c => (
            <SidebarItem
              key={c.id}
              section="user"
              name={c.displayName}
              id={c.id}
            />
          ))}
        </section>
      </div>
    );
  }
}

export default Sidebar;
