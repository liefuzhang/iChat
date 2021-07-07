import React from "react";
import "./ContentMessageItem.UserReactions.css";
import EmojiService from "services/EmojiService";
import { Popup } from "semantic-ui-react";
import ApiService from "services/ApiService";
import EmojiPicker from "components/EmojiPicker";

class ContentMessageItemUserReactions extends React.Component {
  constructor(props) {
    super(props);

    this.apiService = new ApiService(props);
    this.emojiService = new EmojiService();
    this.onReactionClicked = this.onReactionClicked.bind(this);
    this.onEmojiColonsSelected = this.onEmojiColonsSelected.bind(this);
    this.onEmojiClose = this.onEmojiClose.bind(this);
  }

  onReactionClicked(reaction) {
    if (this.reactionClicked === true) return;
    this.reactionClicked = true;
    let reactedByUser = reaction.users.some(
      u => u.id === this.props.userProfile.id
    );
    if (reactedByUser) this.props.onEmojiColonsRemoved(reaction.emojiColons);
    else this.props.onEmojiColonsAdded(reaction.emojiColons);
  }

  onEmojiColonsSelected(colons) {
    if (this.reactionClicked === true) return;
    this.reactionClicked = true;
    let reaction = this.props.reactions.find(r => r.emojiColons === colons);
    if (reaction) this.onReactionClicked(reaction);
    else this.props.onEmojiColonsAdded(colons);
  }

  onHoverMessageReactions(event) {
    event.currentTarget.classList.add("message-reactions-hover");
  }

  onLeaveMessageReactions(event) {
    event.currentTarget.classList.remove("message-reactions-hover");
  }

  onEmojiClose() {
    let currentMessageReactions = document.querySelector(
      ".message-reactions-hover"
    );
    currentMessageReactions &&
      currentMessageReactions.classList.remove("message-reactions-hover");
  }

  componentDidUpdate(prevProps) {
    if (this.props.reactions !== prevProps.reactions) {
      this.reactionClicked = false;
    }
  }

  render() {
    return (
      <div
        className="message-reactions"
        onMouseOver={this.onHoverMessageReactions}
        onMouseLeave={this.onLeaveMessageReactions}
      >
        {this.props.reactions.map(reaction => {
          let reactedByYou = reaction.users.some(
            u => u.id === this.props.userProfile.id
          );
          let userNames = reaction.users
            .map(u =>
              u.id === this.props.userProfile.id ? "You" : u.displayName
            )
            .join(", ");
          return (
            <div key={reaction.id} className="message-reaction-container">
              <Popup
                trigger={
                  <div
                    className={
                      "message-reaction" +
                      (reactedByYou ? " message-reaction-reacted" : "")
                    }
                    onClick={() => this.onReactionClicked(reaction)}
                  >
                    <div
                      className="message-reaction-emoji"
                      dangerouslySetInnerHTML={{
                        __html: this.emojiService.convertColonsToHtml(
                          reaction.emojiColons
                        )
                      }}
                    />
                    <div className="message-reaction-user-count">
                      {reaction.users.length}
                    </div>
                  </div>
                }
                content={`${userNames} reacted with ${reaction.emojiColons}`}
                inverted
                position="top center"
                size="tiny"
              />
            </div>
          );
        })}
        <div className="message-reaction-container">
          <div className="message-reaction message-reaction-add-emoji">
            <EmojiPicker
              onEmojiColonsAdded={this.onEmojiColonsSelected}
              tooltipText="Add reaction"
              onClose={this.onEmojiClose}
            />
          </div>
        </div>
      </div>
    );
  }
}

export default ContentMessageItemUserReactions;
