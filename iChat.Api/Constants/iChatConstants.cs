namespace iChat.Api.Constants {
    public class iChatConstants {
        public static readonly string DefaultChannelGeneral = "general";
        public static readonly string DefaultChannelRandom = "random";
        public static readonly string IdenticonPath = "Contents\\Identicons";
        public static readonly string IdenticonExt = ".svg";
        public static readonly string DefaultIdenticonName = "Default.svg";
        public static readonly string EmailTemplatePath = "Data\\EmailTemplates";

        public static readonly int CacheRecentConversationMaxNumber = 10;

        public static readonly string CacheKeyActiveSidebarItemPrefix = "ActiveSidebarItem";
        public static readonly string CacheKeyRecentConversationPrefix = "RecentConversation";
        public static readonly string CacheKeyRecentUnreadChannelPrefix = "UnreadChannel";
        public static readonly string CacheKeyUserOnlinePrefix = "UserOnline";

        public static readonly string AwsBucketWorkspaceFileFolderPrefix = "Workspace-";

        public static readonly int DefaultMessagePageSize = 30;
    }
}
