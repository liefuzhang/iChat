import React from "react";
import "./ContentMessageItem.UserReaction.css";
import EmojiService from "services/EmojiService";
import { Icon, Popup } from "semantic-ui-react";

class ContentMessageItemUserReactions extends React.Component {
  constructor(props) {
    super(props);

    this.emojiService = new EmojiService();
  }

  render() {
    return (
      <div className="message-reactions">
        {this.props.reactions.map(reaction => {
          let reactedByYou = reaction.users.some(
            u => u.id === this.props.userProfile.id
          );
          let userNames = reaction.users
            .map(u => (reactedByYou ? "You" : u.displayName))
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
      </div>
    );
  }
}

export default ContentMessageItemUserReactions;
