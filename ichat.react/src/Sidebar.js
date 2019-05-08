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

    this.myHeaders = new Headers();
    this.myHeaders.append(
      "Authorization",
      "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyIiwibmJmIjoxNTU3MzA5NjQyLCJleHAiOjE1NTc5MTQ0NDIsImlhdCI6MTU1NzMwOTY0Mn0.6w5Fv39ErR34g3WMDCO0XWdsLbwoTZQn-YD5lX7YKKc"
    );
  }

  componentDidMount() {
    fetch("/api/channels", {
      method: "GET",
      headers: this.myHeaders
    })
      .then(response => response.json())
      .then(channels => this.setState({ channels }));

    fetch("/api/users", {
      method: "GET",
      headers: this.myHeaders
    })
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
