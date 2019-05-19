using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Api.Helpers
{
    public class AppSettings
    {
        public string JwtSecret { get; set; }
        public string GmailHost { get; set; }
        public string GmailUserName { get; set; }
        public string GmailPassword { get; set; }
        public string GmailPort { get; set; }
        public string GmailSsl { get; set; }
    }
}
