import React from "react";
import "./Content.css";
import ContentHeader from "./ContentHeader";
import ContentMessages from "./ContentMessages";

class Content extends React.Component {
  constructor(props) {
    super(props);

    this.params = this.props.match.params;
    this.isChannel = this.params.section === "channel";

    this.state = {
      messages: []
    };
  }

  componentWillReceiveProps(nextProps) {
    if (
      this.props.match.params.section !== nextProps.match.params.section ||
      this.props.match.params.id !== nextProps.match.params.id
    ) {
      this.params = nextProps.match.params;
      this.isChannel = this.params.section === "channel";
    }
  }

  render() {
    return (
      <div id="content">
        <ContentHeader isChannel={this.isChannel} id={this.params.id} />
        <ContentMessages section={this.params.section} id={this.params.id} />

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
