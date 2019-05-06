import React from "react";
import "./Sidebar.css";
import SidebarItem from "./SidebarItem";

class Sidebar extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      channels: [],
      directMessageUsers: []
    };
  }

  componentDidMount() {
    fetch("/api/channels")
      .then(response => response.json())
      .then(channels => this.setState({ channels }));

    fetch("/api/users")
      .then(response => response.json())
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
            <SidebarItem key={c.id}  section="user" name={c.displayName} id={c.id} />
          ))}
        </section>
      </div>
    );
  }
}

export default Sidebar;
