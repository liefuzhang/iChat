using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public interface INotificationService
    {
        Task SendUpdateChannelNotification(int channelId);
        Task SendDirectMessageNotification(int receiverId);
    }
}