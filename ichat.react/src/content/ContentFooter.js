import React from "react";
import "./ContentFooter.css";
import ContentFooterEditor from "./ContentFooter.Editor";
import AuthService from "services/AuthService";

class ContentFooter extends React.Component {
  constructor(props) {
    super(props);

    this.authService = new AuthService(props);
    this.onOtherUserTyping = this.onOtherUserTyping.bind(this);

    if (props.hubConnection) {
      props.hubConnection.on("UserTyping", this.onOtherUserTyping);
    }

    this.otherTypingNames = [];

    this.state = {
      showOtherTypingInfo: false,
      otherTypingUserName: undefined
    };
  }

  componentDidUpdate(prevProps) {
    if (
      this.props.isChannel !== prevProps.isChannel ||
      this.props.id !== prevProps.id
    ) {
      this.otherTypingNames = [];
      this.toggleOtherTypingInfo(false);
    }
  }

  toggleOtherTypingInfo(show) {
    let names =
      this.otherTypingNames.length > 2
        ? "Multiple users"
        : this.otherTypingNames.join(", ");

    this.setState({
      showOtherTypingInfo: show,
      otherTypingUserName: names
    });
  }

  onOtherUserTyping(name, isChannel, id) {
    if (isChannel === this.props.isChannel && id === this.props.id) {
      this.otherTypingNames.push(name);
      this.toggleOtherTypingInfo(true);

      setTimeout(() => {
        let index = this.otherTypingNames.indexOf(name);
        this.otherTypingNames.splice(index, 1);
        this.toggleOtherTypingInfo(this.otherTypingNames.length > 0);
      }, 10000);
    }
  }

  render() {
    return (
      <div className="footer">
        <ContentFooterEditor {...this.props} />
        {this.state.showOtherTypingInfo && (
          <div className="message-typing-info">
            <span>{this.state.otherTypingUserName}</span>{" "}
            {this.otherTypingNames.length > 1 ? "are" : "is"} typing
          </div>
        )}
        <div className="message-prompt">
          <span>
            <b>*bold*</b>
          </span>
          <span className="grey-background">`code`</span>
          <span className="grey-background">```preformatted```</span>
          <span>
            <i>_italics_</i>
          </span>
          <span>~strike~</span>
          <span>>quote</span>
        </div>
      </div>
    );
  }
}

export default ContentFooter;
