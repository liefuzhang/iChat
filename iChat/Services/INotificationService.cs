using System.Threading.Tasks;

namespace iChat.Services
{
    public interface INotificationService
    {
        Task SendUpdateChannelNotification(int channelId);
        Task SendDirectMessageNotification(int receiverId);
    }
}