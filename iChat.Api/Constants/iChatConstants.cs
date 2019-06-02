using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChat.Api.Constants
{
    public class iChatConstants
    {
        public static readonly string DefaultChannelGeneral = "general";
        public static readonly string DefaultChannelRandom = "random";
        public static readonly int DefaultChannelIdInRequest = 0;
        public static readonly string IdenticonPath = "Contents\\Identicons";
        public static readonly string IdenticonExt = ".svg";
        public static readonly string DefaultIdenticonName = "Default.svg";
        public static readonly string EmailTemplatePath = "Data\\EmailTemplates";

        public static readonly string RedisKeyActiveSidebarItemPrefix = "ActiveSidebarItem";
    }
}
