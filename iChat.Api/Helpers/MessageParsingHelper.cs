using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace iChat.Api.Helpers
{
    public class MessageParsingHelper : IMessageParsingHelper
    {
        private class Token
        {
            public Token(string tag, int index)
            {
                Tag = tag;
                Index = index;
            }
            public string Tag { get; }
            public int Index { get; }
        }

        public string Parse(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            var preFormattedRanges = new List<(int start, int end)>();
            input = HandlePreformatted(input, preFormattedRanges);

            input = HandleQuote(input);

            var markedChanges = new List<Token>();
            var stagedTokens = new List<Token>();

            for (var i = 0; i < input.Length; i++)
            {
                // skip all chars between <pre> and </pre>
                if (preFormattedRanges.Any(r => r.start < i && r.end > i))
                {
                    continue;
                }

                var ch = input[i];
                switch (ch)
                {
                    case '*':
                        ParseChar(stagedTokens, input, i, markedChanges, "<b>", "</b>");
                        break;
                    case '_':
                        ParseChar(stagedTokens, input, i, markedChanges, "<i>", "</i>");
                        break;
                    case '~':
                        ParseChar(stagedTokens, input, i, markedChanges, "<strike>", "</strike>");
                        break;
                    case '`':
                        ParseChar(stagedTokens, input, i, markedChanges, "<code>", "</code>");
                        break;
                    case ' ':
                    case '<':
                    case '>':
                        stagedTokens.Clear();
                        break;
                }
            }

            input = HandleUrlLink(input);

            var result = new StringBuilder();
            for (var i = 0; i < input.Length; i++)
            {
                var ch = input[i];
                if ((ch == '*' || ch == '_' || ch == '~' || ch == '`') &&
                    markedChanges.Any(mc => mc.Index == i))
                {
                    var markedChange = markedChanges.Single(mc => mc.Index == i);
                    result.Append(markedChange.Tag);
                }
                else
                {
                    result.Append(ch);
                }
            }

            return result.ToString();
        }

        private static string HandlePreformatted(string input, List<(int start, int end)> preFormattedRanges)
        {
            // special handling for ```preformatted```
            var pattern = @"(<p>```)((?:.)+?)(```</p>)";
            input = Regex.Replace(input, pattern, "<pre>$2</pre>");

            var regex = new Regex(@"(<pre>)((?:.)+?)(</pre>)");
            var matches = regex.Matches(input);
            foreach (Match match in matches)
            {
                // first group is the entire matched string
                preFormattedRanges.Add((match.Groups[1].Index, match.Groups[3].Index));
            }

            return input;
        }

        private static string HandleQuote(string input)
        {
            var pattern = @"(<p>&gt;)((?:.)+?)(</p>)";
            input = Regex.Replace(input, pattern, "<blockquote>$2</blockquote>");

            return input;
        }

        private static void ParseChar(ICollection<Token> stagedTokens, string input, int i,
            ICollection<Token> markedChanges, string openTag, string closeTag)
        {
            var ch = input[i];
            if (stagedTokens.All(s => s.Tag != ch.ToString()))
            {
                stagedTokens.Add(new Token(ch.ToString(), i));
            }
            else
            {
                var matchedToken = stagedTokens.Single(s => s.Tag == ch.ToString());
                stagedTokens.Remove(matchedToken);

                if (matchedToken.Index == i - 1)
                {
                    // when **, simply skip
                    return;
                }

                // we only allow double tags such as _*string*_
                var tokenBefore = stagedTokens.SingleOrDefault(t => t.Index == matchedToken.Index - 1);

                stagedTokens.Clear();
                if (tokenBefore != null && i != input.Length - 1 && tokenBefore.Tag == input[i + 1].ToString())
                {
                    stagedTokens.Add(tokenBefore);
                }

                markedChanges.Add(new Token(openTag, matchedToken.Index));
                markedChanges.Add(new Token(closeTag, i));
            }
        }

        private string HandleUrlLink(string input)
        {
            var urlPattern = @"(https?:\/\/)(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&;/=]*)";
            return Regex.Replace(input, urlPattern, "<a href=\"$&\" target=\"_blank\">$&</a>");
        }

        public string Stringify(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return string.Empty;
            }

            html = StringifyUrlLink(html);

            html = StringifyTag(html, "<b>", "</b>", '*');
            html = StringifyTag(html, "<i>", "</i>", '_');
            html = StringifyTag(html, "<strike>", "</strike>", '~');
            html = StringifyTag(html, "<code>", "</code>", '`');

            html = StringifyQuoteTag(html);
            html = StringifyPreformmatedTag(html);

            return html;
        }

        private string StringifyUrlLink(string html) {
            var pattern = "<a href=\"(.*?)\" target=\"_blank\">.*?<\\/a>";
            html = Regex.Replace(html, pattern, "$1");

            return html;
        }

        private string StringifyTag(string html, string openTag, string closeTag, char replacingChar)
        {
            var pattern = $@"({openTag})((?:\S)+?)({closeTag})";
            html = Regex.Replace(html, pattern, $"{replacingChar}$2{replacingChar}");

            return html;
        }

        private string StringifyQuoteTag(string html)
        {
            var pattern = @"(<blockquote>)((?:.)+?)(</blockquote>)";
            html = Regex.Replace(html, pattern, $"<p>&gt;$2</p>");

            return html;
        }

        private string StringifyPreformmatedTag(string html)
        {
            var pattern = @"(<pre>)((?:.)+?)(</pre>)";
            html = Regex.Replace(html, pattern, $"<p>```$2```</p>");

            return html;
        }
    }
}
