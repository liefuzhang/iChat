'use strict';

function MessagePrompt() {
    return (
        <div className="message-prompt">
            <b>*bold*</b>&nbsp;
            <span className="grey-background">`code`</span>&nbsp;
            <span className="grey-background">```preformatted```</span>&nbsp;
            <i>_italics_</i>&nbsp;
            <span>~strike~</span>&nbsp;
            <span>>quote</span>&nbsp;
        </div>
    );
}
