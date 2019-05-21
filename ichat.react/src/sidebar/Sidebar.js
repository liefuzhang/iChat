import React from "react";
import "./Sidebar.css";
import SidebarItem from "./SidebarItem";
import SidebarHeader from "./SidebarHeader";
import AuthService from "services/AuthService";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Modal from "modals/Modal";
import CreateChannelForm from "modalForms/CreateChannelForm";

class Sidebar extends React.Component {
  constructor(props) {
    super(props);

    this.authService = new AuthService(props);
    this.onCreateChannel = this.onCreateChannel.bind(this);
    this.onCloseCreateChannel = this.onCloseCreateChannel.bind(this);

    this.state = {
      channels: [],
      directMessageUsers: [],
      isCreateChannelModalOpen: false
    };

    this.fecthData();
  }

  onCreateChannel(event) {
    this.setState({
      isCreateChannelModalOpen: true,
    });
  }

  onCloseCreateChannel(){
    this.setState({
      isCreateChannelModalOpen: false
    });
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
          <SidebarHeader userProfile={this.props.userProfile} />
        </section>
        <section>
          <div className="section-title">
            <span>CHANNELS</span>
            <FontAwesomeIcon
              icon="plus-circle"
              title="Create a channel"
              className="icon-circle"
              onClick={this.onCreateChannel}
            />
          </div>
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
          <div>
            {this.state.isCreateChannelModalOpen && (
              <Modal onClose={this.onCloseCreateChannel}>
                <CreateChannelForm
                  onClose={this.onCloseCreateChannel}
                />
              </Modal>
            )}
          </div>
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