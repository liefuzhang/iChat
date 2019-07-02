namespace iChat.Api.Constants {
    public class iChatConstants {
        public static readonly string DefaultChannelGeneral = "general";
        public static readonly string DefaultChannelRandom = "random";
        public static readonly string IdenticonPath = "Contents\\Identicons";
        public static readonly string IdenticonExt = ".svg";
        public static readonly string DefaultIdenticonName = "Default.svg";
        public static readonly string EmailTemplatePath = "Data\\EmailTemplates";

        public static readonly int RedisRecentConversationMaxNumber = 10;

        public static readonly string RedisKeyActiveSidebarItemPrefix = "ActiveSidebarItem";
        public static readonly string RedisKeyRecentConversationPrefix = "RecentConversation";
        public static readonly string RedisKeyRecentUnreadChannelPrefix = "UnreadChannel";
        public static readonly string RedisKeyUserOnlinePrefix = "UserOnline";

        public static readonly string AwsBucketWorkspaceFileFolderPrefix = "Workspace-";

        public static readonly int DefaultMessagePageSize = 30;
    }
}
