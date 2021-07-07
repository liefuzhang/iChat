import React from "react";
import EmojiService from "services/EmojiService";
import "lib/emoji.css";
import { NimblePicker } from "emoji-mart";
import "lib/emoji-mart.css";
import data from "emoji-mart/data/google.json";
import { Icon, Popup } from "semantic-ui-react";
import "./EmojiPicker.css";
import BlankModalWithPageOverlay from "modals/BlankModalWithPageOverlay";

class EmojiPicker extends React.Component {
  constructor(props) {
    super(props);

    this.emojiService = new EmojiService();
    this.onEmojiIconClick = this.onEmojiIconClick.bind(this);
    this.closeEmoji = this.closeEmoji.bind(this);
    this.addEmoji = this.addEmoji.bind(this);

    this.state = {
      isEmojiOpen: false
    };
  }

  onEmojiIconClick(event) {
    this.clickedTarget = event.currentTarget;
    this.setState({ isEmojiOpen: true });
  }

  closeEmoji() {
    this.setState({ isEmojiOpen: false }, () => {
      this.clickedTarget = undefined;
      if (this.props.onClose) this.props.onClose();
    });
  }

  addEmoji(event) {
    this.setState({ isEmojiOpen: false });

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
              onClick={this.onEmojiIconClick}
            />
          }
          content={this.props.tooltipText}
          inverted
          position="top center"
          size="tiny"
        />

        {this.state.isEmojiOpen && (
          <div className="emoji-picker-container">
            <BlankModalWithPageOverlay
              clickedTarget={this.clickedTarget}
              onClose={this.closeEmoji}
            >
              <NimblePicker set="google" data={data} onSelect={this.addEmoji} />
            </BlankModalWithPageOverlay>
          </div>
        )}
      </div>
    );
  }
}

export default EmojiPicker;
