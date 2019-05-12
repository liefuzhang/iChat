namespace iChat.Api.Services {
    public interface IWorkspaceService {
        int Register(string name);
        void UpdateOwnerId(int workspaceId, int userId);
    }
}