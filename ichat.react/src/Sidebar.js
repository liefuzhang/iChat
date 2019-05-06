import React from "react";

class Sidebar extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      channels: []
    };
  }

  componentDidMount() {
    fetch("/api/channels")
      .then(response => response.json())
      .then(channels => this.setState({ channels }));
  }

  render() {
    return (
      <div id="sideBar">
        <section>
          <div className="section-title">CHANNELS</div>
          {this.state.channels.map(c => 
            <a>
              <div className="sidebar-item">
                #&nbsp;
                {c.name}
              </div>
            </a>
          )}
        </section>
        {/* 
        <section>
            <div className="section-title">DIRECT MESSAGES</div>
            @foreach (var user in Model.DirectMessageUsers) {
                var isActive = "";
                if (user.Id == Model.SelectedUser?.Id) {
                    isActive = "active-item";
                }
                <a asp-page="./Index" asp-route-selectedUserId="@user.Id">
                    <div className="sidebar-item @isActive">
                        #&nbsp;
                        @Html.DisplayFor(model => user.DisplayName)
                    </div>
                </a>
            }
        </section> */}
      </div>
    );
  }
}

export default Sidebar;
