﻿namespace iChat.Api.Helpers {
    public interface IMessageParsingHelper {
        string Parse(string input);
        string Stringify(string html);
    }
}