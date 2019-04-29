using System.Threading.Tasks;

namespace iChat.Services
{
    public interface INotificationService
    {
        Task SendUpdateChannelNotification(int channelID);
    }
}