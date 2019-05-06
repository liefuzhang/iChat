import React from "react";
import "./Content.css";
import { withRouter } from "react-router-dom";
import ContentHeader from "./ContentHeader";

class Content extends React.Component {
  constructor(props) {
    super(props);

    this.params = this.props.match.params;
    this.isChannel = this.params.section === "channel";
    this.fetchData = this.fetchData.bind(this);

    this.state = {
      messages: []
    };
  }

  fetchData() {
    fetch(`/api/messages/${this.params.section}/${this.params.id}`)
      .then(response => response.json())
      .then(messages => this.setState({ messages }));
  }

  componentWillMount() {
    this.fetchData();
  }

  componentWillReceiveProps(nextProps) {
    if (
      this.props.match.params.section !== nextProps.match.params.section ||
      this.props.match.params.id !== nextProps.match.params.id
    ) {
      this.params = nextProps.match.params;
      this.isChannel = this.params.section === "channel";

      this.fetchData();
    }
  }

  render() {
    console.log(this.props);
    return (
      <div id="content">
        <ContentHeader isChannel={this.isChannel} id={this.params.id} />
        <div className="message-container">
          <div className="message-scrollable">
            @foreach (var message in Model.MessagesToDisplay){" "}
            {
              <div className="message-item">
                <div className="message-title">
                  @message.Sender.DisplayName &nbsp;
                  @message.CreatedDate.ToShortTimeString()
                </div>
                <div className="message-content">
                  @Html.Raw(message.Content)
                </div>
              </div>
            }
          </div>
        </div>

        <div className="footer">
          <form id="messageForm" method="post">
            <input
              type="hidden"
              name="channelId"
              value="@Model.SelectedChannel?.Id"
            />
            <input
              type="hidden"
              name="selectedUserId"
              value="@Model.SelectedUser?.Id"
            />
            <input type="hidden" name="newMessage" />
            <div className="message-box">
              <div id="messageEditor" />
            </div>
          </form>
          <div className="message-prompt">
            <b>*bold*</b>&nbsp;
            <span className="grey-background">`code`</span>&nbsp;
            <span className="grey-background">```preformatted```</span>&nbsp;
            <i>_italics_</i>&nbsp;
            <span>~strike~</span>&nbsp;
            <span>>quote</span>&nbsp;
          </div>
        </div>
      </div>
    );
  }
}

export default Content;
