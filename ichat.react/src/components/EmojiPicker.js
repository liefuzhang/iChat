import React from "react";
import EmojiService from "services/EmojiService";
import "lib/emoji.css";
import { NimblePicker } from "emoji-mart";
import "lib/emoji-mart.css";
import data from "emoji-mart/data/google.json";
import { Icon, Popup } from "semantic-ui-react";
import "./EmojiPicker.css";

class EmojiPicker extends React.Component {
  constructor(props) {
    super(props);

    this.emojiService = new EmojiService();
    this.onEmojiButtonClicked = this.onEmojiButtonClicked.bind(this);
    this.onEmojiKeyup = this.onEmojiKeyup.bind(this);
    this.closeEmoji = this.closeEmoji.bind(this);
    this.addEmoji = this.addEmoji.bind(this);

    this.state = {
      isEmojiOpen: false
    };
  }

  onEmojiButtonClicked() {
    this.setState({ isEmojiOpen: true }, () => {
      document.addEventListener("keyup", this.onEmojiKeyup);

      let emojiPicker = document.querySelector(".emoji-picker");
      let contentHeaderHeight = document.querySelector(".content-header")
        .offsetHeight;
      if (
        emojiPicker.getBoundingClientRect().top <
        contentHeaderHeight /* content header height */
      ) {
        let diff =
          contentHeaderHeight - emojiPicker.getBoundingClientRect().top;
        emojiPicker.style.transform = `translateY(${diff}px)`;
      }
      emojiPicker.style.visibility = "visible";
    });
  }

  onEmojiKeyup(event) {
    if (
      !event.ctrlKey &&
      !event.shiftKey &&
      !event.altKey &&
      event.keyCode === 27
    ) {
      // esc pressed
      this.closeEmoji();
    }
  }

  closeEmoji(event) {
    let emojiPicker = document.querySelector(".emoji-picker");
    if (event && emojiPicker.contains(event.target)) return;
    this.setState({ isEmojiOpen: false }, () => {
      document.removeEventListener("keyup", this.onEmojiKeyup);

      if (this.props.onClose) this.props.onClose();
    });
  }

  addEmoji(event) {
    this.closeEmoji();

    if (this.props.onEmojiHtmlAdded) {
      var imgHtml = this.emojiService.convertColonsToHtml(event.colons);
      this.props.onEmojiHtmlAdded(imgHtml);
    }
    if (this.props.onEmojiColonsAdded)
      this.props.onEmojiColonsAdded(event.colons);
  }

  render() {
    return (
      <div className="emoji-picker-root">
        <Popup
          trigger={
            <Icon
              name="smile outline"
              className="icon-emoji"
              onClick={this.onEmojiButtonClicked}
            />
          }
          content={this.props.tooltipText}
          inverted
          position="top center"
          size="tiny"
        />

        {this.state.isEmojiOpen && (
          <div className="emoji-picker-container">
            <div
              className="emoji-picker-overlay page-overlay"
              onClick={this.closeEmoji}
            />
            <div className="emoji-picker page-overlay-content">
              <NimblePicker set="google" data={data} onSelect={this.addEmoji} />
            </div>
          </div>
        )}
      </div>
    );
  }
}

export default EmojiPicker;
