import moment from "moment";

class MessageChangeService {
  constructor() {
    this.CHANGE_TYPE = {
      ADDED: 1,
      EDITED: 2,
      DELETED: 3
    };
  }

  setIsConsecutiveMessage(newerMessage, olderMessage) {
    if (
      newerMessage.senderId !== olderMessage.senderId ||
      olderMessage.isSystemMessage
    ) {
      newerMessage.isConsecutiveMessage = false;
      return;
    }

    let newerMessageDate = moment(newerMessage.timeString, "h:mm A");
    let olderMessageDate = moment(olderMessage.timeString, "h:mm A");
    newerMessage.isConsecutiveMessage =
      newerMessageDate - olderMessageDate <= 3 * 60 * 1000;
  }

  mergeMessageGroups(olderMessageGroups, newerMessageGroups) {
    let mergedMessageGroups = [];
    if (
      newerMessageGroups.length > 0 &&
      olderMessageGroups.length > 0 &&
      olderMessageGroups[olderMessageGroups.length - 1].dateString ===
        newerMessageGroups[0].dateString
    ) {
      // when message group has overlapping date
      let firstNewerMessageGroupMessages = newerMessageGroups[0].messages;
      let lastOlderMessageGroupMessages =
        olderMessageGroups[olderMessageGroups.length - 1].messages;
      this.setIsConsecutiveMessage(
        firstNewerMessageGroupMessages[0],
        lastOlderMessageGroupMessages[lastOlderMessageGroupMessages.length - 1]
      );
      newerMessageGroups[0].messages = lastOlderMessageGroupMessages.concat(
        firstNewerMessageGroupMessages
      );
      mergedMessageGroups = olderMessageGroups
        .slice(0, olderMessageGroups.length - 1)
        .concat(newerMessageGroups);
    } else {
      mergedMessageGroups = olderMessageGroups.concat(newerMessageGroups);
    }

    return mergedMessageGroups;
  }

  handleEditedMessageItem(messageGroupDto, currentMessageGroups) {
    let editedMessage = messageGroupDto.messages[0];
    let currentGroup = currentMessageGroups.find(
      g => g.dateString === messageGroupDto.dateString
    );
    if (!currentGroup) return;
    let currentMessageIndex = currentGroup.messages.findIndex(
      m => m.id === editedMessage.id
    );
    if (currentMessageIndex < 0) return;
    editedMessage.isConsecutiveMessage =
      currentGroup.messages[currentMessageIndex].isConsecutiveMessage;
    currentGroup.messages[currentMessageIndex] = editedMessage;

    return currentMessageGroups;
  }

  handleDeletedMessageItem(messageId, currentMessageGroups) {
    for (let group of currentMessageGroups) {
      let currentMessageIndex = group.messages.findIndex(
        m => m.id === messageId
      );
      if (currentMessageIndex < 0) continue;
      if (
        currentMessageIndex > 0 &&
        currentMessageIndex < group.messages.length - 1
      ) {
        this.setIsConsecutiveMessage(
          group.messages[currentMessageIndex + 1],
          group.messages[currentMessageIndex - 1]
        );
      } else if (
        currentMessageIndex === 0 &&
        currentMessageIndex < group.messages.length - 1
      ) {
        group.messages[currentMessageIndex + 1].isConsecutiveMessage = false;
      }
      group.messages.splice(currentMessageIndex, 1);
      if (group.messages.length === 0) {
        let currentGroupIndex = currentMessageGroups.indexOf(group);
        currentMessageGroups.splice(currentGroupIndex, 1);
      }
      return currentMessageGroups;
    }
  }
}

export default MessageChangeService;
