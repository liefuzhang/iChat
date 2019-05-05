'use strict';

function Footer(props) {
    console.log(props);

    return React.createElement(
        "div",
        { className: "footer" },
        "channel: ",
        props.selectedChannelId,
        React.createElement(
            "form",
            { id: "messageForm", method: "post" },
            React.createElement("input", { type: "hidden", name: "channelId", value: props.selectedChannelId }),
            React.createElement("input", { type: "hidden", name: "newMessage" }),
            React.createElement(
                "div",
                { className: "message-box" },
                React.createElement("div", { id: "messageEditor" })
            )
        ),
        React.createElement(MessagePrompt, null)
    );
}

var domContainer = document.querySelector('#footer');
ReactDOM.render(React.createElement(Footer, { selectedChannelId: domContainer.getAttribute('data-selected-channel-id'),
    selectedUserId: domContainer.getAttribute('data-selected-user-id') }), domContainer);
