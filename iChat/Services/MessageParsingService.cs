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
            var markedChanges = new List<Token>();
            var stagedTokens = new List<Token>();

            for (var i = 0; i < input.Length; i++) {
                var ch = input[i];
                switch (ch) {
                    case '*':
                        ParseChar(stagedTokens, ch, i, markedChanges, "<b>", "</b>");
                        break;
                    case '_':
                        ParseChar(stagedTokens, ch, i, markedChanges, "<i>", "</i>");
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

        private static void ParseChar(ICollection<Token> stagedTokens, char ch, int i, 
            ICollection<Token> markedChanges, string openTag, string closeTag) {
            if (stagedTokens.All(s => s.Tag != ch.ToString())) {
                stagedTokens.Add(new Token(ch.ToString(), i));
            }
            else {
                var stagedToken = stagedTokens.Single(s => s.Tag == ch.ToString());
                stagedTokens.Remove(stagedToken);
                markedChanges.Add(new Token(openTag, stagedToken.Index));
                markedChanges.Add(new Token(closeTag, i));
            }
        }
    }
}
