'use strict';

function MessagePrompt() {
    return React.createElement(
        "div",
        { className: "message-prompt" },
        React.createElement(
            "b",
            null,
            "*bold*"
        ),
        "\xA0",
        React.createElement(
            "span",
            { className: "grey-background" },
            "`code`"
        ),
        "\xA0",
        React.createElement(
            "span",
            { className: "grey-background" },
            "```preformatted```"
        ),
        "\xA0",
        React.createElement(
            "i",
            null,
            "_italics_"
        ),
        "\xA0",
        React.createElement(
            "span",
            null,
            "~strike~"
        ),
        "\xA0",
        React.createElement(
            "span",
            null,
            ">quote"
        ),
        "\xA0"
    );
}
