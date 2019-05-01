using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iChat.Services {
    public class MessageParsingService : IMessageParsingService {
        private class Token {
            public Token(string tag, int index) {
                Tag = tag;
                Index = index;
            }
            public string Tag { get; }
            public int Index { get; }
        }

        public string Parse(string input) {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            var markedChanges = new List<Token>();
            var stagedTokens = new List<Token>();

            for (var i = 0; i < input.Length; i++) {
                var ch = input[i];
                switch (ch) {
                    case '*':
                        ParseChar(stagedTokens, input, i, markedChanges, "<b>", "</b>");
                        break;
                    case '_':
                        ParseChar(stagedTokens, input, i, markedChanges, "<i>", "</i>");
                        break;
                    case ' ':
                        stagedTokens.Clear();
                        break;
                }
            }

            var result = new StringBuilder();
            for (var i = 0; i < input.Length; i++) {
                var ch = input[i];
                if ((ch == '*' || ch == '_') &&
                    markedChanges.Any(mc=>mc.Index == i)) {
                    var markedChange = markedChanges.Single(mc => mc.Index == i);
                    result.Append(markedChange.Tag);
                }
                else {
                    result.Append(ch);
                }
            }

            return result.ToString();
        }

        private static void ParseChar(ICollection<Token> stagedTokens, string input, int i, 
            ICollection<Token> markedChanges, string openTag, string closeTag)
        {
            var ch = input[i];
            if (stagedTokens.All(s => s.Tag != ch.ToString())) {
                stagedTokens.Add(new Token(ch.ToString(), i));
            }
            else {
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
    }
}
