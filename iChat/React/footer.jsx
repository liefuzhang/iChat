'use strict';

function Footer(props) {
    console.log(props)

    return (
        <div className="footer">
            channel: {props.selectedChannelId}
            <form id="messageForm" method="post">
                <input type="hidden" name="channelId" value={props.selectedChannelId} />
                <input type="hidden" name="newMessage" />
                <div className="message-box">
                    <div id="messageEditor">
                    </div>
                </div>
            </form>
            <MessagePrompt />
        </div>
    );
}

const domContainer = document.querySelector('#footer');
ReactDOM.render(<Footer selectedChannelId={domContainer.getAttribute('data-selected-channel-id')}
    selectedUserId={domContainer.getAttribute('data-selected-user-id')} />, domContainer);