import React from "react";
import { EmojiConvertor } from "emoji-js";
import "lib/emoji.css";
import { NimblePicker } from "emoji-mart";
import "lib/emoji-mart.css";
import data from "emoji-mart/data/google.json";
import { Icon, Popup } from "semantic-ui-react";

class EmojiPicker extends React.Component {
  constructor(props) {
    super(props);

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
      document.addEventListener("click", this.closeEmoji);
      document.addEventListener("keyup", this.onEmojiKeyup);
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
      document.removeEventListener("click", this.closeEmoji);
      document.removeEventListener("keyup", this.onEmojiKeyup);
    });
  }

  addEmoji(event) {
    var emoji = new EmojiConvertor();
    emoji.img_sets.google.path =
      "https://unpkg.com/emoji-datasource-google@4.0.4/img/google/sheets-256/";
    emoji.img_sets.google.sheet =
      "https://unpkg.com/emoji-datasource-google@4.0.4/img/google/sheets-256/64.png";
    emoji.img_set = "google";
    emoji.use_sheet = true;
    var imgHtml = emoji.replace_colons(event.colons);
    this.closeEmoji();

    if (this.props.onEmojiHtmlAdded) this.props.onEmojiHtmlAdded(imgHtml);
    if (this.props.onEmojiColonsAdded)
      this.props.onEmojiColonsAdded(event.colons);
  }

  render() {
    return (
      <span>
        <Icon
          name="smile outline"
          className="icon-emoji"
          onClick={this.onEmojiButtonClicked}
        />
        {this.state.isEmojiOpen && (
          <div className="emoji-picker">
            <NimblePicker set="google" data={data} onSelect={this.addEmoji} />
          </div>
        )}
      </span>
    );
  }
}

export default EmojiPicker;
